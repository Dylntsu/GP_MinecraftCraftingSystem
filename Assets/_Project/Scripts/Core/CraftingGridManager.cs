using UnityEngine;
using System.Collections.Generic;

public class CraftingGridManager : MonoBehaviour
{
    [Header("Input Slots")]
    public List<SlotUI> inputSlots = new List<SlotUI>(); 

    [Header("Output Slot")]
    public SlotUI outputSlot;

    [Header("Dependencies")]
    public CraftingManager craftingManager; 

    private void Start()
    {
        InitializeCraftingSlots();
        // Listeners Soon...
    }

    // Initialize slots
    private void InitializeCraftingSlots()
    {
        // make sure each SlotUI has an InventorySlot associated to avoid NREs
        foreach (SlotUI slotUI in inputSlots)
        {
            slotUI.assignedSlot = new InventorySlot(null, 0); 
            slotUI.UpdateSlotUI(); 
        }

        outputSlot.assignedSlot = new InventorySlot(null, 0);
        outputSlot.UpdateSlotUI();
    }

   // calls this function every time the items in the 2x2 grid change
    public void UpdateCraftingResult()
    {
        //get current input items
        List<RequiredItem> currentInput = GetInputItems();

        //find matching recipe
        CraftingRecipe foundRecipe = craftingManager.FindMatchingRecipe(currentInput);

        if (foundRecipe != null)
        {
            //if recipe is found and items meet total amount (logic of CanCraft)
            if (craftingManager.CanCraft(foundRecipe))
            {
                //update output slot with recipe result
                outputSlot.assignedSlot.UpdateSlot(foundRecipe.resultItem, foundRecipe.resultAmount);
                outputSlot.UpdateSlotUI();
                Debug.Log($"Recipe found: {foundRecipe.resultItem.displayName} x{foundRecipe.resultAmount}");
                return;
            }
        }

        //if no recipe is found or requirements are not met: clear output slot
        outputSlot.assignedSlot.ClearSlot();
        outputSlot.UpdateSlotUI();
        Debug.Log("No valid recipe found.");
    }


    //auxiliary function: collect all items from the 3x3 grid
    private List<RequiredItem> GetInputItems()
    {
        List<RequiredItem> input = new List<RequiredItem>();

        foreach (SlotUI slotUI in inputSlots)
        {
            InventorySlot slot = slotUI.assignedSlot;

            // only add items that actually exist in the slot
            if (slot.item != null && slot.stackSize > 0)
            {
                // create a temporary RequiredItem object to verify the recipe
                RequiredItem item = new RequiredItem
                {
                    itemData = slot.item,
                    amount = slot.stackSize
                };
                input.Add(item);
            }
        }
        return input;
    }

    /// <summary>
    /// Returns the list of UI slots in the crafting grid.
    /// Used by CraftingManager to check recipes and consume ingredients.
    /// </summary>
    public List<SlotUI> GetGridSlots()
    {
        return inputSlots;
    }
}