using UnityEngine;

public class DragData
{
    public ItemData item;
    public int amount;
    public SlotUI originalSlot; 
    
    public bool IsHoldingItem => item != null && amount > 0;

    public void Clear()
    {
        item = null;
        amount = 0;
        originalSlot = null;
    }
}

public class DragManager : MonoBehaviour
{
    public static DragManager Instance { get; private set; }

    [Header("Dependencies")]
    public DragVisual dragVisualPrefab; 

    public DragData currentDragData { get; private set; } = new DragData();
    private DragVisual visualInstance; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            if (dragVisualPrefab != null)
            {
                visualInstance = Instantiate(dragVisualPrefab, transform); 
                visualInstance.HideVisual();
            }
        }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        // The visual follows the mouse every frame
        if (currentDragData.IsHoldingItem && visualInstance != null)
        {
            visualInstance.transform.position = Input.mousePosition;
        }
    }

    public void StartDrag(ItemData item, int amount, SlotUI originalSlot)
    {
        currentDragData.item = item;
        currentDragData.amount = amount;
        currentDragData.originalSlot = originalSlot;

        if (visualInstance != null)
        {
            visualInstance.SetVisualData(item.icon, amount);
            // FIX: Instant positioning on click
            visualInstance.transform.position = Input.mousePosition;
        }
    }
    
    public void AddDragStack(int amountToAdd)
    {
        if (currentDragData.IsHoldingItem)
        {
            // 1. Add the logical amount
            currentDragData.amount += amountToAdd;

            // 2. Update the visual (the number on the floating icon)
            if (visualInstance != null)
            {
                visualInstance.SetVisualData(currentDragData.item.icon, currentDragData.amount);
            }
        }
    }

    public void EndDrag()
    {
        currentDragData.Clear();
        if (visualInstance != null) visualInstance.HideVisual();
    }
}