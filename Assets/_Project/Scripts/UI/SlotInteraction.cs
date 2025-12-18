using UnityEngine;
using UnityEngine.EventSystems;

public class SlotInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IDropHandler
{
    private SlotUI ui;
    private static float lastPickUpTime; 

    [Header("Settings")]
    public bool isOutputSlot = false;

    // Initializes the SlotUI reference
    private void Awake() => ui = GetComponent<SlotUI>();

    // Handles the start of an interaction (click or drag start) when the slot has an item
    public void OnPointerDown(PointerEventData eventData)
    {
        if (ui.assignedSlot == null || ui.assignedSlot.item == null) return;

        bool isHandEmpty = !DragManager.Instance.currentDragData.IsHoldingItem;
        bool isSameItem = DragManager.Instance.currentDragData.item == ui.assignedSlot.item;

        if (isHandEmpty || (isOutputSlot && isSameItem))
        {
            HandlePickUp(eventData);
            lastPickUpTime = Time.time; 
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // If it is an output slot, we block any attempt to drop items
        if (isOutputSlot) return;

        if (DragManager.Instance.currentDragData.IsHoldingItem && Time.time > lastPickUpTime + 0.1f)
        {
            // Only drop if not currently dragging to avoid double execution with OnDrop
            if (!eventData.dragging) 
            {
                HandleDrop();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData) { }
    public void OnDrag(PointerEventData eventData) { }

    public void OnDrop(PointerEventData eventData)
    {
        // If it is an output slot, we block the drop
        if (isOutputSlot) return;

        if (DragManager.Instance.currentDragData.IsHoldingItem)
        {
            Debug.Log("Dropped via Drag (OnDrop)");
            HandleDrop();
        }
    }
    
    // Handles logic for picking up items (Left click: all, Right click: half)
    private void HandlePickUp(PointerEventData eventData)
    {
        var slot = ui.assignedSlot;
        var cm = Object.FindFirstObjectByType<CraftingManager>();
        var dragData = DragManager.Instance.currentDragData;

        if (isOutputSlot)
        {
            // --- STACKING LOGIC IN OUTPUT ---
            
            // CASE A: Empty Hand -> Start dragging
            if (!dragData.IsHoldingItem)
            {
                DragManager.Instance.StartDrag(slot.item, slot.stackSize, ui);
                if (cm != null) cm.ConsumeIngredients(1);
                slot.ClearSlot(); // Visually clear (the manager will refill if materials remain)
            }
            // CASE B: Hand with Same Item -> Add to stack (Stacking)
            else if (dragData.item == slot.item)
            {
                // Check if stackable (optional, but good practice)
                if (slot.item.isStackable)
                {
                    // Add the result amount to what we already have in hand
                    DragManager.Instance.AddDragStack(slot.stackSize);
                    
                    // Consume ingredients again
                    if (cm != null) cm.ConsumeIngredients(1);
                    
                    slot.ClearSlot(); 
                }
            }
        }
        else // logic for inventory slots
        {
            if (DragManager.Instance.currentDragData.IsHoldingItem) return;

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                DragManager.Instance.StartDrag(slot.item, slot.stackSize, ui);
                slot.ClearSlot();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                int amount = Mathf.CeilToInt(slot.stackSize / 2f);
                DragManager.Instance.StartDrag(slot.item, amount, ui);
                slot.stackSize -= amount;
                if (slot.stackSize <= 0) slot.ClearSlot();
            }
        }

        ui.UpdateSlotUI();
        NotifyCrafting();
    }
    // Handles logic for dropping items (swap or stack)
    private void HandleDrop()
    {
        var drag = DragManager.Instance.currentDragData;
        var slot = ui.assignedSlot;

        if (slot.item == null || (slot.item == drag.item && slot.item.isStackable))
        {
            if (slot.item == null) slot.UpdateSlot(drag.item, drag.amount);
            else slot.stackSize += drag.amount;
            DragManager.Instance.EndDrag();
        }
        else
        {
            // --- NORMAL LOGIC (INVENTORY/GRID) ---
            ItemData tempItem = slot.item;
            int tempAmount = slot.stackSize;
            slot.UpdateSlot(drag.item, drag.amount);
            DragManager.Instance.StartDrag(tempItem, tempAmount, drag.originalSlot);
        }
        ui.UpdateSlotUI();
        NotifyCrafting();
    }

    // Notifies the CraftingManager to check for recipes after an inventory change
    private void NotifyCrafting()
    {
        var cm = Object.FindFirstObjectByType<CraftingManager>();
        if (cm != null) cm.CheckForRecipes();
    }
}