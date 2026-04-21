using UnityEngine;

public class PlayerStateChecker : MonoBehaviour
{
    public PlayerHealth playerHealth;

    public int pollution = 0;

    void Update()
    {
        CheckPlayerState();
    }

    void CheckPlayerState()
    {
        int hp = playerHealth.GetTotalHP();

        if (hp <= 0 || pollution >= 100)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player is dead");

        //désactiver le joueur
        //jouer animation de mort
        //affichage menu restart ou quit
        gameObject.SetActive(false);
    }
}