using System;
using UnityEngine;

public class ArrowInGround : MonoBehaviour
{
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private AnimationCurve _animationCurve = new AnimationCurve();
    [SerializeField] private float _animationZAmplitude = 10;
    [SerializeField] private float _animationTime = 0.5f;
    [SerializeField] private float _postAnimationLifeTime = 5;
    [SerializeField] private AnimationCurve _posAnimationAlpha = new AnimationCurve();
    private PopoteTimer _animationTimer;
    private PopoteTimer _postAnimationTimer;
    private float _zRotPos; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        _animationTimer = new PopoteTimer(_animationTime);
        _postAnimationTimer = new PopoteTimer(_postAnimationLifeTime);
        
        _animationTimer.OnTimerEnd += OnAnimationTimerEnd;
        _postAnimationTimer.OnTimerEnd+= OnPostAnimTimerEnd;
        
        _animationTimer.Play();
        _zRotPos = transform.eulerAngles.z;
    }

    private void OnAnimationTimerEnd(object sender, EventArgs e) {
        _postAnimationTimer.Play();
    }
    private void OnPostAnimTimerEnd(object sender, EventArgs e) {
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

    private void ManagerAnimation() {
        float z = _zRotPos+_animationZAmplitude*_animationCurve.Evaluate(_animationTimer.T);
        transform.eulerAngles = new Vector3(0f, 0f,z );
    }

    private void ManagerPostAnimation()
    {
        _spriteRenderer.color = new Color(1,1,1,_posAnimationAlpha.Evaluate(_postAnimationTimer.T));
    }
}
