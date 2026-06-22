public static class GameState
{
    private static bool inDialogue;
    private static bool inPause;
    private static bool canTriggerDialogue;

    public static bool InDialogue
    {
        get => inDialogue;
        set => inDialogue = value;
    }

    public static bool InPause
    {
        get => inPause;
        set => inPause = value;
    }

    public static bool CanTriggerDialogue
    {
        get => canTriggerDialogue;
        set => canTriggerDialogue = value;
    }

    public static void Reset()
    {
        inDialogue = false;
        inPause = false;
        canTriggerDialogue = false;
    }
}