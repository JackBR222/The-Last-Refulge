using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    [Header("Nome da Cena de Destino")]
    [SerializeField] private string targetSceneName;

    [Header("Delay opcional antes da transição")]
    [SerializeField] private float transitionDelay = 0f;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            if (transitionDelay > 0f)
                Invoke(nameof(LoadTargetScene), transitionDelay);
            else
                LoadTargetScene();
        }
    }

    private void LoadTargetScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}