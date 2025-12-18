using UnityEngine;
using UnityEngine.EventSystems;

public class SlotInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IDropHandler
{
    // PROTECTED: So the child script (OutputSlotInteraction) can access 'ui'
    protected SlotUI ui;
    protected static float lastPickUpTime; 

    private void Awake() => ui = GetComponent<SlotUI>();

    // VIRTUAL: Allows the child to decide whether to allow the click or not
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (ui.assignedSlot == null || ui.assignedSlot.item == null) return;

        // Default behavior: Only pick up if the hand is empty
        if (!DragManager.Instance.currentDragData.IsHoldingItem)
        {
            HandlePickUp(eventData);
            lastPickUpTime = Time.time; 
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (DragManager.Instance.currentDragData.IsHoldingItem && Time.time > lastPickUpTime + 0.1f)
        {
            if (!eventData.dragging) HandleDrop();
        }
    }

    public void OnBeginDrag(PointerEventData eventData) { }
    public void OnDrag(PointerEventData eventData) { }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if (DragManager.Instance.currentDragData.IsHoldingItem)
        {
            HandleDrop();
        }
    }
    
    // VIRTUAL: This is where the inheritance magic happens
    protected virtual void HandlePickUp(PointerEventData eventData)
    {
        // --- STANDARD LOGIC (Inventory and Grid Only) ---
        // There is no longer "if (isOutputSlot)", this is clean code.
        var slot = ui.assignedSlot;

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

        ui.UpdateSlotUI();
        NotifyCrafting();
    }

    protected virtual void HandleDrop()
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
            ItemData tempItem = slot.item;
            int tempAmount = slot.stackSize;
            slot.UpdateSlot(drag.item, drag.amount);
            DragManager.Instance.StartDrag(tempItem, tempAmount, drag.originalSlot);
        }
        ui.UpdateSlotUI();
        NotifyCrafting();
    }

    protected void NotifyCrafting()
    {
        var cm = Object.FindFirstObjectByType<CraftingManager>();
        if (cm != null) cm.CheckForRecipes();
    }
}