using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOTriggerIdsAndQuest))]
public class EditorSoTriggerIdAndQuest : Editor {
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Open Window"))EditorWindowSoTrigerIdAndQuest.ShowWindow((SOTriggerIdsAndQuest)target);
        base.OnInspectorGUI();
    }
}