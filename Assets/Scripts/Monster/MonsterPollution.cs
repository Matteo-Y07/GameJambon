using UnityEngine;

public class MonsterPollution : Monster
{
    [Header("Pollution")]
    [SerializeField] private float pollutionPerSecond = 5f;

    private GarbageBar garbageBar;
    private Renderer rend;

    protected override void Awake()
    {
        base.Awake();
        rend = GetComponent<Renderer>();
    }

    protected override void Start()
    {
        base.Start();

        if (player != null)
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null) garbageBar = pm.GetGarbageBar();
        }

        SetDetectionRange(10f);
        SetAttackRange(0f);
        SetMoveSpeed(1f);
        SetDamage(0);
        SetHealth(3);
    }

    protected override void Update()
    {
        base.Update();
        Pollute();
    }

    void Pollute()
    {
        if (garbageBar == null) return;
        if (!IsVisible()) return;

        garbageBar.Add(pollutionPerSecond * Time.deltaTime);
    }

    bool IsVisible()
    {
        return rend != null && rend.isVisible;
    }
}