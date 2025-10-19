using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DepthSorter : MonoBehaviour
{
    public int sortingOrderBase = 5000;
    public float offset = 0f;
    public bool runOnce = false;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (runOnce && sr.sortingOrder != 0) return;

        sr.sortingOrder = (int)(sortingOrderBase - (transform.position.y + offset) * 100);

        if (runOnce)
            enabled = false;
    }
}