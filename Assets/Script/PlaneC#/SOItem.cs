using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "SO/SoItem")]
public class SOItem : ScriptableObject {
    public Sprite Sprite;
    public string Name;
    [TextArea]public string Description;
}


