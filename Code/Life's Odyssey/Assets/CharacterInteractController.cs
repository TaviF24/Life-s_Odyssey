using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteractController : MonoBehaviour
{
    CharacterController2D characterController;
    Rigidbody2D rigidbody2D;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] float sizeOfInteractableArea = 1.2f;
    Character character;
    [SerializeReference] HighlightController highlightController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
    }

    private void Update()
    {
        Check();
        if (Input.GetMouseButton(1))
        {
            Interact();
        }
    }

    private void Check()
    {
        Vector2 position = rigidbody2D.position + characterController.lastMotionVector * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        

        foreach (Collider2D collider in colliders)
        {
            Interactable toolHit = collider.GetComponent<Interactable>();
            if (toolHit != null)
            {
                highlightController.Highlight(toolHit.gameObject);
                return;
            }
        }

        highlightController.Hide();
        
    }

    private void Interact()
    {
        Vector2 position = rigidbody2D.position + characterController.lastMotionVector * offsetDistance;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, sizeOfInteractableArea);

        foreach (Collider2D collider in colliders)
        {
            Interactable toolHit = collider.GetComponent<Interactable>();
            if (toolHit != null)
            {
                toolHit.Interact(character);
                break;
            }
        }
    }
}
