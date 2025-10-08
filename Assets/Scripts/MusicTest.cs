using UnityEngine;

public class MusicTest : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Configuração de Música")]
    [SerializeField] private AudioClip musicClip;  // Áudio a ser tocado, configurável no Inspector

    void Start()
    {
        // Obtém o AudioSource do objeto
        audioSource = GetComponent<AudioSource>();

        // Verifica se o AudioSource foi encontrado
        if (audioSource == null)
        {
            Debug.LogError("[MusicTest] AudioSource não encontrado no GameObject.");
            return;
        }

        // Verifica se o musicClip foi atribuído
        if (musicClip == null)
        {
            Debug.LogError("[MusicTest] Nenhum AudioClip atribuído.");
            return;
        }

        // Atribui o áudio ao AudioSource
        audioSource.clip = musicClip;

        // Toca a música ao iniciar a cena
        PlayMusic();
    }

    // Função para tocar música
    public void PlayMusic()
    {
        if (audioSource != null && musicClip != null)
        {
            // Verifica se a música não está tocando antes de iniciar
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); // Toca a música
                Debug.Log("[MusicTest] Música iniciada.");
            }
            else
            {
                Debug.Log("[MusicTest] Música já está tocando.");
            }
        }
        else
        {
            Debug.LogWarning("[MusicTest] AudioSource ou AudioClip não configurado corretamente.");
        }
    }

    // Função para parar a música
    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // Para a música
            Debug.Log("[MusicTest] Música parada.");
        }
        else
        {
            Debug.Log("[MusicTest] Música não está tocando.");
        }
    }
}
