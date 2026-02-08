using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryPanel : MonoBehaviour
{
    [SerializeField] private Button _bpInventory;
    [SerializeField] private Transform _transformButtonHolder;
    [SerializeField] private Button _pfxInventoryButton;
    [SerializeField] private Image _imgSelectedItem;
    [SerializeField] private TMP_Text _txtTitleItem;
    [SerializeField] private TMP_Text _txtDescriptionItem;
    [Header("Tweening")]
    [SerializeField] private float _openningAnimationTime = 0.5f;
    [SerializeField] private AnimationCurve _openingPivotanimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [Space (5)]
    [SerializeField] private float _closeningAnimationTime = 0.25f;
    [SerializeField] private AnimationCurve _closingPivotanimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [Space (5), Header("Audio")]
    [SerializeField] private AudioElement _audioOnOpen;
    [SerializeField] private AudioElement _audioOnClose;
    
    private bool _panelClose=false ;
    private SOItem _selectedItem;

    private void Start() {
        _bpInventory.onClick.AddListener(UIOpenClosePanel);
        StaticData.OnInventoryChange+= OnInventoryChange;
    }

    private void OnDestroy()
    {
        StaticData.OnInventoryChange-= OnInventoryChange;
    }

    private void OnInventoryChange() {
        if (!StaticData._inventory.Contains(_selectedItem)) {
            SetUpSelectionItem(null);
        }
        SetInventoryButtons();
    }

    private void SetUpSelectionItem(SOItem item)
    {
        if (item == null)
        {
            _selectedItem = null;
            _imgSelectedItem.gameObject.SetActive(false);
            _txtTitleItem.text = "";
            _txtDescriptionItem.text = "";
            return;
        }    
        _imgSelectedItem.gameObject.SetActive(true);
        _imgSelectedItem.sprite = item.Sprite;
        _txtTitleItem.text = item.Name;
        _txtDescriptionItem.text = item.Description;
        _selectedItem = item;
    }

    private void SetInventoryButtons()
    {
        ClearInventoryButtons();
        foreach (SOItem item in StaticData._inventory) {
            Button bp =Instantiate(_pfxInventoryButton, _transformButtonHolder);
            bp.transform.GetComponentInChildren<TMP_Text>().text = item.Name;
            bp.onClick.AddListener(delegate {
                SetUpSelectionItem(item);
            });
        }
    }

    private void ClearInventoryButtons() {
        for (int i = _transformButtonHolder.childCount - 1; i >= 0; i--)
        {
            Destroy(_transformButtonHolder.GetChild(i).gameObject);
        }
    }
    private void UIOpenClosePanel() {
        if (!_panelClose) OpenPanel();
        else ClosePanel();
    }
    
    private void ClosePanel()
    {
        transform.GetComponent<RectTransform>().DOPivotX(1,_closeningAnimationTime).SetEase(_closingPivotanimationCurve);
        _audioOnClose.PlayAsSFX();
        _panelClose = false;
    }

    private void OpenPanel()
    {
        SetUpSelectionItem(null);
        SetInventoryButtons();
        transform.GetComponent<RectTransform>().DOPivotX(0,_openningAnimationTime).SetEase(_openingPivotanimationCurve);
        _audioOnOpen.PlayAsSFX();
        _panelClose = true;
    }
}