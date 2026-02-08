using DG.Tweening;
using UnityEngine;

public class PickableSpriteFeedBack : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [Space(5)]
    [SerializeField] private float _idleEndPosition=0.1f;
    [SerializeField] private float _idleaniamtionTime=0.3f;
    [SerializeField] private AnimationCurve _idleAnimationCurve = AnimationCurve.EaseInOut(0,0,1,1);
    [Space(10)]
    [SerializeField] private float _pickUpSpeed=0.5f;
    [SerializeField] private float _pichUpXtarget=0;
    [SerializeField] private AnimationCurve _pichUpXAnimationCurve = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private float _pichUpYtarget=0;
    [SerializeField] private AnimationCurve _pichUpYAnimationCurve = AnimationCurve.EaseInOut(0,0,1,1);
    [Space(10)]
    [SerializeField] private float _fadeOutSpeed=5f;
    [SerializeField] private AnimationCurve _fadeOutAnimationCurve= AnimationCurve.EaseInOut(0,0,1,1);
    void Start() {
        transform.DOLocalMoveY(_idleEndPosition, _idleaniamtionTime).SetEase(_idleAnimationCurve).SetLoops(-1, LoopType.Yoyo);
    }

    public void SetUpSprite(Sprite sprite) {
        _spriteRenderer .sprite = sprite;
    }

    [ContextMenu("DoPickUp")]
    public void DoPickup() {
        transform.DOScaleX(_pichUpXtarget, _pickUpSpeed).SetEase(_pichUpXAnimationCurve);
        transform.DOScaleY(_pichUpYtarget, _pickUpSpeed).SetEase(_pichUpYAnimationCurve);
    }

    [ContextMenu("FadeOut")]
    public void DoFadeOut() {
        _spriteRenderer.DOFade(0,_fadeOutSpeed).SetEase(_fadeOutAnimationCurve);
    }
}
