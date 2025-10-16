using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePortraitManager : MonoBehaviour
{
    [Header("Refer�ncias de UI")]
    [SerializeField] private Image portraitImage;
    [SerializeField] private TextMeshProUGUI displayNameText;

    [Header("Refer�ncias de Sprites")]
    [SerializeField] private Sprite sarueNeutral;
    [SerializeField] private Sprite maracajaNeutral;

    [Header("Refer�ncias de Sons")]
    [SerializeField] private AudioSource sarueVoice;
    [SerializeField] private AudioSource maracajaVoice;
    [SerializeField] private AudioSource vowelSoundA;

    public AudioSource HandleTags(string speakerName, string portraitName, string audioName = null)
    {
        if (!string.IsNullOrEmpty(speakerName))
            UpdateSpeakerName(speakerName);

        if (!string.IsNullOrEmpty(portraitName))
            SetPortrait(portraitName);

        if (!string.IsNullOrEmpty(audioName))
            return GetAudioSource(audioName);

        return null;
    }

    public void UpdateSpeakerName(string speakerName)
    {
        displayNameText.text = speakerName;
        Debug.Log("Nome do personagem atualizado para: " + speakerName);
    }

    public void SetPortrait(string portraitName)
    {
        switch (portraitName)
        {
            case "SarueNeutral":
                portraitImage.sprite = sarueNeutral;
                break;

            case "MaracajaNeutral":
                portraitImage.sprite = maracajaNeutral;
                break;

            default:
                Debug.LogWarning("Retrato n�o encontrado: " + portraitName);
                break;
        }
    }

    private AudioSource GetAudioSource(string audioName)
    {
        switch (audioName)
        {
            case "SarueVoice":
                return sarueVoice;

            case "MaracajaVoice":
                return maracajaVoice;

            case "VowelA":
                return vowelSoundA;

            default:
                Debug.LogWarning("�udio n�o encontrado: " + audioName);
                return null;
        }
    }
}
