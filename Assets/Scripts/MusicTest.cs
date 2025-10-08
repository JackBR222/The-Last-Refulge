using UnityEngine;

public class MusicTest : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Configura��o de M�sica")]
    [SerializeField] private AudioClip musicClip;  // �udio a ser tocado, configur�vel no Inspector

    void Start()
    {
        // Obt�m o AudioSource do objeto
        audioSource = GetComponent<AudioSource>();

        // Verifica se o AudioSource foi encontrado
        if (audioSource == null)
        {
            Debug.LogError("[MusicTest] AudioSource n�o encontrado no GameObject.");
            return;
        }

        // Verifica se o musicClip foi atribu�do
        if (musicClip == null)
        {
            Debug.LogError("[MusicTest] Nenhum AudioClip atribu�do.");
            return;
        }

        // Atribui o �udio ao AudioSource
        audioSource.clip = musicClip;

        // Toca a m�sica ao iniciar a cena
        PlayMusic();
    }

    // Fun��o para tocar m�sica
    public void PlayMusic()
    {
        if (audioSource != null && musicClip != null)
        {
            // Verifica se a m�sica n�o est� tocando antes de iniciar
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); // Toca a m�sica
                Debug.Log("[MusicTest] M�sica iniciada.");
            }
            else
            {
                Debug.Log("[MusicTest] M�sica j� est� tocando.");
            }
        }
        else
        {
            Debug.LogWarning("[MusicTest] AudioSource ou AudioClip n�o configurado corretamente.");
        }
    }

    // Fun��o para parar a m�sica
    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // Para a m�sica
            Debug.Log("[MusicTest] M�sica parada.");
        }
        else
        {
            Debug.Log("[MusicTest] M�sica n�o est� tocando.");
        }
    }
}
