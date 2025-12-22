using UnityEngine;
using UnityEngine.EventSystems;

public class SlotInteraction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IDropHandler, IPointerEnterHandler
{
    protected SlotUI ui;
    protected static float lastPickUpTime; 

    private void Awake() => ui = GetComponent<SlotUI>();

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        // --- SHIFT + CLICK ---
        if (HotbarUI.Instance != null)
        {
            HotbarUI.Instance.SetHighlightPosition(ui.GetComponent<RectTransform>());
        }

        // check shift key
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            HandleShiftClick();
            return;
        }

        // if there is no item, we leave
        if (ui.assignedSlot == null || ui.assignedSlot.item == null) return;
        if (!DragManager.Instance.currentDragData.IsHoldingItem)
        {
            HandlePickUp(eventData);
            lastPickUpTime = Time.time; 
        }
    }

    // DRAG PAINTING (IPointerEnterHandler)
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        // If the left mouse button is pressed and we have something in our hand
        if (Input.GetMouseButton(0) && DragManager.Instance.currentDragData.IsHoldingItem)
        {
            HandleDragPainting();
        }
    }

    protected virtual void HandleDragPainting()
    {
        var drag = DragManager.Instance.currentDragData;
        var slot = ui.assignedSlot;

        // only paint if the slot is empty or has the same item and there is space
        if (slot.item == null || (slot.item == drag.item && slot.stackSize < slot.item.maxStackSize))
        {
            if (drag.amount > 0)
            {
                if (slot.item == null) slot.UpdateSlot(drag.item, 1);
                else slot.stackSize += 1;

                drag.amount -= 1;

                // If we run out of items in the hand, end the drag
                if (drag.amount <= 0) DragManager.Instance.EndDrag();
                else DragManager.Instance.UpdateDragVisual(); // Update the number in the cursor

                ui.UpdateSlotUI();
                NotifyCrafting();
            }
        }
    }

        protected virtual void HandleShiftClick()
    {
        var slot = ui.assignedSlot;
        if (slot == null || slot.item == null) return;

        var invManager = Object.FindFirstObjectByType<InventoryManager>();
        var gridManager = Object.FindFirstObjectByType<CraftingGridManager>();

        // if there is no crafting table visible, do nothing
        bool isCraftingActive = gridManager != null && gridManager.gameObject.activeInHierarchy;
        if (!isCraftingActive) return;

        bool isInCraftingGrid = gridManager.GetGridSlots().Contains(ui);

        if (isInCraftingGrid)
        {
            // CASE A: From Crafting Table -> Inventory
            // AddItem generally returns the remainder, it is assumed that if it returns true, everything fit.
            if (invManager.AddItem(slot.item, slot.stackSize))
            {
                slot.ClearSlot();
            }
        }
        else
        {
            // CASE B: From Inventory -> Crafting Table
            foreach (var gridSlotUI in gridManager.GetGridSlots())
            {
                var gridSlot = gridSlotUI.assignedSlot;
                
                // Check if the slot is empty or has the same item and there is space
                if (gridSlot.item == null || (gridSlot.item == slot.item && gridSlot.stackSize < gridSlot.item.maxStackSize))
                {
                    // Calculate how much we can move really
                    int maxStack = slot.item.maxStackSize;
                    int currentStack = gridSlot.item != null ? gridSlot.stackSize : 0;
                    int spaceLeft = maxStack - currentStack;
                    
                    int amountToMove = Mathf.Min(slot.stackSize, spaceLeft);

                    if (amountToMove > 0)
                    {
                        // Add to destination
                        if (gridSlot.item == null) gridSlot.UpdateSlot(slot.item, amountToMove);
                        else gridSlot.AddStack(amountToMove);

                        // Remove from origin
                        slot.RemoveStack(amountToMove); // Use RemoveStack instead of ClearSlot for safety

                        // Update the destination slot UI
                        gridSlotUI.UpdateSlotUI(); 
                        
                        // If we empty the origin slot, break the loop
                        if (slot.stackSize <= 0)
                        {
                            slot.ClearSlot();
                            break;
                        }
                    }
                }
            }
        }

        // Update the slot UI where we clicked
        ui.UpdateSlotUI();
        NotifyCrafting();
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
    
    protected virtual void HandlePickUp(PointerEventData eventData)
    {
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