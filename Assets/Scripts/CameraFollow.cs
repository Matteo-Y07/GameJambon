using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2.0f;
    public Transform Target;
    void Start()
    {
        if (Target == null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        Vector3 targetPosition = new Vector3(Target.position.x, Target.position.y, -10.0f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, FollowSpeed * Time.deltaTime);
    }
    void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(Target.position.x, Target.position.y, -10.0f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, FollowSpeed * Time.deltaTime);
    }
}
