using UnityEngine;
using UnityEngine.UI;

public class GarbageBar : MonoBehaviour
{
    public Image Fill;

    public float currentValue = 0f;
    public float maxValue = 100f;

    void Start()
    {
        UpdateBar();
    }

    void Update()
    {
        // test : augmente avec une touche
        if (Input.GetKey(KeyCode.H))
        {
            Add(20f * Time.deltaTime);
        }
    }

    public void Add(float amount)
    {
        currentValue += amount;
        currentValue = Mathf.Clamp(currentValue, 0f, maxValue);
        UpdateBar();
    }

    public void Set(float value)
    {
        currentValue = Mathf.Clamp(value, 0f, maxValue);
        UpdateBar();
    }

    void UpdateBar()
    {
        fillImage.fillAmount = currentValue / maxValue;
    }
}