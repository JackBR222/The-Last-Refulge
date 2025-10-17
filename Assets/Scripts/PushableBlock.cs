using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isMoving = false;
    private Vector2 targetPosition;
    private float moveCooldown = 0.2f;
    private float lastMoveTime = -1f;

    private void Start()
    {
        transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        targetPosition = transform.position;
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
                }
            }
        }
    }

    Vector2 GetPushDirection(Collision2D collision)
    {
        Vector2 direction = collision.contacts[0].normal;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            return new Vector2(Mathf.Sign(direction.x), 0);
        else
            return new Vector2(0, Mathf.Sign(direction.y));
    }


    bool CanMoveTo(Vector2 targetPos)
    {
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.4f);
        return hit == null;
    }
}



