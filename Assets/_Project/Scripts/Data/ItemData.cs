using UnityEngine;
using System;
/// <summary>
/// Information Container for Items
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "MinecraftSystem/Item")]
public class ItemData : ScriptableObject
{
    [Header("Identification")]
    public string id;
    public string displayName;
    [TextArea] public string description; // Text displayed in UI tooltips

    [Header("Visual")]
    public Sprite icon;

    [Header("Inventory Logic")]
    public bool isStackable = true;
    public int maxStackSize = 64;
    public int defaultResultAmount = 1;
}