using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private Sprite activeSprite;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        Activate();
    }

    void Activate()
    {
        if (sr != null && activeSprite != null)
            sr.sprite = activeSprite;

        // Sauvegarde la position
        GameManager.instance.SetCheckpoint(transform.position);
    }
}