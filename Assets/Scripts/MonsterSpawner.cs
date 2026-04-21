using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private GameObject monsterPrefab; //le monstre qu'on veut générer 
    [SerializeField] private Transform player; //le joueur
    [SerializeField] private float spawnInterval = 3f; //toutes les combien de secondes un monstre apparait
    [SerializeField] private float spawnDistance = 8f; //distance autour du joueur où les monstres spawnent

    void Start()
    {

        InvokeRepeating(nameof(SpawnMonster), 1f, spawnInterval); //appelle une fonction au bout de 1sec puis la rapelle toutes les spawnInterval sec 
    }
    void SpawnMonster()
    {
        if (player == null) return; //si il n'y a pas de joueur on arrete pour éviter les crash

        Vector2 randomDir = Random.insideUnitCircle.normalized; // créer un point (de coordonnées entre -1 et 1) random dans un cercle 
        Vector2 spawnPos = (Vector2)player.position + randomDir * spawnDistance; //on prend la pos du joueur auquel on ajoute la direction du spawn du mob multiplié par la distance pour avoir le point exacte du spawn du mob

        Instantiate(monsterPrefab, spawnPos, Quaternion.identity); //on créer un monstre de type monstrePrefab à la pos spawnPos
        //Quaternion.identity sert apparement à ce qu'il n'y ait pas de rotation
    }
}