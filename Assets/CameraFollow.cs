using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public float FollowSpeed = 2.0f;
    public Transform Target;

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(Target.position.x, Target.position.y, -10.0f);
        transform.position = Vector3.Slerp(transform.position, targetPosition, FollowSpeed * Time.deltaTime);
    }
}
