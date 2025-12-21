using System;
using UnityEditor;
using UnityEngine;

public class EditorWindowTriggerInGame : EditorWindow
{
    private Vector2  scrollPos;
    [MenuItem("PopoteTools/WindowTriggerInGame")]
    public static void ShowWindow() {
        EditorWindowTriggerInGame windon = GetWindow<EditorWindowTriggerInGame>();
    }
    
    private void OnGUI() {
        if (EditorApplication.isPlaying) {
            if (TriggerAndQuest.SoTriggerIdsAndQuest == null)
            {
                EditorGUILayout.LabelField("No Trigger and Quest data fount");
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            for (int i = 0; i < TriggerAndQuest.SoTriggerIdsAndQuest.Triggers.Length; i++) {
                DisplayTrigger(i);
            }
            EditorGUILayout.EndScrollView();
            
        }
        else
        {
            EditorGUILayout.LabelField("Not playing");
        }
    }

    private void DisplayTrigger(int id) {
        EditorGUILayout.BeginHorizontal();
        if( GUILayout.Button("Force True", GUILayout.Width(75))) {TriggerAndQuest.SetTriggerID(id, true);}
        if( GUILayout.Button("Force false", GUILayout.Width(75))) {TriggerAndQuest.SetTriggerID(id, false);}
        GUIStyle style = new GUIStyle();
        style.fixedWidth = 30;
        style.fixedHeight = 25;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.LabelField( id.ToString(),style, GUILayout.Width(30));
        EditorGUILayout.ToggleLeft( GUIContent.none, TriggerAndQuest.IsTriggered(id), GUILayout.Width(30));
        EditorGUILayout.LabelField(TriggerAndQuest.SoTriggerIdsAndQuest.Triggers[id].TriggerDescription);
        EditorGUILayout.EndHorizontal();
    }

    public void Update() {
        Repaint();
    }
}