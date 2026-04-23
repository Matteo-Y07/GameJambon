using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Hearts")]
    [SerializeField] private HeartUI heart3;
    [SerializeField] private HeartUI heart2;
    [SerializeField] private HeartUI heart1;

    [Header("HP")]
    [SerializeField] private int hp3 = 100;
    [SerializeField] private int hp2 = 100;
    [SerializeField] private int hp1 = 100;

    private bool isDead = false;

    public event Action OnDeath;

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        int remaining = damage;

        remaining = ApplyDamage(ref hp3, heart3, remaining);
        if (remaining > 0)
            remaining = ApplyDamage(ref hp2, heart2, remaining);
        if (remaining > 0)
            remaining = ApplyDamage(ref hp1, heart1, remaining);

        if (hp1 <= 0 && hp2 <= 0 && hp3 <= 0)
        {
            isDead = true;
            OnDeath?.Invoke();
        }
    }

    int ApplyDamage(ref int hp, HeartUI ui, int damage)
    {
        int newHP = hp - damage;

        if (newHP <= 0)
        {
            int overflow = -newHP;
            hp = 0;
            ui.SetHP(0);
            return overflow;
        }

        hp = newHP;
        ui.SetHP(hp);
        return 0;
    }

    public int GetTotalHP()
    {
        return hp1 + hp2 + hp3;
    }

    public void ResetHealth()
    {
        hp1 = 100;
        hp2 = 100;
        hp3 = 100;

        heart1.SetHP(hp1);
        heart2.SetHP(hp2);
        heart3.SetHP(hp3);
        isDead = false;
    }

    public bool IsDead()
    {
        return isDead;
    }
}