using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        PlayerStateCheck state = collision.GetComponent<PlayerStateCheck>();

        if (state != null)
            state.Kill(DeathType.DeadZone);
    }
}