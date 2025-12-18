using UnityEngine;
using UnityEngine.EventSystems;

public class OutputSlotInteraction : SlotInteraction
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (ui.assignedSlot == null || ui.assignedSlot.item == null) return;

        bool isHandEmpty = !DragManager.Instance.currentDragData.IsHoldingItem;
        bool isSameItem = DragManager.Instance.currentDragData.item == ui.assignedSlot.item;

        // Allow click if hand is empty OR if we are carrying the same item (stacking)
        if (isHandEmpty || isSameItem)
        {
            HandlePickUp(eventData);
            lastPickUpTime = Time.time;
        }
    }

    public override void OnDrop(PointerEventData eventData) { return; }
    public override void OnPointerUp(PointerEventData eventData) { return; }

   // Implements the consumption and stacking logic
    protected override void HandlePickUp(PointerEventData eventData)
    {
        var slot = ui.assignedSlot;
        var cm = Object.FindFirstObjectByType<CraftingManager>();
        var dragData = DragManager.Instance.currentDragData;

        // CASE A: Empty Hand -> Pick up new
        if (!dragData.IsHoldingItem)
        {
            DragManager.Instance.StartDrag(slot.item, slot.stackSize, ui);
            if (cm != null) cm.ConsumeIngredients(1);
            slot.ClearSlot();
        }
        // CASE B: Same Item -> Add to stack
        else if (dragData.item == slot.item)
        {
            if (slot.item.isStackable)
            {
                DragManager.Instance.AddDragStack(slot.stackSize);
                if (cm != null) cm.ConsumeIngredients(1);
                slot.ClearSlot();
            }
        }
        
        ui.UpdateSlotUI();
        NotifyCrafting();
    }
}