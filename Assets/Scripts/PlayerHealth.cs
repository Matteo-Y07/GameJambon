using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HeartUI heart3;
    public HeartUI heart2;
    public HeartUI heart1;

    int hp3 = 100;
    int hp2 = 100;
    int hp1 = 100;

    public void TakeDamage(int damage)
    {
        int remaining = damage;

        remaining = ApplyDamage(ref hp3, heart3, remaining);
        if (remaining > 0)
            remaining = ApplyDamage(ref hp2, heart2, remaining);
        if (remaining > 0)
            remaining = ApplyDamage(ref hp1, heart1, remaining);
    }

    int ApplyDamage(ref int hp, HeartUI ui, int damage)
    {
        int newHP = hp - damage;

        if (newHP < 0)
        {
            int overflow = -newHP;
            hp = 0;
            ui.SetHP(0);
            return overflow;
        }
        else
        {
            hp = newHP;
            ui.SetHP(hp);
            return 0;
        }
    }
}