using System;

[Serializable] 
public class InventorySlot
{
    public ItemData item; // Object inside the Slot
    public int stackSize; // Stack size

    public InventorySlot(ItemData item, int amount) // Slot constructor 
    {
        this.item = item;
        this.stackSize = amount;
    }

    public void AddStack(int amount) // Add to stack
    {
        stackSize += amount;
    }

    public void RemoveStack(int amount) // Remove from stack
    {
        stackSize -= amount;
        if (stackSize <= 0) Clear();
    }

    public void Clear() // Clear
    {
        item = null;
        stackSize = 0;
    }
}