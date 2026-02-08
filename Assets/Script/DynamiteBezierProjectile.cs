using UnityEngine;

public class DynamiteBezierProjectile : BezierProjectile
{
    [SerializeField] protected float _minStartRotation= -720;
    [SerializeField] protected float _maxStartRotation = 720;
    [Space(5)]
    [SerializeField] protected float _minEndRotation =-180;
    [SerializeField] protected float _maxEndRotation=180;

    private float _startRotation;
    private float _endRotation;
    public override void SetUpBezierProjectile(Vector2 startPos, Vector2 startDirection, Vector2 endPos) {
        _startRotation = Random.Range(_minStartRotation, _maxStartRotation);
        _endRotation = Random.Range(_minEndRotation, _maxEndRotation);
        transform.eulerAngles = new Vector3(0,0, _startRotation);
        base.SetUpBezierProjectile(startPos, startDirection, endPos);
    }
    protected override void Update() {
        _timer.UpdateTimer();
        transform.position = Bezier(_startPos, _starDirection, _endPos, _timer.T);
        transform.eulerAngles = new Vector3(0,0,Mathf.Lerp(_startRotation,_endRotation,_timer.T));
        _lastPos = transform.position;
        
        transform.localScale = Vector3.one*_sizeOverLifeTime.Evaluate(_timer.T);
    }
}