using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IDamagable {
    [SerializeField] private PlayerInteractor _playerInteractor;
    [Space(5)]
    [SerializeField] private bool _controlBlock;
    [SerializeField] private float _moveSpeed = 100;
    [SerializeField] private float _maxMoveSpeed = 2.5f;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [Header("Health")] [SerializeField] private int _health = 4;
    [SerializeField] private int _maxHealt = 4;
    [SerializeField] private GameObject _prfDeathParticule;

    [Space(10), Header("Attacks")] [SerializeField]
    private int _attackDamage = 2;

    [SerializeField] private float _attackForcePower = 10;
    [SerializeField] private float _attackTime = 0.3f;
    [SerializeField] private Bounds _leftAttackBounds;
    [SerializeField] private Bounds _rightAttackBounds;

    [Header("DamagedParameters")] [SerializeField]
    private float _damageTime = 0.75f;

    [SerializeField] private AnimationCurve _damageCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private CinemachineImpulseSource _attackImpulceSource;
    [SerializeField] private CinemachineImpulseSource _damagedImpulceSource;

    private Rigidbody2D _rb;
    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _interactAction;
    private bool _isMoving = false;
    private bool _isXFlipped = false;
    private bool _attack2Buffer = false;
    private bool _attack1Buffer = false;
    private PlayerAttackStat _playerAttackStat;

    private PopoteTimer _attackTimer;
    private PopoteTimer _damagedTimer;

    private enum PlayerAttackStat
    {
        None,
        Attack1,
        Attack2
    }

    public bool IsAttacking
    {
        get => _playerAttackStat != PlayerAttackStat.None;
    }

    public float NormalizeHealth {
        get =>(float)_health/_maxHealt;
    }

    private void Awake() {
        StaticData.StartPlayingDialogue+= StartPlayingDialogue;
        StaticData.EndPlayingDialogue+= EndPlayingDialogue;
    }

    private void OnDestroy() {
        StaticData.StartPlayingDialogue-= StartPlayingDialogue;
        StaticData.EndPlayingDialogue-= EndPlayingDialogue;
    }

    private void EndPlayingDialogue() {
       Invoke("SetPlayerControlBlockToFalse", 0.1f);
    }

    private void StartPlayingDialogue(SO_DialogueData obj) =>  SetPlayerControlBlock(true);
    private void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _moveAction =InputSystem.actions.FindAction("Move");
        _attackAction = InputSystem.actions.FindAction("Attack");
        _attackAction.started+= AttackActionOnstarted;
        _interactAction = InputSystem.actions.FindAction("Interact");
        _interactAction.started+= InteractActionOnstarted;
        _attackTimer = new PopoteTimer(_attackTime);
        _attackTimer.OnTimerEnd+= OnAttackTimerEnd;
        _damagedTimer = new PopoteTimer(_damageTime);
        
        
        StaticData.PlayerHealthChanged.Invoke(NormalizeHealth);
    }

    

    private void SetPlayerControlBlockToFalse() => SetPlayerControlBlock(false);
    public void SetPlayerControlBlock(bool controlBlock) {
        _controlBlock = controlBlock;
        _playerInteractor.SetCanInteract(!controlBlock);
    }
    
    private void AttackActionOnstarted(InputAction.CallbackContext obj) {
        if (_controlBlock) return;
        if( _attackTimer.IsPlaying) {
            if (_playerAttackStat == PlayerAttackStat.Attack1) _attack2Buffer = true;
            if (_playerAttackStat == PlayerAttackStat.Attack2) _attack1Buffer = true;
            return;
        }
        DoAttack();
    }

    private void DoAttack()
    {
        if (_attack2Buffer) {
            _animator.SetTrigger("Attack2");
            _playerAttackStat = PlayerAttackStat.Attack2;
        } else {
            _animator.SetTrigger("Attack");
            _playerAttackStat = PlayerAttackStat.Attack1;
        }
        _attackTimer.Play();
        _animator.SetTrigger("Attack");
        if( !_isXFlipped)MakeDamage(_rightAttackBounds);
        else MakeDamage(_leftAttackBounds);
        _attack2Buffer = false;
        _attack1Buffer = false;
        if (_attackImpulceSource)_attackImpulceSource.GenerateImpulse();
    }
    private void OnAttackTimerEnd(object sender, EventArgs e)
    {
        if (_attack2Buffer || _attack1Buffer) DoAttack();
        else _playerAttackStat = PlayerAttackStat.None;
    }

    private void MakeDamage(Bounds bound) {
        RaycastHit2D[] hit2Ds= Physics2D.BoxCastAll(transform.position + bound.center, bound.size, 0, Vector2.zero);
        foreach (var hit in hit2Ds)
        {
            if (hit.collider.gameObject == gameObject) continue;
            if (hit.collider.GetComponent<IDamagable>()!=null) {
                Vector2 dir = hit.collider.transform.position - transform.position;
                dir.Normalize();
                hit.collider.GetComponent<IDamagable>().TakeDamage(_attackDamage,dir*_attackForcePower, IDamagable.AttackerType.Player );
            }
        }
    }

    private void Update() {
        StaticData.PlayerPos =transform.position;
        _attackTimer.UpdateTimer();
        _damagedTimer.UpdateTimer();
        if (_damagedTimer.IsPlaying)
        {
            ManagerDamaged();
            return;
        }

        if (!IsAttacking && !_controlBlock) {
            ManageMovement();
        }
        ManagerVisual();
    }
    private void InteractActionOnstarted(InputAction.CallbackContext obj) {
        if (IsAttacking || _controlBlock) return;
        _playerInteractor.Interact();
    }

    private void ManageMovement() {
        Vector2 moveDirection = _moveAction.ReadValue<Vector2>();
        _rb.AddForce(moveDirection *( _moveSpeed*Time.deltaTime));
        _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, _maxMoveSpeed);
    }
    
    protected virtual void ManagerVisual() {
        _isMoving = _rb.linearVelocity.magnitude > 0.2f;
        _animator.SetBool("IsWalking",_isMoving);
        if (_rb.linearVelocity.magnitude > 0.2f) {
            _spriteRenderer.flipX = _isXFlipped = _rb.linearVelocity.x < 0;;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position+ _leftAttackBounds.center, _leftAttackBounds.size);
        Gizmos.DrawWireCube(transform.position+ _rightAttackBounds.center, _rightAttackBounds.size);
    }

    public void TakeDamage(int damage, Vector2 direction, IDamagable.AttackerType attackerType)
    {
        if (_damagedImpulceSource)_damagedImpulceSource.GenerateImpulse();
        _rb.AddForce(direction, ForceMode2D.Force);
        _damagedTimer.Play();
        _health -= damage;
        StaticData.PlayerHealthChanged.Invoke(NormalizeHealth);
        if (_health <= 0) Die();
    }
    private void ManagerDamaged() {
        _damagedTimer.UpdateTimer();
        _spriteRenderer.material.SetFloat("_HitProgress", _damageCurve.Evaluate(_damagedTimer.T));
    }
    private void Die(){
        Instantiate(_prfDeathParticule, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void HealPlayer(int healAmount) {
        _health = Mathf.Clamp(_health+healAmount,0,_maxHealt );
        StaticData.PlayerHealthChanged.Invoke(NormalizeHealth);
    }
}