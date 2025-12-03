using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PhonemeKey
{
    [Tooltip("Seconds from the start of the audio.")]
    public float time;
    public Phoneme phoneme;
}

[Serializable]
public class PhonemeKeyData
{
    public List<PhonemeKey> keys;
}

[CreateAssetMenu(fileName = "NewLipSyncClip", menuName = "LipSync/Clip")]
public class LipSyncClip : ScriptableObject
{
    public AudioClip audio;
    public TextAsset jsonFile;
    public PhonemeKey[] keys;

    [ContextMenu("Load Keys from JSON")]
    public void LoadKeysFromJsonFile()
    {
        if (jsonFile == null)
        {
            Debug.LogError("JSON file is not assigned in the ScriptableObject");
            return;
        }

        LoadKeysFromJson(jsonFile);
    }

    /// <summary>
    /// Load phoneme keys from a JSON file
    /// </summary>
    /// <param name="jsonFilePath">Path to the JSON file (relative to Resources folder or absolute path)</param>
    public void LoadKeysFromJson(string jsonFilePath)
    {
        try
        {
            string jsonContent;
            
            // Try loading from Resources folder first
            TextAsset jsonAsset = Resources.Load<TextAsset>(jsonFilePath);
            if (jsonAsset != null)
            {
                jsonContent = jsonAsset.text;
            }
            else
            {
                // Try loading from absolute path
                if (System.IO.File.Exists(jsonFilePath))
                {
                    jsonContent = System.IO.File.ReadAllText(jsonFilePath);
                }
                else
                {
                    Debug.LogError($"JSON file not found at path: {jsonFilePath}");
                    return;
                }
            }

            PhonemeKeyData data = JsonUtility.FromJson<PhonemeKeyData>(jsonContent);
            if (data != null && data.keys != null)
            {
                keys = data.keys.ToArray();
                Debug.Log($"Successfully loaded {keys.Length} phoneme keys from JSON");
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
            else
            {
                Debug.LogError("Failed to parse JSON or no keys found");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading keys from JSON: {e.Message}");
        }
    }

    /// <summary>
    /// Load phoneme keys from a TextAsset
    /// </summary>
    /// <param name="jsonAsset">TextAsset containing the JSON data</param>
    public void LoadKeysFromJson(TextAsset jsonAsset)
    {
        if (jsonAsset == null)
        {
            Debug.LogError("JSON TextAsset is null");
            return;
        }

        try
        {
            PhonemeKeyData data = JsonUtility.FromJson<PhonemeKeyData>(jsonAsset.text);
            if (data != null && data.keys != null)
            {
                keys = data.keys.ToArray();
                Debug.Log($"Successfully loaded {keys.Length} phoneme keys from JSON");
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
            else
            {
                Debug.LogError("Failed to parse JSON or no keys found");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading keys from JSON: {e.Message}");
        }
    }
}
