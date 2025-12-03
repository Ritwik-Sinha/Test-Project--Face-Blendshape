// File: Assets/Scripts/ScriptableObjectScripts/Editor/LipSyncClipEditor.cs
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
                lipSyncClip.LoadKeysFromJsonFile();
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
}