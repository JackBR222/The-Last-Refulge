using UnityEngine;
using UnityEngine.Events;

public class TriggerOnceArea : MonoBehaviour
{
    public UnityEvent onPlayerEnter;
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            onPlayerEnter.Invoke();
            hasTriggered = true;
        }
    }
}