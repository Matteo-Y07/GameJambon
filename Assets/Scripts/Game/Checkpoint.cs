using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private GameObject inactiveSprite;
    [SerializeField] private GameObject activeSprite;

    void Start()
    {
        inactiveSprite.SetActive(true);
        activeSprite.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        inactiveSprite.SetActive(false);
        activeSprite.SetActive(true);
        
        GameManager.instance.SetCheckpoint(transform.position);
    }
}