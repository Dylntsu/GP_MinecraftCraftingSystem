using UnityEngine;
using System.Collections.Generic;
using System; 

// active data (recipe SO)
[CreateAssetMenu(fileName = "New Recipe", menuName = "MinecraftSystem/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("Recipe Definition")]
    // item list & amount needed for recipe
    public List<RequiredItem> requiredItems = new List<RequiredItem>();

    [Header("Crafting Result")]
    // item result & amount
    public ItemData resultItem; 
    public int resultAmount = 1; 
}

//Ingredient Structure
[Serializable]
public class RequiredItem
{
    public ItemData itemData;
    public int amount;
}