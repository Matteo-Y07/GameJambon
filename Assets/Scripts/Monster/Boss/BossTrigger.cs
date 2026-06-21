using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    private BossTuto boss;

    private void Awake()
    {
        // AUTOMATISATION : Le trigger cherche le boss posé sur le même objet, 
        // ou dans ses enfants / parents proches.
        boss = GetComponentInChildren<BossTuto>();
        if (boss == null) boss = GetComponentInParent<BossTuto>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (boss != null)
            {
                boss.ActivateBoss();
            }
        }
    }
}