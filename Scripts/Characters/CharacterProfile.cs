using UnityEngine;

[System.Serializable]
public class CharacterProfile
{
    public string characterName;
    [TextArea(3, 10)] public string personalityPrompt;
    [HideInInspector] public bool isMurderer;
}
