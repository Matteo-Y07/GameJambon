using UnityEngine;
using System.Collections;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private float respawnDelay = 0.2f;
    [SerializeField] private PlayerHealth playerHealth;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    public void Die()
    {
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        Vector3 respawnPos = GameManager.instance.GetCheckpoint(startPosition);
        transform.position = respawnPos;
        playerHealth.ResetHealth();
        GetComponent<PlayerStateChecker>().ResetState();
    }

    public Vector3 GetStartPosition()
    {
        return startPosition;
    }
}