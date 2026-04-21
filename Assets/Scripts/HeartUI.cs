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
        if (hp <= 0)
            img.sprite = empty;
        else if (hp <= 25)
            img.sprite = quarter;
        else if (hp <= 50 )
            img.sprite = half;
        else if (hp <= 75 )
            img.sprite = threeQuarters;
        else
            img.sprite = full;
    }
}