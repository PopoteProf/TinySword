using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public static class StaticData
{
    public static Vector2 PlayerPos;
    
    public static Action<float> PlayerHealthChanged;
    public static Action<SO_DialogueData> StartPlayingDialogue;
    public static Action EndPlayingDialogue;
    public static Action OnRessourcesChanged;
    public static Action<string> OnEnterArea;
    public static Action OnInventoryChange;

    public enum RessourcesType { Gold, Wood, Food }
    public static int Gold;
    public static int Wood;
    public static int Food;
    public static List<SOItem> _inventory = new List<SOItem>();

    public static void SetUpInitialRessources(int gold, int wood, int food) {
        Gold = gold;
        Wood = wood;
        Food = food;
    }
    public static void AddItemToInventory (SOItem newItem) {
        _inventory.Add(newItem);
        OnInventoryChange?.Invoke();
    }

    public static void RemoveItemFromInventory(SOItem itemToRemove) {
        if (_inventory.Contains(itemToRemove))
        {
            _inventory.Remove(itemToRemove);
            OnInventoryChange?.Invoke();
        }
    }
    
    public static void ChangeGold(int value)
    {
        Gold = Mathf.Clamp(Gold + value, 0, Gold + value);
        Debug.Log("Gold changed to " + Gold);
        OnRessourcesChanged?.Invoke();
    }

    public static void ChangeWood(int value) {
        Wood = Mathf.Clamp(Wood + value, 0,Wood + value );
        OnRessourcesChanged?.Invoke();
    }
    
    public static void ChangeFood(int value) {
        Food = Mathf.Clamp(Food + value, 0,Food + value );
        OnRessourcesChanged?.Invoke();
    }
    
}