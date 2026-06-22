public static class GameState
{
    private static bool inPause;
    private static bool canTriggerDialogue;

    public static bool InPause
    {
        get => inPause;
        set => inPause = value;
    }

    public static bool CanTriggerDialogue
    {
        get => canTriggerDialogue;
        set
        {
            canTriggerDialogue = value;
            UnityEngine.Debug.Log($"[GameState] CanTriggerDialogue = {value}");
        }
    }

    public static void Reset()
    {
        inPause = false;
        canTriggerDialogue = false;

        UnityEngine.Debug.Log("[GameState] Reset()");
    }
}