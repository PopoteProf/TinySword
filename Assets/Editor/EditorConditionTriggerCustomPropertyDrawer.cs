using UnityEditor;
using UnityEngine;

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