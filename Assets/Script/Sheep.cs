using System;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = System.Numerics.Vector3;

public class Sheep : NavGridAgent
{
    [Header("Health")]
    [SerializeField] private int _health =4;
    [SerializeField] private GameObject _prfDeathParticule;
    [Header("WonderingParameters")]
    [SerializeField] private float _minWaitTime = 1;
    [SerializeField] private float _maxWaitTime = 5;
    [SerializeField] private int _wonderingDistance = 3;
    [Header("DamagedParameters")]
    [SerializeField] private float _damageTime = 1;
    [SerializeField] private AnimationCurve _damageCurve = AnimationCurve.EaseInOut(0,0,1,1);
    
    private PopoteTimer _wonderingTimer;
    private PopoteTimer _damagedTimer;

    protected override void Start() {
        _wonderingTimer = new PopoteTimer(Random.Range(_minWaitTime, _maxWaitTime));
        _wonderingTimer.OnTimerEnd+= OnTimerEnd;
        _damagedTimer = new PopoteTimer(_damageTime);
        base.Start();
    }

    protected override void Update() {
        _damagedTimer.UpdateTimer();
        if (_damagedTimer.IsPlaying)
        {
            ManagerDamaged();
            return;
        }
        _wonderingTimer.UpdateTimer();
        if (!_isMoving && !_wonderingTimer.IsPlaying) {
            _wonderingTimer.Play(Random.Range(_minWaitTime, _maxWaitTime));
        }
        base.Update();
    }

    private void OnTimerEnd(object sender, EventArgs e) {
        DoWanderingMovement(_wonderingDistance);
    }

    private void ManagerDamaged()
    {
        Debug.Log("ManagerDamaged with T ="+_damagedTimer.T);
        _spriteRenderer.material.SetFloat("_HitProgress", _damageCurve.Evaluate(_damagedTimer.T));
    }

    public override void TakeDamage(int damage, Vector2 direction, IDamagable.AttackerType attackerType)
    {
        _rb.AddForce(direction, ForceMode2D.Force);
        _damagedTimer.Play();
        _health -= damage;
        if (_health <= 0) Die();
        base.TakeDamage(damage, direction, attackerType);
    }
    
    private void Die(){
       Instantiate(_prfDeathParticule, transform.position, Quaternion.identity);
       Destroy(gameObject);
    }
}