using UnityEngine;
using UnityEngine.UI;
using System;

public class GarbageBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fill;

    [Header("Values")]
    [SerializeField] private float currentValue = 0f;
    [SerializeField] private float maxValue = 100f;

    private bool maxReached;

    void Awake()
    {
        UpdateBar();
    }

    void OnEnable()
    {
        UpdateBar();
    }

    public void Add(float amount)
    {
        Set(currentValue + amount);
    }

    public void Set(float value)
    {
        currentValue = Mathf.Clamp(value, 0f, maxValue);
        UpdateBar();

        if (!maxReached && currentValue >= maxValue)
        {
            maxReached = true;
        }
    }

    public void ResetBar()
    {
        currentValue = 0f;
        maxReached = false;
        UpdateBar();
    }

    public float GetPercent()
    {
        return maxValue <= 0f ? 0f : currentValue / maxValue;
    }

    void UpdateBar()
    {
        if (fill == null) return;
        fill.fillAmount = GetPercent();
    }

    public bool IsMaxReached() => maxReached;
}