using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class NPCFollowPlayer : MonoBehaviour
{
    [Header("Refer�ncias")]
    [SerializeField] private Transform player;  // Refer�ncia ao jogador
    [SerializeField] private float followSpeed = 3f;  // Velocidade de seguimento normal (andar)
    [SerializeField] private float runSpeed = 6f;  // Velocidade de seguimento r�pido (correr)
    [SerializeField] private float followDistanceThreshold = 5f;  // Dist�ncia a partir da qual o NPC come�a a correr
    [SerializeField] private float stoppingDistance = 1f;  // Dist�ncia para parar de seguir
    [SerializeField] private float closeEnoughDistance = 0.5f;  // Dist�ncia para desativar colis�o

    [Header("Refer�ncias do Rigidbody e Animator")]
    [SerializeField] private Rigidbody2D rb;  // Refer�ncia ao Rigidbody do NPC
    [SerializeField] private Animator animator;  // Refer�ncia ao Animator do NPC

    private Collider2D npcCollider;  // Refer�ncia ao Collider do NPC
    private Collider2D playerCollider; // Refer�ncia ao Collider do jogador
    private Vector2 directionToPlayer;
    private float speed;

    private bool shouldFollow = false;  // Flag para controlar o movimento do NPC

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponent<Animator>();
        npcCollider = GetComponent<Collider2D>();  // Obt�m o Collider do NPC
        playerCollider = player.GetComponent<Collider2D>();  // Obt�m o Collider do jogador

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
        // Verifica a dist�ncia entre o NPC e o jogador
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Atualiza a velocidade de acordo com a dist�ncia
        if (distanceToPlayer > followDistanceThreshold)
        {
            // Se o NPC estiver muito longe, ele come�a a correr
            speed = runSpeed;
        }
        else
        {
            // Se o NPC estiver perto o suficiente, ele anda
            speed = followSpeed;
        }

        // Se o NPC estiver mais distante que o "stoppingDistance", ele se move
        if (distanceToPlayer > stoppingDistance)
        {
            directionToPlayer = (player.position - transform.position).normalized;
            rb.linearVelocity = directionToPlayer * speed; // Usando rb.velocity, n�o linearVelocity

            // Desativa a colis�o quando o NPC est� suficientemente perto do jogador
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
            rb.linearVelocity = Vector2.zero;  // Se estiver perto o suficiente, o NPC para de se mover
            EnableCollider();  // Garante que a colis�o � ativada quando o NPC para
        }
    }

    private void UpdateAnimator()
    {
        if (animator == null) return;

        // Atualiza a anima��o com base na dire��o e velocidade do NPC
        animator.SetFloat("Speed", rb.linearVelocity.magnitude);

        if (rb.linearVelocity.magnitude > 0)
        {
            animator.SetBool("IsMoving", true);
            animator.SetFloat("MoveX", directionToPlayer.x);
            animator.SetFloat("MoveY", directionToPlayer.y);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }

    // Adiciona o m�todo StartFollowing
    public void StartFollowing()
    {
        shouldFollow = true;  // Inicia o movimento do NPC
        DisableColliderForPlayer(); // Desabilita a colis�o entre NPC e jogador
    }

    // Adiciona o m�todo StopFollowing
    public void StopFollowing()
    {
        shouldFollow = false;  // Para o movimento do NPC
        rb.linearVelocity = Vector2.zero;  // Para de mover o NPC
        EnableCollider();  // Garante que a colis�o � ativada quando parar de seguir
        EnableColliderForPlayer(); // Restaura a colis�o entre NPC e jogador
    }

    // Fun��o para desativar a colis�o do NPC com o jogador
    private void DisableColliderForPlayer()
    {
        if (npcCollider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(npcCollider, playerCollider, true); // Ignora a colis�o
        }
    }

    // Fun��o para ativar a colis�o do NPC com o jogador
    private void EnableColliderForPlayer()
    {
        if (npcCollider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(npcCollider, playerCollider, false); // Restaura a colis�o
        }
    }

    // Fun��o para desativar o Collider do NPC
    private void DisableCollider()
    {
        if (npcCollider != null)
        {
            npcCollider.enabled = false;  // Desativa a colis�o do NPC com outros objetos
        }
    }

    // Fun��o para ativar o Collider do NPC
    private void EnableCollider()
    {
        if (npcCollider != null)
        {
            npcCollider.enabled = true;  // Ativa a colis�o do NPC com outros objetos
        }
    }
}
