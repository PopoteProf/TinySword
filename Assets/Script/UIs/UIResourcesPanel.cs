using TMPro;
using UnityEngine;

public class UIResourcesPanel : MonoBehaviour {
    [SerializeField] private TMP_Text _txtMeat;
    [SerializeField] private TMP_Text _txtWood;
    [SerializeField] private TMP_Text _txtGold;

    public void Awake() {
        StaticData.OnRessourcesChanged+= OnResourcesChanged;
    }

    public void OnDestroy() {
        StaticData.OnRessourcesChanged -= OnResourcesChanged;
    }

    private void OnResourcesChanged() {
        _txtMeat.text = StaticData.Food.ToString();
        _txtWood.text = StaticData.Wood.ToString();
        _txtGold.text = StaticData.Gold.ToString();
    }
}