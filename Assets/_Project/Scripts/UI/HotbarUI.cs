using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    // Singleton to be able to call it from any slot without references
    public static HotbarUI Instance;

    [Header("Visual References")]
    public RectTransform highlighterRT; 

    private void Awake()
    {
        // Basic Singleton configuration
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        // Ensure the frame doesn't block clicks
        if (highlighterRT.GetComponent<Image>() != null)
            highlighterRT.GetComponent<Image>().raycastTarget = false;
    }

    public void SetHighlightPosition(RectTransform targetSlotTransform)
    {
        if (highlighterRT != null && targetSlotTransform != null)
        {
            highlighterRT.position = targetSlotTransform.position;
        }
    }
}