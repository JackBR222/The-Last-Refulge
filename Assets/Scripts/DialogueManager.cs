using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [Header("Referências de UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image characterImage;

    [Header("Falas")]
    [TextArea(2, 5)]
    [SerializeField] private string[] lines;

    [Header("Sprites do personagem")]
    [SerializeField] private Sprite[] characterSprites;

    [Header("Comportamento")]
    [SerializeField] private bool autoStart = false;
    [SerializeField] private bool useTypewriter = false;
    [SerializeField, Range(0.01f, 0.1f)] private float charDelay = 0.02f;

    [Header("Eventos")]
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

    private int index = 0; // Inicializa o índice com 0
    private bool isRunning = false;
    private Coroutine typingRoutine;

    private void Awake()
    {
        // Verifica se o botão 'nextButton' está configurado
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(Next);
        }
        else
        {
            Debug.LogError("[DialogueManager] nextButton não foi atribuído!");
        }

        // Verifica se o painel de diálogo está configurado
        if (dialoguePanel == null)
        {
            Debug.LogError("[DialogueManager] dialoguePanel não foi atribuído!");
        }

        // Certifique-se de que o painel de diálogo esteja oculto ao iniciar
        dialoguePanel?.SetActive(false);
    }

    private void Start()
    {
        if (autoStart) StartDialogue();
    }

    public void StartDialogue()
    {
        Debug.Log("Iniciando o diálogo...");

        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning("[DialogueManager] Nenhuma fala configurada.");
            return;
        }

        index = 0; // Inicializa o índice corretamente
        isRunning = true;

        // Verifique se o painel de diálogo foi atribuído corretamente
        if (dialoguePanel == null)
        {
            Debug.LogError("[DialogueManager] dialoguePanel não foi atribuído!");
            return;
        }

        // Garantir que o painel de diálogo seja ativado corretamente
        dialoguePanel.SetActive(true);
        Debug.Log("Painel de diálogo ativado.");

        // Certifique-se de que o botão de "Next" também está ativo
        if (nextButton != null)
        {
            nextButton.gameObject.SetActive(true);  // Ativa o botão se estiver configurado
            Debug.Log("Botão 'Next' ativado.");
        }

        onDialogueStart?.Invoke();
        Next();
    }

    public void SetDialogueLines(string[] newLines)
    {
        lines = newLines;
        StartDialogue();  // Inicia o diálogo com as novas falas
    }

    public void SetCharacterSprites(Sprite[] newSprites)
    {
        characterSprites = newSprites;  // Recebe os sprites específicos para o NPC
    }

    public void SetCharacterSprite(Sprite newSprite)
    {
        if (characterImage != null)
        {
            characterImage.sprite = newSprite;
            characterImage.gameObject.SetActive(newSprite != null);
        }
        else
        {
            Debug.LogError("[DialogueManager] characterImage não foi atribuído!");
        }
    }
    public void Next()
    {
        if (!isRunning) return;

        // Checando se o índice está dentro dos limites
        if (index < 0 || index >= lines.Length)
        {
            Debug.LogWarning("[DialogueManager] Índice de fala inválido ou não há mais falas.");
            EndDialogue();
            return;
        }

        // Exibe a fala com ou sem efeito de digitação
        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        // Atualiza o sprite do personagem
        if (index < characterSprites.Length && characterSprites[index] != null)
        {
            SetCharacterSprite(characterSprites[index]);
        }
        else
        {
            SetCharacterSprite(null); // Se não houver sprite, desativa a imagem
        }

        // Exibe a fala com ou sem efeito de digitação
        if (useTypewriter)
            typingRoutine = StartCoroutine(TypeText(lines[index]));
        else
            dialogueText.text = lines[index];

        // Incrementa o índice para a próxima fala
        index++;
    }

    private IEnumerator TypeText(string line)
    {
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(charDelay);
        }
        typingRoutine = null;
    }

    private void EndDialogue()
    {
        // Espera um pequeno tempo após a última fala antes de finalizar
        StartCoroutine(EndDialogueWithDelay());
    }

    private IEnumerator EndDialogueWithDelay()
    {
        // Espera um pouco após a última fala para dar tempo de visualização
        yield return new WaitForSeconds(0f);  // Ajuste o tempo se necessário
        isRunning = false;
        dialoguePanel?.SetActive(false);
        onDialogueEnd?.Invoke();
    }
}
