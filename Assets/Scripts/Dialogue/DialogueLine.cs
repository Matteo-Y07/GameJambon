using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string characterName;

    [TextArea(3, 5)]
    public string text;

    public Sprite portrait;
}