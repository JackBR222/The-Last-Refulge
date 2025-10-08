using UnityEngine;
using UnityEngine.UI;                // Necessário para Button, Image
using TMPro;                         // Necessário para TextMeshProUGUI
using Ink.Runtime;                   // Necessário para Story e Choice
using System.Collections;            // Necessário para IEnumerator
using System.Collections.Generic;    // Necessário para listas e coleções
using System.Text.RegularExpressions; // Necessário para Regex

public class InkDialogueManager : MonoBehaviour
{
    [Header("Referências de UI")]
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

    [Header("Configuração")]
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
            Debug.LogWarning("Botão 'Next' não atribuído!");  // Caso o botão não tenha sido atribuído.

        dialoguePanel?.SetActive(false);  // Desabilita o painel de diálogo na inicialização.
    }

    private void Start()
    {
        if (inkJSONAsset != null)
        {
            StartDialogue(inkJSONAsset);  // Inicia o diálogo com o arquivo .ink
        }
        else
        {
            Debug.LogError("Arquivo Ink JSON não atribuído!");
        }
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        // Cria a história a partir do arquivo .ink
        currentStory = new Story(inkJSON.text);
        dialoguePanel.SetActive(true);  // Ativa o painel de diálogo.

        NextLine();  // Inicia o diálogo com a primeira linha.
    }

    public void NextLine()
    {
        // Verifica se o diálogo ainda pode continuar
        if (currentStory == null || !currentStory.canContinue)
        {
            EndDialogue();
            return;
        }

        nextButton.gameObject.SetActive(false);  // Desativa o botão "Next" enquanto a linha está sendo digitada.

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);  // Para qualquer coroutine de digitação em andamento.

        string line = currentStory.Continue().Trim();  // Obtém a próxima linha da história.

        ParseCharacterInfoFromTags(line);  // Analisa os metadados de personagem (nome, retrato, etc.) na linha.

        if (useTypewriter)
            typingCoroutine = StartCoroutine(TypeLine(line));  // Exibe a linha de forma "escrita".
        else
            dialogueText.text = line;  // Exibe a linha toda de uma vez.
    }

    private IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";  // Limpa o texto de diálogo atual.

        // Exibe cada letra uma por uma, com o intervalo configurado.
        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        ShowChoices();  // Após terminar de escrever, exibe as escolhas, se houver.
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
            nextButton.gameObject.SetActive(false);  // Desativa o botão "Next" durante a exibição das escolhas.

            foreach (Choice choice in currentStory.currentChoices)
            {
                // Cria um botão de escolha para cada uma das opções.
                GameObject choiceButton = Instantiate(choiceButtonPrefab, choicesContainer);
                TextMeshProUGUI buttonText = choiceButton.GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = choice.text;

                Button button = choiceButton.GetComponent<Button>();
                button.onClick.AddListener(() => OnChoiceSelected(choice, choiceButton));  // Configura a ação do botão.
            }
        }
        else
        {
            nextButton.gameObject.SetActive(true);  // Ativa o botão "Next" se não houver escolhas.
        }
    }

    private void OnChoiceSelected(Choice choice, GameObject selectedButton)
    {
        // Remove as escolhas antigas quando uma for selecionada.
        foreach (Transform child in choicesContainer)
        {
            Destroy(child.gameObject);
        }

        // Avança na história para a escolha selecionada.
        currentStory.ChooseChoiceIndex(choice.index);

        NextLine();  // Exibe a próxima linha de diálogo.

        Destroy(selectedButton);  // Destroi o botão de escolha após a seleção.
    }

    private void ParseCharacterInfoFromTags(string line)
    {
        // Usando Regex para extrair informações de nome e retrato do personagem.
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
        dialoguePanel?.SetActive(false);  // Desativa o painel de diálogo.
        currentStory = null;  // Limpa a história atual.
    }
}
