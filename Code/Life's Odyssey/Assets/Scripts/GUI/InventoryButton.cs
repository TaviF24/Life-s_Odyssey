using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image icon;
    [SerializeField] Text text;
    [SerializeField] Image highlight;

    int myIndex;

    public void SetIndex(int index)
    {
        // set index for the inventory buttons inside the panel
        myIndex = index;
    }

    public void Set(ItemSlot slot)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon;

        if (slot.item.stackable == true)
        {
            text.gameObject.SetActive(true); // if item is stackable show the count on inventory screen
            text.text = slot.count.ToString();
        }
        else
        {
            text.gameObject.SetActive(false); // if item is UNstackable do not show count on inventory screen
        }
    }

    public void Clean()
    {
        // clean item from inventory
        icon.sprite = null;
        icon.gameObject.SetActive(false);

        text.gameObject.SetActive(false);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        // handle clicking on item
        ItemPanel itemPanel = transform.parent.GetComponent<ItemPanel>();
        itemPanel.OnClick(myIndex);
    }

    public void Highlight(bool b)
    {
        // if item is selected
        highlight.gameObject.SetActive(b);
    }
}
