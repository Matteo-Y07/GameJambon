using UnityEngine;
using UnityEngine.UI;

public class GarbageBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fill;

    [Header("Values")]
    [SerializeField] private float currentValue = 0f;
    [SerializeField] private float maxValue = 100f;

    void Start()
    {
        UpdateBar();
    }

    void Update()
    {
        // test
        if (Input.GetKey(KeyCode.H))
        {
            Add(20f * Time.deltaTime);
        }
    }

    public void Add(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0f, maxValue);
        UpdateBar();
    }

    public void Set(float value)
    {
        currentValue = Mathf.Clamp(value, 0f, maxValue);
        UpdateBar();
    }

    public float GetValue()
    {
        return currentValue;
    }

    public float GetPercent()
    {
        return currentValue / maxValue;
    }

    void UpdateBar()
    {
        if (fill == null) return;

        fill.fillAmount = currentValue / maxValue;
    }
}