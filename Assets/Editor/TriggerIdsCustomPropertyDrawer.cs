using UnityEditor;
using UnityEngine;

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