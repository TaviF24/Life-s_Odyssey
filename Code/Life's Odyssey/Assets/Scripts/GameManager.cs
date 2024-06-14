using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*--------Singleton--------*/

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject player;
    public ItemContainer inventoryContainer;
    public ItemDragAndDropController dragAndDropController;
    public DayTimeController timeController;
    public DialogueSystem dialogueSystem;
    public PlaceableObjectsReferenceManager placeableObjects;
}
