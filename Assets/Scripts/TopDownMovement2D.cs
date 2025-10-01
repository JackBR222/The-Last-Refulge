using UnityEngine;

public class TopDownMovement2D : MonoBehaviour
{
    [Header("Movimentação")]
    [Tooltip("Velocidade em unidades por segundo.")]
    public float moveSpeed = 5f;

    [Header("Referências")]
    public Rigidbody2D rb;
    public Animator animator; // opcional

    // Cache de entrada
    private Vector2 rawInput;

    // Controla se o jogador pode se mover
    public bool canMove = true; // Adiciona o booleano para ativar/desativar movimento

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (!canMove)
        {
            // Zera a entrada quando o movimento é desativado
            rawInput = Vector2.zero;
            return; // Ignora qualquer entrada do jogador
        }

        // Entrada do jogador (WASD ou Setas)
        rawInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        rawInput = Vector2.ClampMagnitude(rawInput, 1f);

        // Atualiza parâmetros no Animator, se existir
        if (animator != null)
        {
            animator.SetFloat("MoveX", rawInput.x);
            animator.SetFloat("MoveY", rawInput.y);
            animator.SetFloat("Speed", rawInput.sqrMagnitude);
        }
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            // Garante que o movimento pare imediatamente, desativando a velocidade
            rb.linearVelocity = Vector2.zero;
            return; // Não aplica nenhum movimento no FixedUpdate
        }

        // Move o personagem diretamente com base na entrada
        rb.linearVelocity = rawInput * moveSpeed;
    }
}
