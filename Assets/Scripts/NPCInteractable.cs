using UnityEngine;
using UnityEngine.InputSystem;

public class NPCInteractable : MonoBehaviour
{
    [Header("Configura��o de Intera��o")]
    [SerializeField] private string[] dialogueLines; // Fala para esse NPC espec�fico
    [SerializeField] private DialogueManager dialogueManager; // Refer�ncia ao DialogueManager
    [SerializeField] private Sprite[] npcSprites; // Sprites do NPC, que v�o ser usados conforme a fala

    private bool playerInRange = false; // Controla se o jogador est� dentro do alcance

    private PlayerInput playerInput;  // Refer�ncia para o PlayerInput
    private InputAction interactAction; // A��o de intera��o do novo sistema de Input

    private void Awake()
    {
        // Verifica se o PlayerInput j� est� atribu�do
        playerInput = GameObject.FindWithTag("Player")?.GetComponent<PlayerInput>();

        // Verifica se o PlayerInput foi encontrado
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput n�o encontrado! Verifique se o componente PlayerInput est� anexado ao GameObject do jogador.");
            return;
        }

        // Obt�m a a��o de intera��o
        interactAction = playerInput.actions["Interact"];

        // Verifica se a a��o de intera��o foi configurada corretamente
        if (interactAction == null)
        {
            Debug.LogError("A a��o 'Interact' n�o foi encontrada nas configura��es de Input!");
        }
    }

    private void Update()
    {
        // Verifica se o jogador est� dentro do alcance e pressiona o bot�o de intera��o
        if (playerInRange && interactAction.triggered)
        {
            StartDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detecta quando o jogador entra na �rea de intera��o
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Detecta quando o jogador sai da �rea de intera��o
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void StartDialogue()
    {
        Debug.Log("Interagindo com o NPC...");

        // Verifica se o DialogueManager existe e se as falas est�o configuradas
        if (dialogueManager != null && dialogueLines.Length > 0)
        {
            Debug.Log("Falas encontradas, iniciando di�logo...");
            // Passa as falas e os sprites para o DialogueManager
            dialogueManager.SetDialogueLines(dialogueLines);
            dialogueManager.SetCharacterSprites(npcSprites);

            // Inicia o di�logo
            dialogueManager.StartDialogue();
        }
        else
        {
            Debug.LogWarning("[NPCInteractable] N�o h� falas configuradas ou DialogueManager n�o encontrado.");
        }
    }
}
