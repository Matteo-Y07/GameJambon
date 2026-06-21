using UnityEngine;

public class RepeatBg : MonoBehaviour
{
    [SerializeField] private int nbBackground;
    private Transform cam;
    private float width;

    void Start()
    {
        cam = Camera.main.transform;
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        width = sr.bounds.size.x;
        if (nbBackground == 0)
        {
            if (transform.parent != null)
            {
                nbBackground = transform.parent.childCount;
            }
            else 
            {
                nbBackground = 1;
            }
        }
    }

    void Update()
    {
        float camX = cam.position.x;
        float posX = transform.position.x;

        if (posX + width < camX)
        {
            transform.position += new Vector3(width * nbBackground, 0f, 0f);
        }
        else if (posX - width > camX)
        {
            transform.position -= new Vector3(width * nbBackground, 0f, 0f);
        }
    }
}