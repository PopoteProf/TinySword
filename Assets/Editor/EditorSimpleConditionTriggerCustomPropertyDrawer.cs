using UnityEditor;
using UnityEngine;

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