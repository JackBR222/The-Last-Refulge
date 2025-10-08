using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class NPCInteractDialogue : MonoBehaviour
{
    [Header("Configuração de Interação")]
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private Sprite[] npcSprites;

    [Header("Condição de Direção")]
    [Tooltip("Ângulo máximo permitido entre a direção que o player está olhando e o NPC para permitir interação.")]
    [Range(10f, 90f)]
    [SerializeField] private float maxFacingAngle = 35f; // em graus

    private bool playerInRange = false;

    private PlayerInput playerInput;
    private InputAction interactAction;
    private Transform playerTransform;
    private Animator playerAnimator;
    private PlayerMove playerMove;

    private void Awake()
    {
        // Localiza o jogador e componentes relevantes
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("[NPCInteractDialogue] Jogador não encontrado! Verifique se ele tem a tag 'Player'.");
            return;
        }

        playerInput = player.GetComponent<PlayerInput>();
        playerTransform = player.transform;
        playerMove = player.GetComponent<PlayerMove>();

        // Procura o Animator no filho chamado 'Sprite'
        Transform spriteChild = playerTransform.Find("Sprite");
        if (spriteChild != null)
        {
            playerAnimator = spriteChild.GetComponent<Animator>();
            if (playerAnimator == null)
            {
                Debug.LogError("[NPCInteractDialogue] Animator não encontrado no objeto filho 'Sprite'.");
            }
        }
        else
        {
            Debug.LogError("[NPCInteractDialogue] Objeto filho 'Sprite' não encontrado no Player.");
        }

        if (playerInput == null)
            Debug.LogError("[NPCInteractDialogue] PlayerInput não encontrado no jogador!");
        if (playerMove == null)
            Debug.LogWarning("[NPCInteractDialogue] PlayerMove não encontrado! Última direção não será usada.");

        interactAction = playerInput?.actions["Interact"];
        if (interactAction == null)
            Debug.LogError("[NPCInteractDialogue] A ação 'Interact' não foi encontrada nas configurações de Input!");
    }

    private void Update()
    {
        if (!playerInRange || interactAction == null)
            return;

        if (interactAction.triggered && PlayerIsFacingNPC())
        {
            StartDialogue();
        }
    }

    private bool PlayerIsFacingNPC()
    {
        if (playerTransform == null || playerAnimator == null)
            return false;

        // Vetor do player para o NPC
        Vector2 toNPC = (transform.position - playerTransform.position).normalized;

        // Direção que o player está olhando (Animator do filho Sprite)
        Vector2 playerFacing = new Vector2(
            playerAnimator.GetFloat("MoveX"),
            playerAnimator.GetFloat("MoveY")
        );

        // Se parado, usa última direção do PlayerMove
        if (playerFacing == Vector2.zero && playerMove != null)
        {
            playerFacing = playerMove.LastDirection;
        }

        playerFacing.Normalize();

        float angle = Vector2.Angle(playerFacing, toNPC);

        // Debug para visualização no Scene view
        Debug.DrawLine(playerTransform.position, transform.position, Color.yellow); // linha player->NPC
        Debug.DrawRay(playerTransform.position, playerFacing * 1.2f, Color.green);   // direção olhando player

        Debug.Log($"[NPCInteractDialogue] Angle between playerFacing and toNPC: {angle}°");
        Debug.Log($"[NPCInteractDialogue] playerFacing: {playerFacing}, toNPC: {toNPC}");

        // Permite interação se estiver olhando dentro do ângulo permitido
        return angle <= maxFacingAngle;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    private void StartDialogue()
    {
        Debug.Log("[NPCInteractDialogue] Interagindo com o NPC...");

        if (dialogueManager != null && dialogueLines.Length > 0)
        {
            dialogueManager.SetDialogueLines(dialogueLines);
            dialogueManager.SetCharacterSprites(npcSprites);
            dialogueManager.StartDialogue();
        }
        else
        {
            Debug.LogWarning("[NPCInteractDialogue] Não há falas configuradas ou DialogueManager não encontrado.");
        }
    }
}
