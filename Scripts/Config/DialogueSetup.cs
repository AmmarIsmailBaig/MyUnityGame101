using UnityEngine;

[DefaultExecutionOrder(-1)] // Ensure this runs before other scripts
public class DialogueSetup : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GroqConfig groqConfig;
    
    [Header("Required Components")]
    [SerializeField] private GroqDialogueManager dialogueManager;

    void Awake()
    {
        if (groqConfig == null)
        {
            Debug.LogError("GroqConfig asset is missing! Please create one via Create > Config > GroqConfig");
            return;
        }

        if (dialogueManager == null)
        {
            Debug.LogError("GroqDialogueManager reference is missing! Please assign it in the inspector.");
            return;
        }

        // Ensure the dialogue manager has its config
        var managerConfig = dialogueManager.GetComponent<GroqDialogueManager>();
        if (managerConfig != null)
        {
            // Use reflection to set the private config field
            var configField = typeof(GroqDialogueManager).GetField("config", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (configField != null)
            {
                configField.SetValue(managerConfig, groqConfig);
            }
        }
    }

#if UNITY_EDITOR
    // Add menu item to create the setup
    [UnityEditor.MenuItem("GameObject/Dialogue System/Create Dialogue Setup")]
    public static void CreateDialogueSetup()
    {
        // Create the main setup object
        var setupObj = new GameObject("Dialogue System");
        var setup = setupObj.AddComponent<DialogueSetup>();

        // Create the dialogue manager object
        var managerObj = new GameObject("Dialogue Manager");
        managerObj.transform.SetParent(setupObj.transform);
        var manager = managerObj.AddComponent<GroqDialogueManager>();

        // Try to find or create config
        var config = UnityEditor.AssetDatabase.FindAssets("t:GroqConfig");
        GroqConfig groqConfig = null;
        
        if (config.Length == 0)
        {
            // Create new config
            groqConfig = ScriptableObject.CreateInstance<GroqConfig>();
            string path = "Assets/Resources/GroqConfig.asset";
            System.IO.Directory.CreateDirectory("Assets/Resources");
            UnityEditor.AssetDatabase.CreateAsset(groqConfig, path);
            UnityEditor.AssetDatabase.SaveAssets();
        }
        else
        {
            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(config[0]);
            groqConfig = UnityEditor.AssetDatabase.LoadAssetAtPath<GroqConfig>(path);
        }

        // Assign references
        setup.groqConfig = groqConfig;
        setup.dialogueManager = manager;

        // Select the created object
        UnityEditor.Selection.activeGameObject = setupObj;
    }
#endif
} 