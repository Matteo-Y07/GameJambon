using UnityEngine;
using UnityEngine.UI;

public class BossUIManager : MonoBehaviour
{
    public static BossUIManager Instance;

    public Image healthBar;

    private void Awake()
    {
        Instance = this;
    }
}