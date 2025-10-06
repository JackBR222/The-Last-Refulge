using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownMovement2D : MonoBehaviour
{
    [Header("Movimentação")]
    public float moveSpeed = 5f;

    [Header("Referências")]
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 rawInput;
    public bool canMove = true;

    private PlayerInput playerInput;
    private InputAction moveAction;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        // Obtém a referência ao PlayerInput e à ação de movimento
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    void Update()
    {
        if (!canMove) return;

        // Entrada do jogador (novo sistema de input)
        rawInput = moveAction.ReadValue<Vector2>().normalized;

        // Atualiza parâmetros no Animator, se houver
        if (animator != null)
        {
            animator.SetFloat("MoveX", rawInput.x);
            animator.SetFloat("MoveY", rawInput.y);
            animator.SetFloat("Speed", rawInput.sqrMagnitude);
        }
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            // Aplica o movimento imediato sem aceleração
            rb.linearVelocity = rawInput * moveSpeed;
        }
        else
        {
            // Garante que o movimento pare ao desabilitar o movimento
            rb.linearVelocity = Vector2.zero;
        }
    }
}
