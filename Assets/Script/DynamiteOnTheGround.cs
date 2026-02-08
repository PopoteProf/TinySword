using System;
using UnityEngine;

public class DynamiteOnTheGround : ArrowInGround
{
    [SerializeField] private float _damageRange;
    [SerializeField] private int _damage =4;
    [SerializeField] private float _damageForce =10;
    [SerializeField] private GameObject _prfExplosion;
    [SerializeField] private AudioElement _audioElementExplision;

    protected override void OnAnimationTimerEnd(object sender, EventArgs e) {
        _audioElementExplision.PlayAsSFX();
        Instantiate(_prfExplosion, transform.position, Quaternion.identity);
        RaycastHit2D[] hit2Ds= Physics2D.CircleCastAll(transform.position , _damageRange, Vector2.one);
        foreach (var hit in hit2Ds) {
            if (hit.collider.gameObject == gameObject) continue;
            if (hit.collider.GetComponent<IDamagable>()!=null) {
                Vector2 dir = hit.collider.transform.position - transform.position;
                dir.Normalize();
                hit.collider.GetComponent<IDamagable>().TakeDamage(_damage,dir*_damageForce, IDamagable.AttackerType.Player );
            }
        }
        base.OnPostAnimTimerEnd(sender, e);
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _damageRange);
    }
}