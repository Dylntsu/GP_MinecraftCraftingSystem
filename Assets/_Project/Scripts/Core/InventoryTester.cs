using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [Header("References")]
    // Reference to your Manager
    public InventoryManager manager; 

    [Header("Test Data")]
    public ItemData itemToAdd; 
    public int amount = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (manager != null && itemToAdd != null)
            {
                bool success = manager.AddItem(itemToAdd, amount);
                
                // Shows the result in the Console
                if (success)
                {
                    Debug.Log("Success! Added " + amount + " of " + itemToAdd.displayName);
                }
                else
                {
                    Debug.Log("Failed: Inventory full or stacking error.");
                }
            }
            else
            {
                Debug.LogError("Manager or Item not assigned in the Inspector.");
            }
        }
    }
}