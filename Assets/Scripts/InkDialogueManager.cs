using UnityEngine;
using UnityEngine.UI;                // Necess�rio para Button, Image
using TMPro;                         // Necess�rio para TextMeshProUGUI
using Ink.Runtime;                   // Necess�rio para Story e Choice
using System.Collections;            // Necess�rio para IEnumerator
using System.Collections.Generic;    // Necess�rio para listas e cole��es
using System.Text.RegularExpressions; // Necess�rio para Regex

public class InkDialogueManager : MonoBehaviour
{
    [Header("Refer�ncias de UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Transform choicesContainer;
    [SerializeField] private GameObject choiceButtonPrefab;

    [Header("Nome e Retrato")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private Image characterPortraitImage;
    [SerializeField] private Sprite defaultPortrait;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSONAsset;

    [Header("Configura��o")]
    [SerializeField] private float typingSpeed = 0.02f;
    [SerializeField] private bool useTypewriter = true;

    private Story currentStory;
    private Coroutine typingCoroutine;

    private string currentCharacterName = "Desconhecido";
    private Sprite currentCharacterPortrait;

    private void Awake()
    {
        if (nextButton != null)
            nextButton.onClick.AddListener(NextLine);
        else
            Debug.LogWarning("Bot�o 'Next' n�o atribu�do!");  // Caso o bot�o n�o tenha sido atribu�do.

        dialoguePanel?.SetActive(false);  // Desabilita o painel de di�logo na inicializa��o.
    }

    private void Start()
    {
        if (inkJSONAsset != null)
        {
            StartDialogue(inkJSONAsset);  // Inicia o di�logo com o arquivo .ink
        }
        else
        {
            Debug.LogError("Arquivo Ink JSON n�o atribu�do!");
        }
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        // Cria a hist�ria a partir do arquivo .ink
        currentStory = new Story(inkJSON.text);
        dialoguePanel.SetActive(true);  // Ativa o painel de di�logo.

        NextLine();  // Inicia o di�logo com a primeira linha.
    }

    public void NextLine()
    {
        // Verifica se o di�logo ainda pode continuar
        if (currentStory == null || !currentStory.canContinue)
        {
            EndDialogue();
            return;
        }

        nextButton.gameObject.SetActive(false);  // Desativa o bot�o "Next" enquanto a linha est� sendo digitada.

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);  // Para qualquer coroutine de digita��o em andamento.

        string line = currentStory.Continue().Trim();  // Obt�m a pr�xima linha da hist�ria.

        ParseCharacterInfoFromTags(line);  // Analisa os metadados de personagem (nome, retrato, etc.) na linha.

        if (useTypewriter)
            typingCoroutine = StartCoroutine(TypeLine(line));  // Exibe a linha de forma "escrita".
        else
            dialogueText.text = line;  // Exibe a linha toda de uma vez.
    }

    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";  // Limpa o texto de di�logo atual.

        // Exibe cada letra uma por uma, com o intervalo configurado.
        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        ShowChoices();  // Ap�s terminar de escrever, exibe as escolhas, se houver.
    }

    private void ShowChoices()
    {
        // Remove as escolhas antigas, se houver.
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }

        // Exibe as novas escolhas, se existirem.
        if (currentStory.currentChoices.Count > 0)
        {
            nextButton.gameObject.SetActive(false);  // Desativa o bot�o "Next" durante a exibi��o das escolhas.

            foreach (Choice choice in currentStory.currentChoices)
            {
                // Cria um bot�o de escolha para cada uma das op��es.
                GameObject choiceButton = Instantiate(choiceButtonPrefab, choicesContainer);
                TextMeshProUGUI buttonText = choiceButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = choice.text;

                Button button = choiceButton.GetComponent<Button>();
                button.onClick.AddListener(() => OnChoiceSelected(choice, choiceButton));  // Configura a a��o do bot�o.
            }
        }
        else
        {
            nextButton.gameObject.SetActive(true);  // Ativa o bot�o "Next" se n�o houver escolhas.
        }
    }

    private void OnChoiceSelected(Choice choice, GameObject selectedButton)
    {
        // Remove as escolhas antigas quando uma for selecionada.
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }

        // Avan�a na hist�ria para a escolha selecionada.
        currentStory.ChooseChoiceIndex(choice.index);

        NextLine();  // Exibe a pr�xima linha de di�logo.

        Destroy(selectedButton);  // Destroi o bot�o de escolha ap�s a sele��o.
    }

    private void ParseCharacterInfoFromTags(string line)
    {
        // Usando Regex para extrair informa��es de nome e retrato do personagem.
        var nameMatch = Regex.Match(line, @"#speaker\s*:\s*(\w+)");
        var portraitMatch = Regex.Match(line, @"#portrait\s*:\s*(\w+)");

        if (nameMatch.Success)
        {
            currentCharacterName = nameMatch.Groups[1].Value;
            characterNameText.text = currentCharacterName;  // Atualiza o nome do personagem.
        }

        if (portraitMatch.Success)
        {
            string characterPortrait = portraitMatch.Groups[1].Value;
            currentCharacterPortrait = Resources.Load<Sprite>(characterPortrait);
            characterPortraitImage.sprite = currentCharacterPortrait ? currentCharacterPortrait : defaultPortrait;  // Define o retrato do personagem.
        }
    }

    private void EndDialogue()
    {
        dialoguePanel?.SetActive(false);  // Desativa o painel de di�logo.
        currentStory = null;  // Limpa a hist�ria atual.
    }
}
