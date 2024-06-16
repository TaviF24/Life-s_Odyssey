using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemConvertorData
{
    public ItemSlot itemSlot;
    public int timer;

    public ItemConvertorData()
    {
        itemSlot = new ItemSlot();
    }
}


[RequireComponent(typeof(TimeAgent))]
public class ItemConverterInteract : Interactable, IPersistent
{
    [SerializeField] Item convertableItem; // the item that is being converted
    [SerializeField] Item producedItem; // the item that is being produced
    [SerializeField] int producedItemCount = 1; // number of items to produce

    [SerializeField] int timeToProcess = 5; // time required to process the item

    ItemConvertorData data;

    Animator animator;

    private void Start()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += ItemConvertProcess;

        if(data == null)
        {
            data = new ItemConvertorData();
        }
        animator = GetComponent<Animator>();
        Animate();
    }

    private void ItemConvertProcess()
    {
        // start the conversion process if there is an item to process
        if (data.itemSlot == null) { return; }
        if (data.timer > 0)
        {
            data.timer -= 1;
            if (data.timer <= 0)
            {
                CompleteItemConversion();
            }
        }
    }

    public override void Interact(Character character)
    {
        if(data.itemSlot.item == null)
        {
            // check if the item can be converted
            if (GameManager.instance.dragAndDropController.Check(convertableItem))
            {
                StartItemProcessing(GameManager.instance.dragAndDropController.itemSlot);
                return;
            }

            // check the toolbar for the convertible item
            ToolbarController toolbarController = character.GetComponent<ToolbarController>();
            if(toolbarController == null)
            {
                return;
            }

            ItemSlot itemSlot = toolbarController.GetItemSlot;
            // start the item processing if the item matches
            if(itemSlot.item == convertableItem)
            {
                StartItemProcessing(itemSlot);
                return;
            }
        }

        // if the item is done processing and is ready, add it to the inventory
        if(data.itemSlot.item != null && data.timer <= 0)
        {
            GameManager.instance.inventoryContainer.Add(data.itemSlot.item, data.itemSlot.count);
            data.itemSlot.Clear();
        }
    }

    private void StartItemProcessing(ItemSlot toProcess)
    {
        // copy the item to the conversion slot and update the count
        data.itemSlot.Copy(GameManager.instance.dragAndDropController.itemSlot);
        data.itemSlot.count = 1;
        if (toProcess.item.stackable)
        {
            toProcess.count -= 1;
            if(toProcess.count < 0)
            {
                toProcess.Clear();
            }
        }
        else
        {
            toProcess.Clear();
        }

        // set timer and start processing animation
        data.timer = timeToProcess;
        Animate();
    }

    private void Animate()
    {
        animator.SetBool("Working", data.timer > 0f);
    }

    private void CompleteItemConversion()
    {
        Animate();
        data.itemSlot.Clear();
        data.itemSlot.Set(producedItem, producedItemCount);
    }

    public string Read()
    {
        return JsonUtility.ToJson(data);
    }

    public void Load(string jsonString)
    {
        data = JsonUtility.FromJson<ItemConvertorData>(jsonString);
    }
}
