using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;


public class DialogueUIManager : MonoBehaviour
{
    public TMP_InputField questionInput;
    public TMP_Dropdown characterDropdown;
    public Button askButton;

    private List<string> characterNames = new List<string>();

    void Start()
    {
        characterNames = new List<string> { "Theo", "Lena", "Marcus", "Ava" };

        characterDropdown.ClearOptions();
        characterDropdown.AddOptions(characterNames);

        askButton.onClick.AddListener(OnAskClicked);
    }

    void OnAskClicked()
    {
        string selectedCharacter = characterNames[characterDropdown.value];
        string playerInput = questionInput.text;

        if (!string.IsNullOrEmpty(playerInput))
        {
            GroqDialogueManager.Instance.AskCharacter(selectedCharacter, playerInput);
        }
    }
}
