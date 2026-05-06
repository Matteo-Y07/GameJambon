using UnityEngine;

public class PillarSwitch : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private GameObject normalGO;
    [SerializeField] private GameObject highlightGO;

    [Header("Tilemaps")]
    [SerializeField] private GameObject tilemapDead;
    [SerializeField] private GameObject tilemapAlive;

    private bool playerInside = false;
    private float switchCooldown = 2f;
    [SerializeField] private float cooldownTimer = 0f;
    void Start()
    {
        normalGO.SetActive(true);
        highlightGO.SetActive(false);

        tilemapDead.SetActive(true);
        tilemapAlive.SetActive(false);
    }

    void Update()
    {
        if (!playerInside) return;

        // Appuie sur S
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer < switchCooldown) return;
            else
            {
                SwitchTilemaps();
                cooldownTimer = 0f;
            }
        }
        else cooldownTimer = 0f;
    }

    void SwitchTilemaps()
    {
        tilemapDead.SetActive(false);
        tilemapAlive.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerInside = true;

            normalGO.SetActive(false);
            highlightGO.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerInside = false;

            normalGO.SetActive(true);
            highlightGO.SetActive(false);
        }
    }
}