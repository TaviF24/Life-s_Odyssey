using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragAndDropController : MonoBehaviour
{
    public ItemSlot itemSlot;
    [SerializeField] GameObject itemIcon;
    RectTransform iconTransform;
    Image itemIconImage;

    internal void RemoveItem(int count = 1)
    {
        // remove a specified number of items from the slot
        if (itemSlot == null) { return; }

        if (itemSlot.item.stackable)
        {
            itemSlot.count -= count; // decrease count for STACKABLE items
            if (itemSlot.count <= 0)
            {
                itemSlot.Clear();
            }
        }
        else
        {
            itemSlot.Clear(); // if unstackable just delete
        }
        UpdateIcon();
    }

    public bool Check(Item item, int count = 1)
    {
        // check the inventory slot for an item and its count
        if (itemSlot == null) { return false; }

        if (item.stackable)
        {
            return itemSlot.item == item && itemSlot.count >= count;
        }
        return itemSlot.item == item;
    }


    internal void OnClick(ItemSlot itemSlot)
    {
        if(this.itemSlot.item == null)
        {
            this.itemSlot.Copy(itemSlot);
            itemSlot.Clear();
        }
        else
        {
            if(itemSlot.item == this.itemSlot.item)
            {
                // combine stackable items
                itemSlot.count += this.itemSlot.count;
                this.itemSlot.Clear();
            }

            // swap items between held slot and clicked slot
            Item item = itemSlot.item;
            int count = itemSlot.count;

            itemSlot.Copy(this.itemSlot);
            this.itemSlot.Set(item, count);
        }
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        // update icon base on current selected item slot
        if(itemSlot.item == null)
        {
            itemIcon.SetActive(false);
        }
        else
        {
            itemIcon.SetActive(true);
            itemIconImage.sprite = itemSlot.item.icon;
        }
    }

    private void Start()
    {
        itemSlot = new ItemSlot();
        iconTransform = itemIcon.GetComponent<RectTransform>();
        itemIconImage = itemIcon.GetComponent<Image>();
    }

    private void Update()
    {
        if(itemIcon.activeInHierarchy == true)
        {
            // get mouse position
            iconTransform.position = Input.mousePosition;

            // on left click
            if (Input.GetMouseButtonDown(0))
            {
                if(EventSystem.current.IsPointerOverGameObject() == false) // if NOT over UI element
                {
                    // get world position
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    worldPosition.z = 0;

                    // spawn(drop) the item in the world
                    ItemSpawnManager.instance.SpawnItem(
                        worldPosition,
                        itemSlot.item,
                        itemSlot.count);

                    // clear the item from inventory(slot)
                    itemSlot.Clear();
                    itemIcon.SetActive(false);
                }
            }
            
        }
    }

}
