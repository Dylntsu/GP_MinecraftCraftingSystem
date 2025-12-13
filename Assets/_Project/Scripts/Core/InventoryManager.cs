using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int slotNumber = 36;
    public InventorySlot[] inventory;

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
        if (item.isStackable)
        {
            foreach (var slot in inventory)
            {
                // Checks if the item matches & if there is space available
                if (slot.item == item && slot.stackSize < item.maxStackSize)
                {
                    slot.AddStack(amount);
                    return true; 
                }
            }
        }

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].item == null)
            {
                inventory[i].item = item;
                inventory[i].stackSize = amount;
                return true; 
            }
        }

        Debug.Log("Full inventory");
        return false;

    } 
}