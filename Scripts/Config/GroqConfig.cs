using UnityEngine;

[CreateAssetMenu(fileName = "GroqConfig", menuName = "Config/GroqConfig")]
public class GroqConfig : ScriptableObject
{
    [SerializeField]
    private string apiKey = "";
    
    public string ApiKey => apiKey;
} 