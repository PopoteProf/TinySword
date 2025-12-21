using System;
using System.Collections.Generic;
using UnityEngine;

public class NavGridCell : IComparable<NavGridCell>
{
    public Vector2Int Pos;
    public bool IsWalkable;
    public List<DestructibleElements> DestructibleElements = new List<DestructibleElements>();
    
    
    public int HCost = int.MaxValue;
    public int Fcost;
    public NavGridCell FromCell;

    private int _gCost =int.MaxValue;

    public bool IsBlockade {
        get => DestructibleElements.Count != 0;
    }
    public int GCost {
        get { return _gCost; }
        set {
            Fcost = value + HCost;
            _gCost = value;
        }
    }

    public bool IsWalkableCell {
        get {
            if( !IsWalkable)return false;
            if (DestructibleElements != null && DestructibleElements.Count != 0) return false;
            return true;
        }
    }
    
    public Vector2Int Up { get => Pos + Vector2Int.up; }
    public Vector2Int Down { get => Pos + Vector2Int.down; }
    public Vector2Int Left { get => Pos + Vector2Int.left; }
    public Vector2Int Right { get => Pos + Vector2Int.right; }
    public Vector2Int UpLeft { get => Pos + Vector2Int.up+Vector2Int.left; }
    public Vector2Int UpRight { get => Pos + Vector2Int.up+Vector2Int.right; }
    public Vector2Int DownLeft { get => Pos + Vector2Int.down+Vector2Int.left; }
    public Vector2Int DownRight { get => Pos + Vector2Int.down+Vector2Int.right; }
    public NavGridCell(Vector2Int pos, bool isWalkable) {
        Pos = pos;
        IsWalkable = isWalkable;
    }
    

    public int CompareTo(NavGridCell other) {
        return Fcost.CompareTo(other.Fcost);
    }

    public void AddDestructibleElement(DestructibleElements destructibleElement) {
        if( destructibleElement == null ) return;
        destructibleElement.OnElementsDestruction+= DestructibleElementOnOnElementsDestruction;
        DestructibleElements.Add(destructibleElement);
    }

    private void DestructibleElementOnOnElementsDestruction(object sender, DestructibleElements e) {
        DestructibleElements.Remove(e);
    }

    public void ResetAStartData() {
        HCost = int.MaxValue;
        GCost = int.MaxValue;
        FromCell = null;
    }
}