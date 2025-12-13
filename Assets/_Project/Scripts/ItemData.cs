using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Item", menuName = "MinecraftSystem/Item")]
public class ItemData : ScriptableObject
{
    [Header("Identification")]
    public string id;
    public string displayName;
    [TextArea] public string description; //Tooltip

    [Header("Visual")]
    public Sprite icon;

    [Header("Inventory Logic")]
    public bool isStackable = true;
    public int maxStackSize = 64;
}