using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public Image img;

    public Sprite full;
    public Sprite threeQuarters;
    public Sprite half;
    public Sprite quarter;
    public Sprite empty;

    public int hp = 100;

    public void SetHP(int value)
    {
        hp = Mathf.Clamp(value, 0, 100);
        UpdateSprite();
    }

    public int GetHP(){
        return hp;
    }

    void UpdateSprite()
    {
        float p = hp / 100f;

        if (p <= 0)
            img.sprite = empty;
        else if (p <= 0.25f)
            img.sprite = quarter;
        else if (p <= 0.5f)
            img.sprite = half;
        else if (p <= 0.75f)
            img.sprite = threeQuarters;
        else
            img.sprite = full;
    }
}