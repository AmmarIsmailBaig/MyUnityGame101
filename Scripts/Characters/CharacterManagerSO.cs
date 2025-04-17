using UnityEngine;
using System.Collections.Generic;

public class CharacterManagerSO : MonoBehaviour
{
    [Header("Character ScriptableObjects")]
    public List<CharacterData> characterProfiles;

    void Start()
    {
        AssignRandomMurderer();

        foreach (var character in characterProfiles)
        {
            string fullPrompt = GeneratePrompt(character);
            GroqDialogueManager.Instance.InitializeCharacter(character.characterName, fullPrompt);
        }
    }

    void AssignRandomMurderer()
    {
        int index = Random.Range(0, characterProfiles.Count);
        characterProfiles[index].isMurderer = true;
        Debug.Log($"{characterProfiles[index].characterName} is the murderer (keep it hush hush)");
    }

    string GeneratePrompt(CharacterData character)
    {
        if (character.isMurderer)
        {
            return character.personalityPrompt + " You are the murderer. Never admit it. Stay cool, and make your answers seem innocent.";
        }
        else
        {
            return character.personalityPrompt + " You are innocent and want to help the investigation. Be truthful and cooperative.";
        }
    }
}
