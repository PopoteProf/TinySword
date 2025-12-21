using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NavGridManager : MonoBehaviour
{
    public static NavGridManager Instance;
    
    [SerializeField] private Vector2Int _gridSize;
    [SerializeField] private Vector2Int _gridOffset;
    [SerializeField] private Tilemap _colliderMap;
    [SerializeField] private Vector2 _castSizeForDestrucible = new Vector2(0.5f,0.5f);
    [SerializeField] private string _destructibleTag = "Destructible";

    [Space(10), Header("Debug")]
    [SerializeField] private Tilemap _debugMap;
    [SerializeField] private Tile _greenDebugTile;
    [SerializeField] private Tile _debugRedTile = null;
    [SerializeField] private Tile _debugblueTile = null;
    [SerializeField] private Tile _debugPinkTile = null;
    
    private NavGridCell[,] _grid;

    public Vector2 GridOffset => _gridOffset;
    private void Awake() {
        Instance = this;
    }
    
    void Start()
    {
        BakeNavGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    public void DisplayDebugTile(NavGridCell navtile, Tile tile) =>DisplayDebugTile(navtile.Pos+_gridOffset, tile);
    public void DisplayDebugTile(Vector3Int pos, Tile tile) => _debugMap.SetTile(pos, tile);
    public void DisplayDebugTile(Vector2Int pos, Tile tile) => DisplayDebugTile(new Vector3Int(pos.x, pos.y, 0), tile);
    public Vector2 GetCenterWorldPos(NavGridCell cell) => cell.Pos + _gridOffset+new Vector2(0.5f,0.5f);
    [ContextMenu("Clear Debug Grid")] public void ClearDebugMap() => _debugMap.ClearAllTiles();

    public NavGridCell[] DoAStart(NavGridCell start, NavGridCell end) {
        List<NavGridCell> openList = new List<NavGridCell>();
        List<NavGridCell> closedList = new List<NavGridCell>();
        openList.Add(start);

        ClearDebugMap();
        while (openList.Count > 0)
        {
            
            NavGridCell currentCell = openList[0];
            openList.Remove(currentCell);
            if (currentCell.Pos == end.Pos)
            {
                List<NavGridCell> retrunList = new List<NavGridCell>();
                while (currentCell != start) {
                    retrunList.Add(currentCell);
                    currentCell = currentCell.FromCell;
                }
                foreach (var cell in openList) cell.ResetAStartData();
                foreach (var cell in closedList) cell.ResetAStartData();
                start.ResetAStartData();
                end.ResetAStartData();
                return retrunList.ToArray();
            }

            foreach (var neighbor in GetNeighbours4(currentCell)) {
                if (!neighbor.IsWalkableCell) continue;
                
                if (closedList.Contains(neighbor)) continue;
                if (neighbor.HCost == Int32.MaxValue) {
                    neighbor.HCost = Mathf.RoundToInt(Vector2.Distance(neighbor.Pos, end.Pos)*10);
                }

                if (neighbor.GCost > currentCell.GCost + 10) {
                    neighbor.GCost = currentCell.GCost + 10;
                    neighbor.FromCell = currentCell;
                    if(!openList.Contains(neighbor)) openList.Add(neighbor);
                       
                    
                }
            }
            //foreach (var neighbor in GetNeighbours4Diagonal(currentCell)) {
            //    if (!neighbor.IsWalkable)
            //    {
            //        DisplayDebugTile(neighbor, _debugRedTile);
            //        continue;
            //    }
            //    if (closedList.Contains(neighbor)) continue;
            //    if (neighbor.HCost == Int32.MaxValue) {
            //        neighbor.HCost = Mathf.RoundToInt(Vector2.Distance(neighbor.Pos, end.Pos)*10);
            //    }
//
            //    if (neighbor.GCost > currentCell.GCost + 10) {
            //        neighbor.GCost = currentCell.GCost + 10;
            //        neighbor.FromCell = currentCell;
            //        if(!openList.Contains(neighbor)){
            //            openList.Add(neighbor);
            //            DisplayDebugTile(neighbor, _greenDebugTile);
            //        }
            //    }
            //}
            openList.Sort();
            closedList.Add(currentCell);
        }
        foreach (var cell in openList) cell.ResetAStartData();
        foreach (var cell in closedList) cell.ResetAStartData();
        start.ResetAStartData();
        end.ResetAStartData();
       
        return null;
    }

    public NavGridCell[] GetBFS(NavGridCell start, int lenght,bool usDiagonals = false, bool excludeBlockade =true,  bool excludeNonWalkable = true) {
        List<NavGridCell>openList = new List<NavGridCell>();
        List<NavGridCell> closedList = new List<NavGridCell>();
        
        openList.Add(start);
        for (int i = 0; i < lenght; i++) {
            for (int j = openList.Count - 1; j >= 0; j--) {
                NavGridCell[] neughbors;
                if (usDiagonals) neughbors = GetNeighbours8(openList[j]);
                else neughbors = GetNeighbours4(openList[j]);
                foreach (var neighbor in neughbors) {
                    if (excludeNonWalkable && !neighbor.IsWalkable) continue;
                    if (excludeBlockade && neighbor.IsBlockade) continue;
                    if( openList.Contains(neighbor)|| closedList.Contains(neighbor)) continue;
                    openList.Add(neighbor);
                }
                closedList.Add(openList[j]);
                openList.RemoveAt(j);
            }
        }

        closedList.Remove(start);
        return closedList.ToArray();
    }

    public NavGridCell[] GetNeighbours4(NavGridCell cell) {
        if (cell == null) return null;
        List<NavGridCell> neighbours = new List<NavGridCell>();
        if( GetCell(cell.Up)!=null)neighbours.Add(GetCell(cell.Up));
        if( GetCell(cell.Down)!=null)neighbours.Add(GetCell(cell.Down));
        if( GetCell(cell.Left)!=null)neighbours.Add(GetCell(cell.Left));
        if( GetCell(cell.Right)!=null)neighbours.Add(GetCell(cell.Right));
        return neighbours.ToArray();
    }
    public NavGridCell[] GetNeighbours4Diagonal(NavGridCell cell) {
        if (cell == null) return null;
        List<NavGridCell> neighbours = new List<NavGridCell>();
        if( GetCell(cell.DownLeft)!=null)neighbours.Add(GetCell(cell.DownLeft));
        if(GetCell(cell.DownRight)!=null)neighbours.Add(GetCell(cell.DownRight));
        if(GetCell(cell.UpLeft)!=null)neighbours.Add(GetCell(cell.UpLeft));
        if(GetCell(cell.UpRight)!=null)neighbours.Add(GetCell(cell.UpRight));
        return neighbours.ToArray();
    }
    
    public NavGridCell[] GetNeighbours8(NavGridCell cell) {
        if (cell == null) return null;
        List<NavGridCell> neighbours = new List<NavGridCell>();
        if( GetCell(cell.Up)!=null)neighbours.Add(GetCell(cell.Up));
        if( GetCell(cell.Down)!=null)neighbours.Add(GetCell(cell.Down));
        if( GetCell(cell.Left)!=null)neighbours.Add(GetCell(cell.Left));
        if( GetCell(cell.Right)!=null)neighbours.Add(GetCell(cell.Right));
        if( GetCell(cell.DownLeft)!=null)neighbours.Add(GetCell(cell.DownLeft));
        if(GetCell(cell.DownRight)!=null)neighbours.Add(GetCell(cell.DownRight));
        if(GetCell(cell.UpLeft)!=null)neighbours.Add(GetCell(cell.UpLeft));
        if(GetCell(cell.UpRight)!=null)neighbours.Add(GetCell(cell.UpRight));
        return neighbours.ToArray();
    }

    public NavGridCell GetCell(Vector3 pos) {
        return GetCell(new Vector2Int(Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y))-_gridOffset);
    }
    public NavGridCell GetCell(Vector2Int pos) {
        if (pos.x < 0 || pos.x >= _gridSize.x || pos.y < 0 || pos.y >= _gridSize.y) return null;
        return _grid[pos.x, pos.y];
    }

    
    

    [ContextMenu("Bake Nav Grid")]
    private void BakeNavGrid() {
        _grid = new NavGridCell[_gridSize.x, _gridSize.y];
        for (int x = 0; x < _gridSize.x; x++) {
            for (int y = 0; y < _gridSize.y; y++) {
                 bool iswalkable =_colliderMap.GetTile(new Vector3Int(x+_gridOffset.x, y+_gridOffset.y, 0))==null;
                 _grid[x, y] = new NavGridCell(new Vector2Int(x, y), iswalkable);
                 if (iswalkable) {
                     RaycastHit2D[] hits =Physics2D.BoxCastAll(GetCenterWorldPos(_grid[x, y]), _castSizeForDestrucible, 0, Vector2.zero);
                     foreach (var hit in hits) {
                         if (hit.collider.CompareTag("WalkBlocker")) {
                             _grid[x, y].IsWalkable = false;
                             continue;
                         }
                         if (hit.collider.CompareTag(_destructibleTag)) {
                             _grid[x, y].AddDestructibleElement(hit.collider.GetComponent<DestructibleElements>());
                         }                         
                     }
                 }
            }
        }
        DisplayDebugGrid();
    }

    private void DisplayDebugGrid() {
        for (int x = 0; x < _gridSize.x; x++) {
            for (int y = 0; y < _gridSize.y; y++) {
                if (_grid[x,y].IsBlockade)DisplayDebugTile(_grid[x,y], _debugPinkTile);
                else if (_grid[x,y].IsWalkable) _debugMap.SetTile(new Vector3Int(x+_gridOffset.x, y+_gridOffset.y, 0), _greenDebugTile);
            }
        }
    }

   
    

    private void OnDrawGizmos() {
        Vector2 pos1 = _gridOffset;
        Vector2 pos2 = _gridOffset+new Vector2Int(_gridSize.x, 0);
        Vector2 pos3 = _gridOffset+ _gridSize;
        Vector2 pos4 = _gridOffset+new Vector2Int(0, _gridSize.y);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pos1, pos2);
        Gizmos.DrawLine(pos3, pos2);
        Gizmos.DrawLine(pos3, pos4);
        Gizmos.DrawLine(pos1, pos4);
    }
}