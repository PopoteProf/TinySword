using System;
using UnityEngine;

public class BezierProjectile : MonoBehaviour
{
    [SerializeField] private float _projectileSpeed =2;
    [SerializeField] private AnimationCurve _sizeOverLifeTime = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private float _damageRayLength = 0.3f;
    [SerializeField] private GameObject _prfProjectilePrefab;
    private Vector2 _startPos;
    private Vector2 _starDirection;
    private Vector2 _endPos;

    private PopoteTimer _timer;
    private Vector2 _lastPos;

  public void SetUpBezierProjectile(Vector2 startPos, Vector2 startDirection, Vector2 endPos) {
        
         _starDirection =(Vector2.Distance(endPos, startDirection)/2)*startDirection.normalized+startPos;
        //_starDirection = startDirection;
        _startPos = startPos;
        _endPos = endPos;
        _lastPos = startPos;
        _timer = new PopoteTimer(Vector2.Distance(endPos, startDirection)/_projectileSpeed);
        _timer.OnTimerEnd += OnTimerEnd;
        _timer.Play();
        }

    private void OnTimerEnd(object sender, EventArgs e) {
        GameObject go = Instantiate(_prfProjectilePrefab, transform.position, transform.rotation);
        go.transform.right = transform.right;
        Destroy(gameObject);
    }

    public void Update() {
        _timer.UpdateTimer();
        transform.position = Bezier(_startPos, _starDirection, _endPos, _timer.T);
        transform.right = (Vector2)transform.position-_lastPos;
        _lastPos = transform.position;
        
        transform.localScale = Vector3.one*_sizeOverLifeTime.Evaluate(_timer.T);
        ManageRayHit();
        
    }

    private Vector2 Bezier(Vector2 a, Vector2 b, Vector2 c, float t) {
       Vector2 ab =Vector2.Lerp(a,b,t);
       Vector2 bc =Vector2.Lerp(b,c,t);
       return Vector2.Lerp(ab,bc,t);
    }

    private void ManageRayHit(){
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, _damageRayLength);
        Debug.Log("Hit"+hits.Length);
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