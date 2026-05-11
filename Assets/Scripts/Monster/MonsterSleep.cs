using UnityEngine;
using System.Collections;

public class MonsterSleep : Monster
{
    [Header("Sleep")]
    [SerializeField] private float sleepDuration = 2f;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (!collision.gameObject.CompareTag("Player")) return;
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();

        if (player != null)
        {
            StartCoroutine(SleepPlayer(player));
        }
        GameObject.Destroy(gameObject);
    }

    IEnumerator SleepPlayer(PlayerMovement player)
    {
        player.enabled = false;
        yield return new WaitForSeconds(sleepDuration);
        player.enabled = true;
    }
}