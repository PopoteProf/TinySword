using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class NavGridAgent : MonoBehaviour ,IDamagable
{

    [SerializeField] private float _moveSpeedForce = 5;
    [SerializeField] private float _moveSpeedMax = 5;
    [SerializeField] private float _rangeToNextCell = 0.4f;
    [SerializeField] private float _rangeOutOfCell =1f;
    [SerializeField] private Tile _debugblueTile = null;
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected Animator _animator;
    [SerializeField]
    protected NavGridManager _gridManager;
    protected Rigidbody2D _rb;
    

    protected NavGridCell[] _navigationPath;
    protected int _currentpathID = 0;
    protected Vector3 _targetPosition;
    protected bool _isMoving = false;
    protected virtual void Start() {
        _gridManager = NavGridManager.Instance;
        if (_gridManager == null) {
            Debug.LogWarning("No Grid Manager Found", this);
        }

        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update() {
        ManageLocomotion();
        ManagerVisual();
    }

    protected virtual void ManageLocomotion() {
        if (_navigationPath != null) {
            
            //Debug.Log("tested path ID: " + _currentpathID);
            Vector2 direction = _gridManager.GetCenterWorldPos(_navigationPath[_currentpathID]) - _rb.position;
            _rb.AddForce(direction * (_moveSpeedForce*Time.deltaTime));
            _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, _moveSpeedMax);
            if (direction.magnitude < _rangeToNextCell) {
                if (0>= _currentpathID ) {
                    _navigationPath =null;
                    //Debug.Log("Agent get to destination");
                    //_rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, 0.5f);;
                    OnStopWalking();
                    return;
                }
                _currentpathID--;
                //Debug.Log("current path ID: " + _currentpathID);
            }
            

            if (direction.magnitude > _rangeOutOfCell) {
                SetNewDestination(_targetPosition);
            }
        }
    }

    protected virtual void ManagerVisual()
    {
        _isMoving = _rb.linearVelocity.magnitude > 0.2f;
        _animator.SetBool("IsWalking",_isMoving);
        if (_rb.linearVelocity.magnitude > 0.2f)
        {
            _spriteRenderer.flipX = _rb.linearVelocity.x < 0;
        }
    }

    protected virtual void DoWanderingMovement(int wonderingDistance) { 
        NavGridCell[] cells =_gridManager.GetBFS(_gridManager.GetCell(transform.position), wonderingDistance);
        SetNewDestination(_gridManager.GetCenterWorldPos(cells[Random.Range(0, cells.Length)]));
    }

    public virtual void SetNewDestination(Vector3 targetPos) {
        //Debug.Log("Destination set =" + targetPos);
        NavGridCell targetCell =_gridManager.GetCell(targetPos);
        if (targetCell == null) {
            Debug.LogWarning("NavGridAgent don't have valid destination", this);
            return;
        }

        _targetPosition = targetPos;
        NavGridCell startCell = _gridManager.GetCell(transform.position);
        if (startCell == null) {
            Debug.LogWarning("NavGridAgent don't have valid Start", this);
            return;
        }
        _navigationPath = _gridManager.DoAStart(startCell, targetCell);
        if (_navigationPath == null || _navigationPath.Length == 0) {
            _navigationPath = null;
            return;  
        }

        _currentpathID = _navigationPath.Length-1;
        OnStartWalking();
    }

    public void AddForce(Vector2 force) {
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

    protected virtual void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _rangeToNextCell);
        Gizmos.DrawWireSphere(transform.position, _rangeOutOfCell);

        if (_navigationPath != null && _gridManager!=null&& _currentpathID>=0){
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, _gridManager.GetCenterWorldPos(_navigationPath[_currentpathID]));
        }
    }

    public virtual void TakeDamage(int damage, Vector2 direction, IDamagable.AttackerType attackerType) { }
    protected virtual void OnStartWalking() { }
    protected virtual void OnStopWalking() { }
}