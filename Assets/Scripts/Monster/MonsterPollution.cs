using UnityEngine;

public class MonsterPollution : Monster
{
    [Header("Pollution")]
    [SerializeField] private GarbageBar garbageBar;
    [SerializeField] private float pollutionPerSecond = 5f;

    private Renderer rend;

    protected override void Awake()
    {
        base.Awake();
        rend = GetComponent<Renderer>();
    }

    protected override void Start()
    {
        base.Start();
        garbageBar = FindObjectOfType<GarbageBar>();
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