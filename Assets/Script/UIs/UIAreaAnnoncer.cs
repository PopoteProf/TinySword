using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIAreaAnnoncer : MonoBehaviour {
    [SerializeField] private TMP_Text _txtAnnoncer;
    [SerializeField] private CanvasGroup _canvasGroup;

    [Header("Tweening")] 
    [SerializeField] private float _annoncerAnimationTime = 3f;
    [SerializeField] private AnimationCurve _alphaAniamtionCurve = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private AnimationCurve _scaleXAniamtionCurve = AnimationCurve.EaseInOut(0,0,1,1);
    
    void Start() {
        StaticData.OnEnterArea += PlayAnnoncer;
    }

    private void OnDestroy()
    {
        StaticData.OnEnterArea -= PlayAnnoncer;
    }

    private void PlayAnnoncer(string name)
    {
        _canvasGroup.DOPause();
        _canvasGroup.transform.DOPause();
        
        _txtAnnoncer.text = name;
        _canvasGroup.alpha = 0;
        _canvasGroup.transform.localScale = new Vector3(0, 1, 1);

        _canvasGroup.DOFade(1, _annoncerAnimationTime).SetEase(_alphaAniamtionCurve); 
        _canvasGroup.transform.DOScaleX(1, _annoncerAnimationTime).SetEase(_scaleXAniamtionCurve);
    }
}
