using UnityEngine;

public class MenuRunner : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
        GetComponent<Animator>().SetBool("isRunning", true);
    }
}