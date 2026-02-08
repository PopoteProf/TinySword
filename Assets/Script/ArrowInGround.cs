using System;
using UnityEngine;

public class ArrowInGround : MonoBehaviour
{
    
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected AnimationCurve _animationCurve = new AnimationCurve();
    [SerializeField] protected float _animationZAmplitude = 10;
    [SerializeField] protected float _animationTime = 0.5f;
    [SerializeField] protected float _postAnimationLifeTime = 5;
    [SerializeField] protected AnimationCurve _posAnimationAlpha = new AnimationCurve();
    protected PopoteTimer _animationTimer;
    protected PopoteTimer _postAnimationTimer;
    protected float _zRotPos; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _animationTimer = new PopoteTimer(_animationTime);
        _postAnimationTimer = new PopoteTimer(_postAnimationLifeTime);
        
        _animationTimer.OnTimerEnd += OnAnimationTimerEnd;
        _postAnimationTimer.OnTimerEnd+= OnPostAnimTimerEnd;
        
        _animationTimer.Play();
        _zRotPos = transform.eulerAngles.z;
    }

    protected virtual void OnAnimationTimerEnd(object sender, EventArgs e) {
        _postAnimationTimer.Play();
    }
    protected virtual void OnPostAnimTimerEnd(object sender, EventArgs e) {
       Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        _animationTimer.UpdateTimer();
        _postAnimationTimer.UpdateTimer();
        
        if (_animationTimer.IsPlaying)ManagerAnimation();
        if( _postAnimationTimer.IsPlaying)ManagerPostAnimation();
    }

    protected virtual void ManagerAnimation() {
        float z = _zRotPos+_animationZAmplitude*_animationCurve.Evaluate(_animationTimer.T);
        transform.eulerAngles = new Vector3(0f, 0f,z );
    }

    protected virtual void ManagerPostAnimation()
    {
        _spriteRenderer.color = new Color(1,1,1,_posAnimationAlpha.Evaluate(_postAnimationTimer.T));
    }
}