using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemSlot
{
    public Item item;
    public int count;

    public void Copy(ItemSlot itemSlot)
    {
        item = itemSlot.item;
        count = itemSlot.count;
    }

    public void Set(Item item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public void Clear()
    {
        item = null;
        count = 0;
    }
}

[CreateAssetMenu(menuName ="Data/Item Container")]
public class ItemContainer : ScriptableObject
{
    public List<ItemSlot> slots;

    public void Add(Item item, int count = 1)
    {
        if (item.stackable == true)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == item);    // find the existent item and increase the size
            if (itemSlot != null)
            {
                itemSlot.count += count;
            }
            else
            {
                itemSlot = slots.Find(x => x.item == null);
                if (itemSlot != null)
                {
                    itemSlot.item = item;
                    itemSlot.count = count;
                }
            }
        }
        else
        {
            ItemSlot itemSlot = slots.Find(x => x.item == null);     // find the first free slot and add the item if possible
            if (itemSlot != null)
            {
                itemSlot.item = item;
            }
        }
    }

}
