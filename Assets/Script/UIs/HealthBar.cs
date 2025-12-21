using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _imgFill;
    [SerializeField] private Image _imgFollowBar;

    [SerializeField] private float _barChangeTime = 0.5f;
    [SerializeField] private AnimationCurve _barChangeCurve = AnimationCurve.EaseInOut(0,0,1,1);
    [SerializeField] private AnimationCurve _FollowBarChangeCurve = AnimationCurve.EaseInOut(0,0,1,1);
    
    private float _targetHealth =0;
    private float _startingHealth;
    private float _currentHealth =1;
    private PopoteTimer _changeTimer;

    private void Awake()
    {
        StaticData.PlayerHealthChanged += PlayerHealthChanged;
        _changeTimer = new PopoteTimer(_barChangeTime);
    }

    private void PlayerHealthChanged(float newHealth)
    {
        
        _targetHealth = newHealth;
        _startingHealth = _currentHealth;
        _changeTimer.Play();
    }

    // Update is called once per frame
    void Update() {
        if (_changeTimer.IsPlaying) {
            _changeTimer.UpdateTimer();
            _currentHealth  =Mathf.Lerp(_startingHealth, _targetHealth,_barChangeCurve.Evaluate(_changeTimer.T));
            _imgFill.fillAmount = _currentHealth;
            _imgFollowBar.fillAmount= Mathf.Lerp(_startingHealth, _targetHealth,_FollowBarChangeCurve.Evaluate(_changeTimer.T));
        }
    }
    
    
}
