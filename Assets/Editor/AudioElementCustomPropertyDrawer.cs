using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AudioElement))]
public class AudioElementCustomPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        float audioClipHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("AudioClips"));
        EditorGUI.BeginProperty(position, label, property);
        Rect IdRect = new Rect(position.x, position.y, position.width,audioClipHeight);
        Rect VolumeLabelRect = new Rect(position.x, position.y+audioClipHeight, 50,20);
        Rect VolumeRect = new Rect(position.x+50, position.y+audioClipHeight, position.width-50,20);
        Rect UsRandomPitchLabelRect = new Rect(position.x+30, position.y+audioClipHeight+20, position.width-30,20);
        Rect UsRandomPitchRect = new Rect(position.x, position.y+audioClipHeight+20, 30,20);
        
        EditorGUI.PropertyField(IdRect, property.FindPropertyRelative("AudioClips"), new GUIContent(property.displayName));
        EditorGUI.LabelField(VolumeLabelRect, "Volume");
        EditorGUI.Slider(VolumeRect, property.FindPropertyRelative("Volume"),  0, 1, GUIContent.none);
        EditorGUI.PropertyField(UsRandomPitchRect, property.FindPropertyRelative("UsRandomPitch"),  GUIContent.none);
        EditorGUI.LabelField(UsRandomPitchLabelRect, "Random Pitch");
        if (property.FindPropertyRelative("UsRandomPitch").boolValue) {
            Rect MinRect =  new Rect(position.x, position.y+audioClipHeight+40, 40,20);
            Rect MaxRect =  new Rect(position.x+position.width-40, position.y+audioClipHeight+40, 40,20);
            Rect MinMaxRect =  new Rect(position.x+50, position.y+audioClipHeight+40, position.width-100,20);
            EditorGUI.PropertyField(MinRect, property.FindPropertyRelative("MinPitch"),  GUIContent.none);
            EditorGUI.PropertyField(MaxRect, property.FindPropertyRelative("MaxPitch"),  GUIContent.none);
            float min = property.FindPropertyRelative("MinPitch").floatValue;
            float max = property.FindPropertyRelative("MaxPitch").floatValue;
            EditorGUI.MinMaxSlider(MinMaxRect,ref min,ref max,-3,3);
            property.FindPropertyRelative("MinPitch").floatValue = min;
            property.FindPropertyRelative("MaxPitch").floatValue = max;
        }
        EditorGUI.EndProperty();
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        if (property.FindPropertyRelative("UsRandomPitch").boolValue)
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("AudioClips")) + 60;
        return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("AudioClips")) + 40;
    }
}