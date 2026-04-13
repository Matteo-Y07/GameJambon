using UnityEngine;
using UnityEngine.Tilemaps;

public class PillarSwitch : MonoBehaviour
{
    public GameObject tilemapDead;
    public GameObject tilemapAlive;

    bool playerInside = false;

    void Update()
    {
        if (playerInside && Input.GetAxisRaw("Vertical") < 0) // touche S
        {
            ChangeTilemap();
        }
    }

    void ChangeTilemap()
    {
        tilemapDead.SetActive(false);
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