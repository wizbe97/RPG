using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public Text countText;
    [HideInInspector] public Item item;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    bool isDragging = false; // Track if dragging is occurring
    private InventoryManager inventoryManager;



    private void Start()
    {
        inventoryManager = FindObjectOfType<InventoryManager>(); // Find the InventoryManager in the scene
    }
    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDragging && (Mouse.current.leftButton.isPressed || (Mouse.current.leftButton.isPressed && Mouse.current.rightButton.isPressed)))
        {
            isDragging = true; // Start dragging if left button is pressed or both buttons are pressed
            image.raycastTarget = false;
            // Parenting logic adjusted here
            parentAfterDrag = transform.parent; // Store the current parent
            // No need to change the parent here, leave it as it is
            // transform.SetParent(transform.root);
            countText.raycastTarget = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }

    public void OnEndDrag(PointerEventData Data)
    {
        if (isDragging)
        {
            isDragging = false;
            image.raycastTarget = true;
            InventoryItem targetItem = Data.pointerEnter ? Data.pointerEnter.GetComponent<InventoryItem>() : null;
            InventorySlot targetSlot = Data.pointerEnter ? Data.pointerEnter.GetComponent<InventorySlot>() : null;

            if (targetItem != null && item == targetItem.item && item.stackable)
            {
                if (targetSlot != null)
                {
                    // Snap the dragged item onto the target item slot
                    transform.SetParent(targetItem.transform.parent);
                    transform.position = targetItem.transform.position;
                    transform.SetAsLastSibling(); // Ensure the dragged item is rendered on top

                    // Merge the dragged item with the target item
                    targetItem.MergeWith(this);
                }
                else
                {
                    // Put the dragged item back to its original slot
                    transform.SetParent(parentAfterDrag);
                    transform.localPosition = Vector3.zero;
                }
            }
            else
            {
                if (targetSlot != null)
                {
                    // Put the dragged item into the target inventory slot
                    transform.SetParent(targetSlot.transform);
                    transform.localPosition = Vector3.zero;
                }
                else
                {
                    // Put the dragged item back to its original slot
                    transform.SetParent(parentAfterDrag);
                    transform.localPosition = Vector3.zero;
                }
            }

            countText.raycastTarget = true;
        }
    }

    public void MergeWith(InventoryItem otherItem)
    {
        int totalItemsCount = otherItem.count + count;
        if (totalItemsCount <= inventoryManager.maxStackedItems)
        {
            // If the total count of items in the stack doesn't exceed the maximum stack size,
            // merge the stacks completely
            otherItem.count = totalItemsCount;
            otherItem.RefreshCount();
            Destroy(gameObject); // Destroy the dragged item
        }
        else
        {
            // If the target slot can't accommodate the entire stack, partially merge them
            int spaceLeftInStack = inventoryManager.maxStackedItems - otherItem.count;
            otherItem.count += spaceLeftInStack;
            otherItem.RefreshCount();
            count -= spaceLeftInStack;
            RefreshCount();
        }
    }
}