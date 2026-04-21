using UnityEngine;

public class HealthTester : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth player;

    [Header("Hearts")]
    [SerializeField] private HeartUI heart3;
    [SerializeField] private HeartUI heart2;
    [SerializeField] private HeartUI heart1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            player.TakeDamage(80);
            Debug.Log("Test damage: 5");
            Debug.Log(heart3.GetHP());
            Debug.Log(heart2.GetHP());
            Debug.Log(heart1.GetHP());
        }
    }
}