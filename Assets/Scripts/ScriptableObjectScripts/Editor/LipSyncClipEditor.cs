// File: Assets/Scripts/ScriptableObjectScripts/Editor/LipSyncClipEditor.cs
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LipSyncClip))]
public class LipSyncClipEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        LipSyncClip lipSyncClip = (LipSyncClip)target;

        // Add some space
        EditorGUILayout.Space(10);

        // Add a button to load keys from JSON
        if (GUILayout.Button("Load Keys from JSON", GUILayout.Height(30)))
        {
            if (lipSyncClip.jsonFile == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a JSON file first!", "OK");
            }
            else
            {
                LoadKeysFromJsonFile(lipSyncClip);
                EditorUtility.SetDirty(lipSyncClip);
                AssetDatabase.SaveAssets();
            }
        }

        // Display current keys count
        if (lipSyncClip.keys != null && lipSyncClip.keys.Length > 0)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.HelpBox($"Current Keys Count: {lipSyncClip.keys.Length}", MessageType.Info);
        }
    }

    private void LoadKeysFromJsonFile(LipSyncClip lipSyncClip)
    {
        if (lipSyncClip.jsonFile == null)
        {
            Debug.LogError("JSON file is not assigned in the ScriptableObject");
            return;
        }

        LoadKeysFromJson(lipSyncClip, lipSyncClip.jsonFile);
    }

    /// <summary>
    /// Load phoneme keys from a JSON file
    /// </summary>
    /// <param name="lipSyncClip">The LipSyncClip to load keys into</param>
    /// <param name="jsonFilePath">Path to the JSON file (relative to Resources folder or absolute path)</param>
    private void LoadKeysFromJson(LipSyncClip lipSyncClip, string jsonFilePath)
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
                lipSyncClip.keys = data.keys.ToArray();
                Debug.Log($"Successfully loaded {lipSyncClip.keys.Length} phoneme keys from JSON");
                EditorUtility.SetDirty(lipSyncClip);
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
    /// <param name="lipSyncClip">The LipSyncClip to load keys into</param>
    /// <param name="jsonAsset">TextAsset containing the JSON data</param>
    private void LoadKeysFromJson(LipSyncClip lipSyncClip, TextAsset jsonAsset)
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
                lipSyncClip.keys = data.keys.ToArray();
                Debug.Log($"Successfully loaded {lipSyncClip.keys.Length} phoneme keys from JSON");
                EditorUtility.SetDirty(lipSyncClip);
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