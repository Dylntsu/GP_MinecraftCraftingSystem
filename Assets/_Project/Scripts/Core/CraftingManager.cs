using UnityEngine;
using System.Collections.Generic;

public class CraftingManager : MonoBehaviour
{
    [Header("Dependencies")]
    public InventoryManager inventoryManager;

    [Header("Recipe Database")]
    public List<CraftingRecipe> availableRecipes;

    // Stores the currently detected recipe to be consumed later
    private CraftingRecipe currentActiveRecipe;

    /// <summary>
    /// Scans the crafting grid and updates the output slot if there is a match.
    /// </summary>
    public void CheckForRecipes()
    {
        // Get the logical slots of the grid through the GridManager
        var gridSlots = FindFirstObjectByType<CraftingGridManager>().GetGridSlots();
        List<RequiredItem> currentInput = new List<RequiredItem>();

        // Collect what is currently on the crafting table
        foreach (SlotUI slotUI in gridSlots)
        {
            if (slotUI.assignedSlot.item != null)
            {
                currentInput.Add(new RequiredItem
                {
                    itemData = slotUI.assignedSlot.item,
                    amount = slotUI.assignedSlot.stackSize
                });
            }
        }

        // Check if the input matches any recipe in the database
        currentActiveRecipe = FindMatchingRecipe(currentInput);

        // Find the output slot (OutputSlot)
        // NOTE: It is more efficient to drag this reference in the Inspector if possible
        GameObject outputObj = GameObject.Find("OutputSlot");
        if (outputObj != null)
        {
            SlotUI outputSlotUI = outputObj.GetComponent<SlotUI>();
            
            if (currentActiveRecipe != null)
            {
                outputSlotUI.assignedSlot.UpdateSlot(currentActiveRecipe.resultItem, currentActiveRecipe.resultAmount);
            }
            else
            {
                outputSlotUI.assignedSlot.ClearSlot();
            }
            
            outputSlotUI.UpdateSlotUI();
        }
    }

    /// <summary>
    /// Subtracts materials from the grid based on the active recipe.
    /// </summary>
    public void ConsumeIngredients(int times = 1)
    {
        if (currentActiveRecipe == null) return;

        var gridSlots = FindFirstObjectByType<CraftingGridManager>().GetGridSlots();

        foreach (var slotUI in gridSlots)
        {
            if (slotUI.assignedSlot.item != null)
            {
                // Check how much the recipe specifically asks for this item
                var required = currentActiveRecipe.requiredItems.Find(i => i.itemData == slotUI.assignedSlot.item);
                
                if (required != null)
                {
                    // Proportional consumption: (amount required * times the result was removed)
                    int totalToConsume = required.amount * times;
                    slotUI.assignedSlot.RemoveStack(totalToConsume); 
                    slotUI.UpdateSlotUI();
                }
            }
        }
        
        // After consuming, the active recipe is cleared and the grid is rescanned
        currentActiveRecipe = null;
        CheckForRecipes();
    }

    public CraftingRecipe FindMatchingRecipe(List<RequiredItem> currentInput)
    {
        foreach (CraftingRecipe recipe in availableRecipes)
        {
            if (recipe.requiredItems.Count != currentInput.Count) continue;

            bool recipeMatches = true;
            foreach (RequiredItem required in recipe.requiredItems)
            {
                RequiredItem inputItem = currentInput.Find(i => i.itemData == required.itemData);

                if (inputItem == null || inputItem.amount < required.amount)
                {
                    recipeMatches = false;
                    break;
                }
            }

            if (recipeMatches) return recipe;
        }
        return null;
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        foreach (RequiredItem required in recipe.requiredItems)
        {
            int totalOwned = CountItem(required.itemData); 
            if (totalOwned < required.amount) return false;
        }
        return true; 
    }

    private int CountItem(ItemData itemToCount)
    {
        int count = 0;
        foreach (InventorySlot slot in inventoryManager.inventory)
        {
            if (slot.item == itemToCount) count += slot.stackSize;
        }
        return count;
    }
}