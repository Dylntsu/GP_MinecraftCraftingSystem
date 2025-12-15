using UnityEngine;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    [Header("Dependencies")]
    public InventoryManager inventoryManager;

    [Header("Recipe Database")]
    public List<CraftingRecipe> availableRecipes;

    public bool CanCraft(CraftingRecipe recipe)
    {
        // Verify the required ingredients in the recipe
        foreach (RequiredItem required in recipe.requiredItems)
        {
            // Search for the total amount of the required item in the entire inventory
            int totalOwned = CountItem(required.itemData); 

            // If the amount we have is less than required, canot craft
            if (totalOwned < required.amount)
            {
                Debug.Log($"Failed to craft {recipe.resultItem.displayName}: Missing {required.itemData.displayName}. Needed {required.amount}, have {totalOwned}.");
                return false; // Fails on the first requirement not met
            }
        }

        // If the loop finishes without returning 'false', it means all requirements are met.
        return true; 
    }

    private int CountItem(ItemData itemToCount)
    {
        int count = 0;
        
        // Iterate over all logical inventory slots
        foreach (InventorySlot slot in inventoryManager.inventory)
        {
            // If the slot contains the item we are looking for
            if (slot.item == itemToCount) 
            {
                // Add the stack amount
                count += slot.stackSize;
            }
        }
        return count;
    }
}