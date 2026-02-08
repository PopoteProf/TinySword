using System;
using DG.Tweening;
using UnityEngine;

public class SlidingPanelMainMenu : MonoBehaviour
{
    public event Action OnPanelClose; 
    [Header("Tweening")]
    [SerializeField] private float _openningAnimationTime = 0.3f;
    [SerializeField] private AnimationCurve _openingPivotanimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [Space (5)]
    [SerializeField] private float _closeningAnimationTime = 0.25f;
    [SerializeField] private AnimationCurve _closingPivotanimationCurve  = AnimationCurve.EaseInOut(0,0,1,1);
    [Space (5), Header("Audio")]
    [SerializeField] private AudioElement _audioOnOpen;
    [SerializeField] private AudioElement _audioOnClose;
    [SerializeField] protected bool _panelClose = false;
    
    protected void UIOpenClosePanel() {
        if (_panelClose) OpenPanel();
        else ClosePanel();
    }
    
    protected virtual void ClosePanel() {
        transform.GetComponent<RectTransform>().DOPivotY(0,_closeningAnimationTime).SetEase(_closingPivotanimationCurve).OnComplete(ClosePanelFinish);
        _audioOnClose.PlayAsSFX();
        _panelClose = true;
        OnPanelClose?.Invoke();
    }

    public virtual void OpenPanel() {
        transform.GetComponent<RectTransform>().DOPivotY(1,_openningAnimationTime).SetEase(_openingPivotanimationCurve).OnComplete(OpenPanelFinish);
        _audioOnOpen.PlayAsSFX();
        _panelClose = false;
    }

    protected virtual void ClosePanelFinish() {}
    protected virtual void OpenPanelFinish() {}
}