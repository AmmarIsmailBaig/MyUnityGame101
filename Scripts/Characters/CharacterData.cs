using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "MurderMystery/Character")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    [TextArea(3, 10)] public string personalityPrompt;
    [HideInInspector] public bool isMurderer; // will be set at runtime
}
