using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOItem))]
public class EditorSOItemCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        
        SOItem so = (SOItem)target;
        GUILayoutOption[] options = new GUILayoutOption[2];
        options[0] =GUILayout.Height(100);
        options[1] =GUILayout.Width(100);
        so.Sprite =EditorGUILayout.ObjectField(so.Sprite, typeof(Sprite),  false, options) as Sprite;
        base.OnInspectorGUI();
    }
}

[CustomPreview(typeof(SOItem))]
public class MyPreview : ObjectPreview
{
    public override bool HasPreviewGUI() {
        return true;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background) {
        //GUI.Label(r, target.name + " is being previewed");
        GUI.DrawTexture( r, AssetPreview.GetAssetPreview(((SOItem)target).Sprite), ScaleMode.ScaleToFit);
    }
}