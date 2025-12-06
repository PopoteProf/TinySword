using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArcherEnemy : NavGridAgent 
{
    [SerializeField] private float _aggresionRange = 6;
    [SerializeField] private float _chaseRangeRecalculPath = 1.5f;
    [SerializeField] private float _attackTriggerRange = 1.3f;
    
    [Space(10), Header("Attacks")] 
    [SerializeField] private int _attackDamage = 2;
    [SerializeField] private float _attackTime =0.7f;
    [Range(0, 1), SerializeField] private float _deplayBeforeFirering = 0.5f;
    [SerializeField] private BezierProjectile _prfBezierProjectile;

    [SerializeField] private Vector2 _leftProjecticleSpawn;
    [SerializeField] private Vector2 _leftProjecticleDirection;
    [SerializeField] private Vector2 _rightProjecticleSpawn;
    [SerializeField] private Vector2 _rightProjecticleDirection;
    [Header("Health")]
    [SerializeField] private int _health =4;
    [SerializeField] private GameObject _prfDeathParticule;
    [Header("WonderingParameters")]
    [SerializeField] private float _minWaitTime = 1;
    [SerializeField] private float _maxWaitTime = 5;
    [SerializeField] private int _wonderingDistance = 3;
    [Header("DamagedParameters")]
    [SerializeField] private float _damageTime = 0.75f;
    [SerializeField] private AnimationCurve _damageCurve = AnimationCurve.EaseInOut(0,0,1,1);
    [Header("Flee Parameters")]
    [SerializeField] private float _fleeTriggerDistance = 3f;
    [SerializeField, Range(0, 1)] private float _fleeChance = 0.5f;
    [SerializeField] private int _fleeDistance = 4;
    

   
    
    
    private bool _isTriggered = false;
    private EnnemiStat _ennemiStat;
    public enum EnnemiStat {
        Idle, Walk, Attacking, Damaged, Fleeing
    }
    private PopoteTimer _wonderingTimer;
    private PopoteTimer _attackTimer;
    private PopoteTimer _damagedTimer;
    private bool _haveAttacked = false;

    protected override void Start() {
        _wonderingTimer = new PopoteTimer(Random.Range(_minWaitTime, _maxWaitTime));
        _wonderingTimer.OnTimerEnd+= OnTimerWonderingEnd;
        _damagedTimer = new PopoteTimer(_damageTime);
        _damagedTimer.OnTimerEnd += OnTimerDamagedEnd;
        _attackTimer = new PopoteTimer(_attackTime);
        _attackTimer.OnTimerEnd+= OnTimerAttackEnd;
        base.Start();
    }
    
    protected override void Update() {

        switch (_ennemiStat)
        {
            case EnnemiStat.Idle: ManageIdleState(); break;
            case EnnemiStat.Walk: ManageWalkState(); break;
            case EnnemiStat.Attacking: ManageAttackState();break;
            case EnnemiStat.Damaged: ManagerDamaged(); break;
            case EnnemiStat.Fleeing: ManageFleeState(); break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void CheckIfPlayerInAgroRange() {
        if (Vector2.Distance(StaticData.PlayerPos, transform.position) <= _aggresionRange) {
            _isTriggered = true;
            SetNewDestination(StaticData.PlayerPos);
        }
    }
    #region Idle

    
    private void ChangStatToIdle() {
        //Debug.Log("Idle");
        _wonderingTimer.Play(Random.Range(_minWaitTime, _maxWaitTime));
        _ennemiStat = EnnemiStat.Idle;
    }

    private void ManageIdleState() {
        if( _isTriggered)SetNewDestination(StaticData.PlayerPos);
        _wonderingTimer.UpdateTimer();
        CheckIfPlayerInAgroRange();
        ManagerVisual();
        CheckIfPlayerInAttackRange();
        
    }
    private void OnTimerWonderingEnd(object sender, EventArgs e) {
        DoWanderingMovement(_wonderingDistance);
    }
    #endregion
    #region Walk
    protected override void OnStartWalking() {
        _ennemiStat = EnnemiStat.Walk;
    }

    private void ManageWalkState() {
        //Debug.Log("Walk");
        ManageLocomotion();
        if( !_isTriggered)CheckIfPlayerInAgroRange();
        else ManageChase();
        ManagerVisual();
        CheckIfPlayerInAttackRange();
    }

    protected override void OnStopWalking() {
        if (_attackTimer.IsPlaying) return;
        ChangStatToIdle();
    }

    private void ManageChase()
    {
        if (Vector2.Distance(_targetPosition, transform.position) > _chaseRangeRecalculPath) {
            SetNewDestination(StaticData.PlayerPos);
        }
    }
    #endregion
    #region Damaged
    
    private void ChangStatToDamaged() {
        _ennemiStat = EnnemiStat.Damaged;
        _attackTimer.Pause();
        _damagedTimer.Play();
    }

    private void ManagerDamaged() {
        _damagedTimer.UpdateTimer();
        _spriteRenderer.material.SetFloat("_HitProgress", _damageCurve.Evaluate(_damagedTimer.T));
    }
    private void OnTimerDamagedEnd(object sender, EventArgs e) {
        ChangStatToIdle();
    }

    public override void TakeDamage(int damage, Vector2 direction, IDamagable.AttackerType attackerType) {
        _rb.AddForce(direction, ForceMode2D.Force);
        _damagedTimer.Play();
        _health -= damage;
        ChangStatToDamaged();
        _animator.SetTrigger("AnimationCancel");
        
        if (_health <= 0) Die();
        base.TakeDamage(damage, direction, attackerType);
    }
    
    private void Die(){
        Instantiate(_prfDeathParticule, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    #endregion
    #region Attack
    
    private void CheckIfPlayerInAttackRange() {
        if( CheckIfHaveToFlee())return;
        //Debug.Log("DoAttack");
        if (Vector2.Distance(StaticData.PlayerPos, transform.position) <= _attackTriggerRange) {
            _rb.linearVelocity = Vector2.zero;
            _spriteRenderer.flipX = (StaticData.PlayerPos.x- transform.position.x)<0;
            _ennemiStat = EnnemiStat.Attacking;
            _animator.SetTrigger("Attack");
            _attackTimer.Play();
        }
    }

    private void ManageAttackState() {
        _attackTimer.UpdateTimer();
        _animator.SetBool("IsWalking",false);
        if (_haveAttacked)return;
        if (_attackTimer.T > _deplayBeforeFirering) {
            if( _spriteRenderer.flipX)SpawnProjectile( (Vector2)transform.position+_rightProjecticleSpawn,(Vector2)transform.position+_rightProjecticleDirection);
            else SpawnProjectile((Vector2)transform.position+_leftProjecticleSpawn, (Vector2)transform.position+_leftProjecticleDirection);
            _haveAttacked = true;
            
        }
    }

    private void SpawnProjectile(Vector2 startPos, Vector2 direction)
    {
        BezierProjectile projectile = Instantiate(_prfBezierProjectile,  startPos, Quaternion.identity);
        projectile.SetUpBezierProjectile(startPos,direction, StaticData.PlayerPos);
            
    }
    //private void MakeDamage(Bounds bound) {
    //    RaycastHit2D[] hit2Ds= Physics2D.BoxCastAll(transform.position + bound.center, bound.size, 0, Vector2.zero);
    //    foreach (var hit in hit2Ds) {
    //        if (hit.collider.gameObject == gameObject) continue;
    //        if (hit.collider.GetComponent<IDamagable>()!=null) {
    //            Vector2 dir = hit.collider.transform.position - transform.position;
    //            dir.Normalize();
    //            hit.collider.GetComponent<IDamagable>().TakeDamage(_attackDamage,dir*_attackForcePower, IDamagable.AttackerType.Player );
    //        }
    //    }
    //}
    private void OnTimerAttackEnd(object sender, EventArgs e)
    {
        Debug.Log("Finish attack");
        _haveAttacked = false;
        ChangStatToIdle();
    }

    #endregion

    #region  Fleeing

    private bool CheckIfHaveToFlee() {
        if (Vector2.Distance(StaticData.PlayerPos, transform.position) > _fleeTriggerDistance)  return false;
        if (Random.Range(0, 1) > _fleeChance)  return false;
        NavGridCell[] cells =_gridManager.GetBFS(_gridManager.GetCell(transform.position),_fleeDistance);
        SetNewDestination(_gridManager.GetCenterWorldPos(GetFarestCell(cells, StaticData.PlayerPos)));
        _ennemiStat = EnnemiStat.Fleeing;
        return true;
    }

   private NavGridCell GetFarestCell(NavGridCell[] cells, Vector2 worldPos) {
       Vector2 refPos = worldPos-_gridManager.GridOffset;
       NavGridCell bestCell = cells[0];

       foreach (var cell in cells) {
           if (Vector2.Distance(cell.Pos, refPos) > Vector2.Distance(refPos, bestCell.Pos))
           {
               bestCell = cell;
           }
       }
       return bestCell;
   }

    private void ManageFleeState() {
        ManageLocomotion();
        ManagerVisual();
    }

    #endregion
    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _aggresionRange);
        Gizmos.DrawWireSphere(transform.position, _attackTriggerRange);
        Gizmos.DrawSphere((Vector2)transform.position+ _rightProjecticleSpawn,0.05f );
        Gizmos.DrawLine((Vector2)transform.position+ _rightProjecticleSpawn, (Vector2)transform.position+ _rightProjecticleSpawn+_rightProjecticleDirection);
        Gizmos.DrawSphere((Vector2)transform.position+ _leftProjecticleSpawn,0.05f );
        Gizmos.DrawLine((Vector2)transform.position+ _leftProjecticleSpawn, (Vector2)transform.position+ _leftProjecticleSpawn+_leftProjecticleDirection);
        //Gizmos.DrawWireCube(transform.position+ _leftAttackBounds.center, _leftAttackBounds.size);
        //Gizmos.DrawWireCube(transform.position+ _rightAttackBounds.center, _rightAttackBounds.size);
        
        base.OnDrawGizmos();
    } 
}