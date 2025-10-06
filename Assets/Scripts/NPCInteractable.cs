using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteractable : MonoBehaviour
{
    [Header("Configuração de Interação")]
    [SerializeField] private string[] dialogueLines; // Fala para esse NPC específico
    [SerializeField] private DialogueManager dialogueManager; // Referência ao DialogueManager
    [SerializeField] private Sprite[] npcSprites; // Sprites do NPC, que vão ser usados conforme a fala

    private bool playerInRange = false; // Controla se o jogador está dentro do alcance

    private PlayerInput playerInput;  // Referência para o PlayerInput
    private InputAction interactAction; // Ação de interação do novo sistema de Input

    private void Awake()
    {
        // Verifica se o PlayerInput já está atribuído
        playerInput = GameObject.FindWithTag("Player")?.GetComponent<PlayerInput>();

        // Verifica se o PlayerInput foi encontrado
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput não encontrado! Verifique se o componente PlayerInput está anexado ao GameObject do jogador.");
            return;
        }

        // Obtém a ação de interação
        interactAction = playerInput.actions["Interact"];

        // Verifica se a ação de interação foi configurada corretamente
        if (interactAction == null)
        {
            Debug.LogError("A ação 'Interact' não foi encontrada nas configurações de Input!");
        }
    }

    private void Update()
    {
        // Verifica se o jogador está dentro do alcance e pressiona o botão de interação
        if (playerInRange && interactAction.triggered)
        {
            StartDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detecta quando o jogador entra na área de interação
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Detecta quando o jogador sai da área de interação
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void StartDialogue()
    {
        Debug.Log("Interagindo com o NPC...");

        // Verifica se o DialogueManager existe e se as falas estão configuradas
        if (dialogueManager != null && dialogueLines.Length > 0)
        {
            Debug.Log("Falas encontradas, iniciando diálogo...");
            // Passa as falas e os sprites para o DialogueManager
            dialogueManager.SetDialogueLines(dialogueLines);
            dialogueManager.SetCharacterSprites(npcSprites);

            // Inicia o diálogo
            dialogueManager.StartDialogue();
        }
        else
        {
            Debug.LogWarning("[NPCInteractable] Não há falas configuradas ou DialogueManager não encontrado.");
        }
    }
}
