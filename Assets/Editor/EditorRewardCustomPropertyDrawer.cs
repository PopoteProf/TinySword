using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Reward))]
public class EditorRewardCustomPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Rect ComporatRect = new Rect(position.x, position.y, 90, 20);
        EditorGUI.PropertyField(ComporatRect, property.FindPropertyRelative("Type"), GUIContent.none);
        if (property.FindPropertyRelative("Type").enumValueIndex == 0)
        {
            Rect resourcesTypeRect = new Rect(position.x + 95, position.y, 70, 20);
            Rect resourcesAmountType = new Rect(position.x + 170, position.y, position.width - 170, 20);
            EditorGUI.PropertyField(resourcesTypeRect, property.FindPropertyRelative("ResourceType"), GUIContent.none);
            EditorGUI.PropertyField(resourcesAmountType, property.FindPropertyRelative("ResourceAmount"), GUIContent.none);
        }

        if (property.FindPropertyRelative("Type").enumValueIndex == 1)
        {
            Rect itemRect = new Rect(position.x + 95, position.y, position.width - 95, 20);
            EditorGUI.PropertyField(itemRect, property.FindPropertyRelative("SoItem"), GUIContent.none);
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 20;
    }
}