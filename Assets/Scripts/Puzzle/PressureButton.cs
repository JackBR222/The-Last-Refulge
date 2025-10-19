using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PressureButton : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite idleSprite;
    public Sprite pressedSprite;

    [Header("Visual Sprite Renderer (filho)")]
    public SpriteRenderer visualSpriteRenderer;

    [Header("Sons")]
    public AudioClip pressSound;
    private AudioSource audioSource;

    private HashSet<GameObject> _objectsOnButton = new HashSet<GameObject>();

    public bool IsPressed => _objectsOnButton.Count > 0;

    private void Awake()
    {
        if (visualSpriteRenderer == null)
        {
            visualSpriteRenderer = GetComponent<SpriteRenderer>();
            if (visualSpriteRenderer == null)
            {
                Debug.LogWarning("[PressureButton] Nenhum SpriteRenderer encontrado no objeto ou no filho visual!");
            }
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (visualSpriteRenderer != null)
        {
            visualSpriteRenderer.sprite = idleSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsValidObject(other))
        {
            _objectsOnButton.Add(other.gameObject);
            UpdateSprite();
            PlayPressSound();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_objectsOnButton.Contains(other.gameObject))
        {
            _objectsOnButton.Remove(other.gameObject);
            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        if (visualSpriteRenderer == null) return;

        if (IsPressed)
            visualSpriteRenderer.sprite = pressedSprite;
        else
            visualSpriteRenderer.sprite = idleSprite;
    }

    private bool IsValidObject(Collider2D other)
    {
        return other.CompareTag("Player") || other.CompareTag("NPC") || other.CompareTag("Pushable");
    }

    private void PlayPressSound()
    {
        if (pressSound != null)
        {
            audioSource.PlayOneShot(pressSound);
        }
    }
}