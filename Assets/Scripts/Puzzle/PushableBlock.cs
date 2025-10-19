using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isMoving = false;
    private Vector2 targetPosition;
    private float moveCooldown = 0.2f;
    private float lastMoveTime = -1f;

    private BoxCollider2D boxCollider;
    public LayerMask obstacleLayers;

    private void Start()
    {
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        targetPosition = transform.position;

        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogError("PushableBlock precisa de um BoxCollider2D!");
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isMoving || Time.time - lastMoveTime < moveCooldown) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 pushDirection = GetPushDirection(collision);
            if (pushDirection != Vector2.zero)
            {
                Vector2 desiredPos = (Vector2)transform.position + pushDirection;

                if (CanMoveTo(desiredPos))
                {
                    targetPosition = desiredPos;
                    isMoving = true;
                    lastMoveTime = Time.time;

                    Debug.DrawRay(transform.position, pushDirection, Color.red, 0.5f);
                }
            }
        }
    }

    Vector2 GetPushDirection(Collision2D collision)
    {
        Vector2 pushDir = (transform.position - collision.transform.position).normalized;

        if (Mathf.Abs(pushDir.x) > Mathf.Abs(pushDir.y))
            return new Vector2(Mathf.Sign(pushDir.x), 0);
        else
            return new Vector2(0, Mathf.Sign(pushDir.y));
    }

    bool CanMoveTo(Vector2 targetPos)
    {
        if (boxCollider == null) return false;

        Vector2 boxSize = boxCollider.size * 0.9f;

        Collider2D hit = Physics2D.OverlapBox(targetPos, boxSize, 0f, obstacleLayers);

        return hit == null;
    }

    private void OnDrawGizmosSelected()
    {
        if (boxCollider == null) return;

        Gizmos.color = Color.cyan;
        Vector2 boxSize = boxCollider.size * 0.9f;
        Gizmos.DrawWireCube(targetPosition, boxSize);
    }
}