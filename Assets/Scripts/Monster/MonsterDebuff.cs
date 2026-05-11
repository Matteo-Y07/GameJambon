using UnityEngine;
using System.Collections;

public class MonsterDebuff : Monster
{
    [Header("Debuff")]
    [SerializeField] private float slowMultiplier = 0.5f;
    [SerializeField] private float debuffDuration = 3f;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (!collision.gameObject.CompareTag("Player")) return;
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

        if (player != null)
        {
            StartCoroutine(ApplyDebuff(player));
        }
    }

    IEnumerator ApplyDebuff(PlayerMovement player)
    {
        float originalSpeed = player.GetMoveSpeed();
        player.SetMoveSpeed(originalSpeed * slowMultiplier);
        yield return new WaitForSeconds(debuffDuration);
        player.SetMoveSpeed(originalSpeed);
    }
}