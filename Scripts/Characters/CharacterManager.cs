using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public List<CharacterProfile> characters;

    void Start()
    {
        AssignRandomMurderer();
        foreach (var character in characters)
        {
            string prompt = GeneratePrompt(character);
            GroqDialogueManager.Instance.InitializeCharacter(character.characterName, prompt);
        }
    }

    void AssignRandomMurderer()
    {
        int index = Random.Range(0, characters.Count);
        characters[index].isMurderer = true;
        Debug.Log($"{characters[index].characterName} is the murderer (shh!)");
    }

    string GeneratePrompt(CharacterProfile character)
    {
        if (character.isMurderer)
        {
            return character.personalityPrompt + " You are the murderer. Do not admit to it. Answer convincingly to appear innocent.";
        }
        else
        {
            return character.personalityPrompt + " You are innocent and want to help solve the murder. Be honest and helpful.";
        }
    }
}
