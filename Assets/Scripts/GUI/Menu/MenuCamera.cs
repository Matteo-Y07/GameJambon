using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public Transform target;
    public float smooth = 5f;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 desired = new Vector3(target.position.x + 6f, target.position.y + 3f, -10f);
        transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);
    }
}