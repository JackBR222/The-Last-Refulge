using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class NPCFollower : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private Transform player;

    [Header("Configurações de movimento")]
    [SerializeField] private float followSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float followDistanceThreshold = 5f;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private float closeEnoughDistance = 0.5f;
    [SerializeField] private bool canRun = true;

    [Header("Evento quando estiver longe demais")]
    [SerializeField] private float tooFarDistance = 10f;
    public UnityEvent OnTooFarFromPlayer;

    [Header("Referências do Rigidbody e Animator")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    private Collider2D npcCollider;
    private Collider2D playerCollider;
    private Vector2 directionToPlayer;
    private float speed;

    private bool shouldFollow = false;
    private bool hasTriggeredTooFarEvent = false;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();

        npcCollider = GetComponent<Collider2D>();
        playerCollider = player.GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        if (shouldFollow)
        {
            FollowPlayer();
        }

        UpdateAnimator();
    }

    private void FollowPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > tooFarDistance)
        {
            if (!hasTriggeredTooFarEvent)
            {
                hasTriggeredTooFarEvent = true;
                OnTooFarFromPlayer?.Invoke();
            }
        }
        else
        {
            hasTriggeredTooFarEvent = false;
        }

        if (canRun && distanceToPlayer > followDistanceThreshold)
        {
            speed = runSpeed;
        }
        else
        {
            speed = followSpeed;
        }

        if (distanceToPlayer > stoppingDistance)
        {
            directionToPlayer = (player.position - transform.position).normalized;
            rb.linearVelocity = directionToPlayer * speed;

            if (distanceToPlayer <= closeEnoughDistance)
            {
                DisableCollider();
            }
            else
            {
                EnableCollider();
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            EnableCollider();
        }
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        float currentSpeed = rb.linearVelocity.magnitude;

        animator.SetFloat("Speed", currentSpeed);

        if (currentSpeed > 0)
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("MoveX", directionToPlayer.x);
            animator.SetFloat("MoveY", directionToPlayer.y);
            animator.SetBool("IsRunning", speed == runSpeed && canRun);
        }
        else
        {
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsRunning", false);
        }
    }

    public void StartFollowing()
    {
        shouldFollow = true;
        DisableColliderForPlayer();
    }

    public void StopFollowing()
    {
        shouldFollow = false;
        rb.linearVelocity = Vector2.zero;
        EnableCollider();
        EnableColliderForPlayer();
    }

    private void DisableColliderForPlayer()
    {
        if (npcCollider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(npcCollider, playerCollider, true);
        }
    }

    private void EnableColliderForPlayer()
    {
        if (npcCollider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(npcCollider, playerCollider, false);
        }
    }

    private void DisableCollider()
    {
        if (npcCollider != null)
        {
            npcCollider.enabled = false;
        }
    }

    private void EnableCollider()
    {
        if (npcCollider != null)
        {
            npcCollider.enabled = true;
        }
    }
}
