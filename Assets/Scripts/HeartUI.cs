using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image img;

    [Header("Sprites")]
    [SerializeField] private Sprite full;
    [SerializeField] private Sprite threeQuarters;
    [SerializeField] private Sprite half;
    [SerializeField] private Sprite quarter;
    [SerializeField] private Sprite empty;

    [Header("State")]
    [SerializeField] private int hp = 100;

    public void SetHP(int value)
    {
        hp = Mathf.Clamp(value, 0, 100);
        UpdateSprite();
    }

    public int GetHP()
    {
        return hp;
    }

    private void UpdateSprite()
    {
        if (img == null) return;

        if (hp <= 0)
            img.sprite = empty;
        else if (hp <= 25)
            img.sprite = quarter;
        else if (hp <= 50)
            img.sprite = half;
        else if (hp <= 75)
            img.sprite = threeQuarters;
        else
            img.sprite = full;
    }
}