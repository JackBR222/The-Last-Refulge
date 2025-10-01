using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Referências de UI")]
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private Image characterImage; // Nova referência para o sprite do personagem

    [Header("Falas")]
    [TextArea(2, 5)]
    [SerializeField] private string[] lines;

    [Header("Sprites do personagem")]
    [SerializeField] private Sprite[] characterSprites; // Array de sprites para cada fala

    [Header("Comportamento")]
    [SerializeField] private bool autoStart = false;
    [SerializeField] private bool useTypewriter = false;
    [SerializeField, Range(0.01f, 0.1f)] private float charDelay = 0.02f;

    [Header("Eventos")]
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

    private int index = -1;
    private bool isRunning = false;
    private Coroutine typingRoutine;

    private void Awake()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(Next);
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    private void Start()
    {
        if (autoStart) StartDialogue();
    }

    public void StartDialogue()
    {
        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning("[DialogueManager] Nenhuma fala configurada.");
            return;
        }

        index = -1;
        isRunning = true;
        dialoguePanel?.SetActive(true);
        onDialogueStart?.Invoke();
        Next();
    }

    public void Next()
    {
        if (!isRunning) return;

        index++;
        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }

        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        // Configura o sprite correto para a fala atual
        SetCharacterSprite(index);

        if (useTypewriter)
            typingRoutine = StartCoroutine(TypeText(lines[index]));
        else
            dialogueText.text = lines[index];
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

    private void SetCharacterSprite(int lineIndex)
    {
        // Se existir um sprite para a linha atual, define o sprite na Image
        if (lineIndex < characterSprites.Length && characterSprites[lineIndex] != null)
        {
            characterImage.sprite = characterSprites[lineIndex];
            characterImage.gameObject.SetActive(true); // Mostra a imagem
        }
        else
        {
            characterImage.gameObject.SetActive(false); // Se não tiver sprite, esconde a imagem
        }
    }

    private void EndDialogue()
    {
        isRunning = false;
        dialoguePanel?.SetActive(false);
        onDialogueEnd?.Invoke();
    }
}
