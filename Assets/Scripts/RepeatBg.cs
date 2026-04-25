using UnityEngine;

public class RepeatBg : MonoBehaviour
{
    private Transform cam;
    private float width;

    void Start()
    {
        cam = Camera.main.transform;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        width = sr.bounds.size.x;
    }

    void Update()
    {
        float camX = cam.position.x;
        float posX = transform.position.x;

        if (posX + width < camX)
        {
            transform.position += new Vector3(width * 2f, 0f, 0f);
        }
        else if (posX - width > camX)
        {
            transform.position -= new Vector3(width * 2f, 0f, 0f);
        }
    }
}