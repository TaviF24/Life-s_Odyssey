using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [SerializeField] ItemContainer inventory; // player's inventory
    public void Craft (CraftingRecipe recipe)
    {
        // check for enough space in inventory
        if(inventory.FreeSpace()==false)
        {
            Debug.Log("Not enough space to fit the item after crafting");
            return;
        }

        // check if required items and present in inventory for the current recipe
        for(int i=0;i< recipe.elements.Count;i++)
        {
            if(inventory.CheckItem(recipe.elements[i])==false){
                Debug.Log("Crafting recipe elements are not present in the inventory");
                return;
            }
        }

        // remove the required items for the recipe from inventory
        for(int i=0;i< recipe.elements.Count;i++)
        {
            inventory.Remove(recipe.elements[i].item,recipe.elements[i].count);
        }

        // add the crafted item to the inventory
        inventory.Add(recipe.output.item,recipe.output.count);
        }
}
