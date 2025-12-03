// File: Assets/Scripts/ScriptableObjectScripts/Editor/LipSyncClipEditor.cs
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LipSyncClip))]
public class LipSyncClipEditor : Editor
{
    private TextAsset jsonFile; // Editor-only, not serialized in the asset

    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        LipSyncClip lipSyncClip = (LipSyncClip)target;

        // Add some space
        EditorGUILayout.Space(10);
        
        // Editor-only JSON file field
        EditorGUILayout.LabelField("Load Keys from JSON", EditorStyles.boldLabel);
        jsonFile = (TextAsset)EditorGUILayout.ObjectField("JSON File", jsonFile, typeof(TextAsset), false);

        // Add a button to load keys from JSON
        if (GUILayout.Button("Load Keys from JSON", GUILayout.Height(30)))
        {
            if (jsonFile == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a JSON file first!", "OK");
            }
            else
            {
                LoadKeysFromJson(lipSyncClip, jsonFile);
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