using UnityEngine;

public class HealthTester : MonoBehaviour
{
    public PlayerHealth player;
    public HeartUI heart3;
    public HeartUI heart2;
    public HeartUI heart1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            player.TakeDamage(5);
            Debug.Log("Test damage: 5");
            Debug.Log(heart3.GetHP());
            Debug.Log(heart2.GetHP());
            Debug.Log(heart1.GetHP());
        }
    }
}