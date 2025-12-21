using UnityEditor;
using UnityEngine;

public class EditorWindowSoTrigerIdAndQuest : EditorWindow {
    private SOTriggerIdsAndQuest _data;
    private Vector2 _scrollPos;
    private Vector2 _scrollPos2;
    private SerializedObject so;
    private int _selectedQuestId;
    
    public static void ShowWindow(SOTriggerIdsAndQuest data) {
        EditorWindowSoTrigerIdAndQuest window = GetWindow<EditorWindowSoTrigerIdAndQuest>();
        window.titleContent = new GUIContent(data.name);
        window.Setup(data);
    }

    public void Setup(SOTriggerIdsAndQuest data) {
        _data = data;
        so = new SerializedObject(data);
    }
    
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, position.width / 2, position.height));
        _scrollPos = GUILayout.BeginScrollView(_scrollPos, false, true);
        GUILayout.BeginVertical();
        GUILayout.BeginArea(new Rect(0,0,30,_data.Triggers.Length*30+26));
        GUILayout.Space(26);
        GUIStyle style = new GUIStyle();
        style.fixedWidth = 30;
        style.fixedHeight = 25;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.MiddleCenter;
        
        for (int i = 0; i < _data.Triggers.Length; i++) {
            GUILayout.Space(2);
            EditorGUILayout.LabelField(i.ToString(),style);
        }
        GUILayout.EndArea();
        GUILayout.EndVertical();
        //GUILayout.BeginArea(new Rect(30,0,position.width / 2-30,10));
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        EditorGUILayout.PropertyField(so.FindProperty("Triggers"),true);
        so.ApplyModifiedProperties();
        GUILayout.EndHorizontal();
        //GUILayout.EndArea();
        GUILayout.EndScrollView();
        GUILayout.EndArea();
        
        // Partie Quest
        GUILayout.BeginArea(new Rect(position.width / 2, 0, position.width / 2, 160+GetAdditionalQuestTriggerHeight()));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Previus")) ChangeQuestID(-1);
        style.fixedWidth = 130;
        GUILayout.Label("Quest At Index:"+_selectedQuestId, style);
        if (GUILayout.Button("Next")) ChangeQuestID(1);
        GUILayout.EndHorizontal();
        DisplayQuestData();
        
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(position.width / 2, 160+GetAdditionalQuestTriggerHeight(), position.width / 2, position.height-40));
        _scrollPos2 = GUILayout.BeginScrollView(_scrollPos2, false, true);
        EditorGUILayout.PropertyField(so.FindProperty("Quests"),true);
        so.ApplyModifiedProperties();
        GUILayout.EndScrollView();
        GUILayout.EndArea();

        //_data.Quests = _triggerIdsProperties.;
    }

    private void ChangeQuestID(int id) {
        
        _selectedQuestId += id;
        if (_data.Quests.Length == 0) {
            _selectedQuestId = 0;
            return;
        }
        _selectedQuestId = Mathf.Clamp(_selectedQuestId, 0, _data.Quests.Length - 1);
    }

    private void DisplayQuestData() {
        if (_data.Quests.Length == 0 || _selectedQuestId<0|| _selectedQuestId>=_data.Quests.Length||_data.Quests[_selectedQuestId]==null) {
            EditorGUILayout.LabelField("Not Quest Data Fount");
            GUILayout.Space(80);
        }
        else {
            SerializedObject so = new SerializedObject(_data.Quests[_selectedQuestId]);
            EditorGUILayout.PropertyField(so.FindProperty("QuestTitle"),true);
            EditorGUILayout.PropertyField(so.FindProperty("QuestDescription"),true);
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(so.FindProperty("Triggers"),true);
            EditorGUILayout.PropertyField(so.FindProperty("CompletID"),true);
            so.ApplyModifiedProperties();
            GUILayout.EndHorizontal();
        }
    }

    private int GetAdditionalQuestTriggerHeight() {
        if (_data.Quests.Length == 0 || _selectedQuestId < 0 || _selectedQuestId >= _data.Quests.Length ||
            _data.Quests[_selectedQuestId] == null)
        {
            return 0;
        }
        else
        {
            return _data.Quests[_selectedQuestId].Triggers.Length * 22;
        }
    }
}