using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractController : MonoBehaviour
{
    CharacterController2D characterController;
    Rigidbody2D rigidbody2D;
    [SerializeField] float offsetDistance = 1f; // distance from the character to check for interactble objects
    [SerializeField] float sizeOfInteractableArea = 1.2f; // size of the area to check for interactble objects
    Character character; // main character
    [SerializeReference] HighlightController highlightController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
    }

    private void Update()
    {
        Check(); // check for interactble objects
        if (Input.GetMouseButtonDown(1)) // if right mouse button, interact with object
        {
            Interact();
        }
    }

    private void Check()
    {
        // position of interactable object
        Vector2 position = rigidbody2D.position + characterController.lastMotionVector * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);
        
        foreach (Collider2D collider in colliders)
        {
            Interactable toolHit = collider.GetComponent<Interactable>();
            if (toolHit != null)
            {
                highlightController.Highlight(toolHit.gameObject); // highlight the interactable object if found
                return;
            }
        }

        highlightController.Hide(); // hide the highlight if the interactble object is not found anymore
        
    }

    private void Interact()
    {
        // position of interactable object
        Vector2 position = rigidbody2D.position + characterController.lastMotionVector * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D collider in colliders)
        {
            Interactable toolHit = collider.GetComponent<Interactable>();
            if (toolHit != null) // check if collider has interactable component
            {
                toolHit.Interact(character); // interact with the clicked object if found
                break;
            }
        }
    }
}
