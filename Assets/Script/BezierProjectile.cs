using System;
using UnityEngine;

public class BezierProjectile : MonoBehaviour
{
    [SerializeField] protected float _projectileSpeed =2;
    [SerializeField] protected AnimationCurve _sizeOverLifeTime = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] protected float _damageRayLength = 0.3f;
    [SerializeField] protected GameObject _prfProjectilePrefab;
    protected Vector2 _startPos;
    protected Vector2 _starDirection;
    protected Vector2 _endPos;

    protected PopoteTimer _timer;
    protected Vector2 _lastPos;

    public virtual void SetUpBezierProjectile(Vector2 startPos, Vector2 startDirection, Vector2 endPos) {
        _starDirection =(Vector2.Distance(endPos, startPos)/2)*startDirection+startPos;
        //_starDirection = startDirection;
        _startPos = startPos;
        _endPos = endPos;
        _lastPos = startPos;
        float distance = Vector2.Distance(endPos, startDirection) + Vector2.Distance(startPos, startDirection);
        _timer = new PopoteTimer(distance/_projectileSpeed);
        _timer.OnTimerEnd += OnTimerEnd;
        _timer.Play();
        }

    protected virtual void OnTimerEnd(object sender, EventArgs e) {
        GameObject go = Instantiate(_prfProjectilePrefab, transform.position, transform.rotation);
        go.transform.right = transform.right;
        Destroy(gameObject);
    }

    protected virtual void Update() {
        _timer.UpdateTimer();
        transform.position = Bezier(_startPos, _starDirection, _endPos, _timer.T);
        transform.right = (Vector2)transform.position-_lastPos;
        _lastPos = transform.position;
        
        transform.localScale = Vector3.one*_sizeOverLifeTime.Evaluate(_timer.T);
        ManageRayHit();
        
    }

    protected virtual Vector2 Bezier(Vector2 a, Vector2 b, Vector2 c, float t) {
       Vector2 ab =Vector2.Lerp(a,b,t);
       Vector2 bc =Vector2.Lerp(b,c,t);
       return Vector2.Lerp(ab,bc,t);
    }

    protected virtual void ManageRayHit(){
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, _damageRayLength);
        foreach (var hit in hits) {
            if (hit.transform.GetComponent<PlayerController>()!=null) {
                hit.transform.GetComponent<PlayerController>().TakeDamage(1, transform.right, IDamagable.AttackerType.Enemy);
                OnTimerEnd(this, EventArgs.Empty);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position+transform.right*_damageRayLength);
    }
}