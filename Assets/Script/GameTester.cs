using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GameTester : MonoBehaviour
{

    [SerializeField] private Tile _debugRedTile = null;
    [SerializeField] private Tile _debugblueTile = null;
    [SerializeField] private NavGridAgent _agent;
    [SerializeField] private float _debugforce;

    [Space(10), Header("BFS test")] [SerializeField]
    private int _bfsLeagth = 3;

    [SerializeField] private bool _bfsUsDiagonale = false;
    [SerializeField] private bool _excludeBlockable =true;
    [SerializeField] private bool _excludeNonWalkable =true;
    
    private NavGridCell _startCell;
    private NavGridCell _endCell;
    private InputAction _jumpInput;
    private InputAction _interactInput;

    private void Start()
    {
        _jumpInput = InputSystem.actions.FindAction("Jump");
        _jumpInput.started += context =>  _agent.AddForce(Vector2.one*_debugforce);
        _interactInput = InputSystem.actions.FindAction("Interact");
        _interactInput.started += InteractInputOnstarted;
    }

    private void InteractInputOnstarted(InputAction.CallbackContext obj) {
        NavGridCell[] cells =NavGridManager.Instance.GetBFS(
            NavGridManager.Instance.GetCell(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue())),
            _bfsLeagth, _bfsUsDiagonale, _excludeBlockable, _excludeNonWalkable);
        NavGridManager.Instance.ClearDebugMap();
        foreach (NavGridCell cell in cells) {
            NavGridManager.Instance.DisplayDebugTile(cell, _debugblueTile);
        }
    }

    public void Update() {
        //if (Mouse.current.leftButton.wasPressedThisFrame) {
//
        //    Debug.Log(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        //    NavGridCell cell =
        //        NavGridManager.Instance.GetCell(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        //    if (cell == null) return;
        //    _startCell = cell;
        //    NavGridManager.Instance.DisplayDebugTile(_startCell, _debugblueTile);
        //    CheckForAStart();
        //}
//
        //if (Mouse.current.rightButton.wasPressedThisFrame) { 
        //    NavGridCell cell =
        //        NavGridManager.Instance.GetCell(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        //    if (cell == null) return;
        //    _endCell = cell;
        //    NavGridManager.Instance.DisplayDebugTile(_endCell, _debugRedTile);
        //    CheckForAStart();
        //}
        if (Mouse.current.rightButton.wasPressedThisFrame && _agent != null) {
            _agent.SetNewDestination(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        }
        

       // if (_jumpInput.inProgress)
       // {
       //     Debug.Log("Jump");
       //     _agent.AddForce(Vector2.one*_debugforce);
       // }
        
    }

    private void CheckForAStart() {
        if (_startCell != null && _endCell != null) {
            NavGridCell[] path =NavGridManager.Instance.DoAStart(_startCell,_endCell);
            //NavGridManager.Instance.ClearDebugMap();

            if (path == null)
            {
                Debug.Log("return path is null", this);
                
                _startCell = null;
                _endCell = null;
                return;
            }
            foreach (var cell in path) {
                NavGridManager.Instance.DisplayDebugTile(cell, _debugblueTile);
            }
            _startCell = null;
            _endCell = null;
        }
    }
    
}