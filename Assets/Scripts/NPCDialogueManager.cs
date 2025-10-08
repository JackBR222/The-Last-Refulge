using UnityEngine;
using Ink.Runtime;

public class NPCDialogueManager : MonoBehaviour
{
    [SerializeField] private NPCFollowPlayer npcFollowPlayer;  // Refer�ncia ao script NPCFollowPlayer

    private Story story;

    // Fun��o para inicializar a hist�ria e come�ar o di�logo
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
        // Aqui verificamos se a linha cont�m comandos do Ink
        if (line.Contains("#npc.StartFollowing()"))
        {
            npcFollowPlayer.StartFollowing();  // Executa o comando de seguimento
        }
    }

    private void EndDialogue()
    {
        // Finaliza o di�logo
    }
}
