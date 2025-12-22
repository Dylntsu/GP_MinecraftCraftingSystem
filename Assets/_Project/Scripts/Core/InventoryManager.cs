using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int slotNumber = 36;
    public InventorySlot[] inventory;
    
    public InventoryUI inventoryUI;
    
    private void Awake()
    {
        inventory = new InventorySlot[slotNumber];
        for (int i = 0; i < slotNumber; i++)
        {

            inventory[i] = new InventorySlot(null, 0); 
        }
    }

public bool AddItem(ItemData item, int amount)
    {
        // 1. FIRST PHASE: Look for existing stacks (Only if stackable)
        if (item.isStackable)
        {
            // Use inventory instead of inventorySlots
            foreach (var slot in inventory)
            {
                // If we find the same item and it has space
                if (slot.item == item && slot.stackSize < item.maxStackSize)
                {
                    // Calculate how much free space remains
                    int spaceLeft = item.maxStackSize - slot.stackSize;
                    
                    // Take what fits
                    int amountToAdd = Mathf.Min(amount, spaceLeft);

                    slot.AddStack(amountToAdd);
                    amount -= amountToAdd;
                }

                // If we already distributed everything, break the loop
                if (amount <= 0) break;
            }
        }

        // 2. SECOND PHASE: If items still remain, look for empty slots
        if (amount > 0)
        {
            // Use inventory.Length
            for (int i = 0; i < inventory.Length; i++)
            {
                if (inventory[i].item == null)
                {
                    int amountToAdd = Mathf.Min(amount, item.maxStackSize);

                    inventory[i].item = item;
                    inventory[i].stackSize = amountToAdd;
                    
                    amount -= amountToAdd;
                }

                if (amount <= 0) break;
            }
        }

        // Use UpdateUI which is the method you already had in your script
        if (inventoryUI != null) inventoryUI.UpdateUI();

        // Return true only if we managed to save EVERYTHING (amount reached 0)
        return amount <= 0;
    }
}