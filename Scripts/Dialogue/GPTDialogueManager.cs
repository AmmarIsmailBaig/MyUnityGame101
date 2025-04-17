using UnityEngine;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class GroqDialogueManager : MonoBehaviour
{
    public static GroqDialogueManager Instance;
    private readonly HttpClient httpClient = new HttpClient();
    private const string GROQ_API_ENDPOINT = "https://api.groq.com/openai/v1/chat/completions";
    
    [SerializeField]
    private GroqConfig config;

    [System.Serializable]
    private class ChatMessage
    {
        public string role;
        public string content;
    }

    [System.Serializable]
    private class ChatRequest
    {
        public string model = "llama-3.3-70b-versatile";
        public List<ChatMessage> messages;
        public float temperature = 0.7f;
        public int max_completion_tokens = 1024;
    }

    [System.Serializable]
    private class ChatResponse
    {
        public Choice[] choices;
    }

    [System.Serializable]
    private class Choice
    {
        public ChatMessage message;
    }

    private class CharacterMemory
    {
        public List<ChatMessage> messages = new List<ChatMessage>();
    }

    private Dictionary<string, CharacterMemory> memoryMap = new Dictionary<string, CharacterMemory>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (config == null)
        {
            Debug.LogError("GroqConfig not assigned! Please assign it in the Unity Inspector.");
            return;
        }

        if (string.IsNullOrEmpty(config.ApiKey))
        {
            Debug.LogError("GROQ API Key not set in GroqConfig!");
            return;
        }

        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {config.ApiKey}");
    }

    public void InitializeCharacter(string characterName, string systemPrompt)
    {
        CharacterMemory memory = new CharacterMemory();
        memory.messages.Add(new ChatMessage()
        {
            role = "system",
            content = systemPrompt
        });

        memoryMap[characterName] = memory;
    }

    public async void AskCharacter(string characterName, string playerInput)
    {
        if (!memoryMap.ContainsKey(characterName)) return;

        var memory = memoryMap[characterName];
        memory.messages.Add(new ChatMessage
        {
            role = "user",
            content = playerInput
        });

        var request = new ChatRequest
        {
            messages = memory.messages
        };

        try
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            
            var response = await httpClient.PostAsync(GROQ_API_ENDPOINT, content);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            
            if (response.IsSuccessStatusCode)
            {
                var chatResponse = JsonConvert.DeserializeObject<ChatResponse>(jsonResponse);
                if (chatResponse.choices != null && chatResponse.choices.Length > 0)
                {
                    var reply = chatResponse.choices[0].message;
                    memory.messages.Add(reply);
                    Debug.Log($"{characterName} says: {reply.content}");
                }
            }
            else
            {
                Debug.LogError($"Error from GROQ API: {jsonResponse}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error communicating with GROQ API: {e.Message}");
        }
    }

    void OnDestroy()
    {
        httpClient.Dispose();
    }
}
