using UnityEngine;

public class PillarSwitch : MonoBehaviour
{
    [Header("Tilemaps")]
    [SerializeField] private GameObject tilemapDead;
    [SerializeField] private GameObject tilemapAlive;

    private bool playerInside = false;

    void Update()
    {
        if (!playerInside) return;

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            ChangeTilemap();
        }
    }

    void ChangeTilemap()
    {
        if (tilemapDead != null)
            tilemapDead.SetActive(false);

        if (tilemapAlive != null)
            tilemapAlive.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            playerInside = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            playerInside = false;
    }
}