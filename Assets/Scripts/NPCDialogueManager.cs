using UnityEngine;
using Ink.Runtime;

public class NPCDialogueManager : MonoBehaviour
{
    [SerializeField] private NPCFollowPlayer npcFollowPlayer;  // Referência ao script NPCFollowPlayer

    private Story story;

    // Função para inicializar a história e começar o diálogo
    public void StartDialogue(TextAsset inkJson)
    {
        story = new Story(inkJson.text);
        NextLine();
    }

    public void NextLine()
    {
        if (story.canContinue)
        {
            string line = story.Continue().Trim();
            ExecuteCommand(line);
        }
        else
        {
            EndDialogue();
        }
    }

    private void ExecuteCommand(string line)
    {
        // Aqui verificamos se a linha contém comandos do Ink
        if (line.Contains("#npc.StartFollowing()"))
        {
            npcFollowPlayer.StartFollowing();  // Executa o comando de seguimento
        }
    }

    private void EndDialogue()
    {
        // Finaliza o diálogo
    }
}
