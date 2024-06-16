using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainerInteractController : MonoBehaviour
{
    ItemContainer targetItemContainer;
    InventoryController inventoryController;
    [SerializeField] ItemContainerPanel itemContainerPanel;
    Transform openedChest; // reference to currently opened chest
    [SerializeField] float maxDistance = 2.5f; // max interact distance from chest

	private void Awake()
	{
		inventoryController = GetComponent<InventoryController>();
	}

	public void Open(ItemContainer itemContainer, Transform _openedChest)
    {
        targetItemContainer = itemContainer; // target item container
        itemContainerPanel.inventory = targetItemContainer; // inventory container panel
        inventoryController.Open(); // open inventory UI
        itemContainerPanel.gameObject.SetActive(true); // show on screen
        openedChest = _openedChest; // mark chest as opened
    }

	private void Update()
	{
		if (openedChest != null)
		{
			// distance between player nad chest
			float distance = Vector2.Distance(openedChest.position, transform.position);
			if (distance > maxDistance)
			{
				openedChest.GetComponent<LootContainerInteract>().Close(GetComponent<Character>());
			}
		}
	}

	public void Close()
    {
		// close the inventory UI and inventory panel
		inventoryController.Close();
		itemContainerPanel.gameObject.SetActive(false);
		openedChest = null;
	}
}
