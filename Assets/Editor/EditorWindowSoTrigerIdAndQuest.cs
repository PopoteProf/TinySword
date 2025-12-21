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
        GUILayout.BeginArea(new Rect(position.width / 2, 0, position.width / 2, 130));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Previus")) ChangeQuestID(-1);
        style.fixedWidth = 130;
        GUILayout.Label("Quest At Index:"+_selectedQuestId, style);
        if (GUILayout.Button("Next")) ChangeQuestID(1);
        GUILayout.EndHorizontal();
        DisplayQuestData();
        
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(position.width / 2, 130, position.width / 2, position.height-40));
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
            EditorGUILayout.PropertyField(so.FindProperty("TriggerID"),true);
            EditorGUILayout.PropertyField(so.FindProperty("CompletID"),true);
            so.ApplyModifiedProperties();
            GUILayout.EndHorizontal();
        }
    }
    
}


[CustomEditor(typeof(SOTriggerIdsAndQuest))]
public class EditorSoTriggerIdAndQuest : Editor {
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Open Window"))EditorWindowSoTrigerIdAndQuest.ShowWindow((SOTriggerIdsAndQuest)target);
        base.OnInspectorGUI();
    }
}

[CustomPropertyDrawer(typeof(SOTriggerIdsAndQuest.TriggerID))]
public class TriggerIdsCustomPropertyDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty(position, label, property);
        Rect IdRect = new Rect(position.x, position.y, 30, 20);
        Rect DesctiptionRect = new Rect(position.x+30, position.y, position.width-50,20);
        EditorGUI.PropertyField(IdRect, property.FindPropertyRelative("IsTriggered"),  GUIContent.none);
        EditorGUI.PropertyField(DesctiptionRect, property.FindPropertyRelative("TriggerDescription"),  GUIContent.none);
        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 20;
    }
}

[CustomPropertyDrawer(typeof(ConditionTrigger))]
public class EditorConditionTriggerCustomPropertyDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        
        Rect ComporatRect = new Rect(position.x, position.y, 90, 20);
        EditorGUI.PropertyField(ComporatRect, property.FindPropertyRelative("Type"),GUIContent.none);
        if (property.FindPropertyRelative("Type").enumValueIndex == 0)
        {
            Rect TriggerRect = new Rect(position.x+95, position.y, position.width-130,20);
            Rect IsTriggerRect = new Rect(position.x+position.width-30, position.y, 30,20);
            EditorGUI.PropertyField(TriggerRect, property.FindPropertyRelative("TriggerID"),GUIContent.none);
            EditorGUI.PropertyField(IsTriggerRect, property.FindPropertyRelative("IsTriggered"),GUIContent.none);
        }

        if (property.FindPropertyRelative("Type").enumValueIndex == 1)
        {
            Rect RessourceRect = new Rect(position.x+90, position.y, (position.width-90)/3,20);
            Rect ComporatorRect = new Rect(position.x+90+(position.width-90)/3, position.y, (position.width-90)/3,20);
            Rect RessourveValueRect = new Rect(position.x+90+((position.width-90)/3)*2, position.y, (position.width-90)/3,20);
            EditorGUI.PropertyField(RessourceRect, property.FindPropertyRelative("Ressource"),GUIContent.none);
            EditorGUI.PropertyField(ComporatorRect, property.FindPropertyRelative("Comparator"),GUIContent.none);
            EditorGUI.PropertyField(RessourveValueRect, property.FindPropertyRelative("RessourceValue"),GUIContent.none);
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return 20;
    }
}
[CustomPropertyDrawer(typeof(SimpleConditionTrigger))]
public class EditorSimpleConditionTriggerCustomPropertyDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        Rect TriggerRect = new Rect(position.x+30, position.y, position.width-30,20);
        Rect IsTriggerRect = new Rect(position.x, position.y, 30,20);
        EditorGUI.PropertyField(TriggerRect, property.FindPropertyRelative("TriggerID"),GUIContent.none);
        EditorGUI.PropertyField(IsTriggerRect, property.FindPropertyRelative("IsTriggered"),GUIContent.none);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return 20;
    }
}
