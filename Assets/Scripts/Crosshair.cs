using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    //Player components
    [SerializeField] private Camera playerCamera;
    [SerializeField] private InteractionController interactionController;
    
    //Crosshair UI element and sprites which are applied to it
    [SerializeField] private Image crosshair;
    [SerializeField] private Sprite basicSprite;
    [SerializeField] private Sprite interactionSprite;

    private void Update()
    {
        UpdateState();
    }
    
    private void UpdateState()
    {
        //Raycast checking if any interactable object is in radius
        var ray = playerCamera.ViewportPointToRay(Vector3.one * 0.5f);
        if (!Physics.Raycast(ray, out var hit, GameManager.interactionDistance)) return;
        var hits = Physics.RaycastAll(ray);
        
        //Show interaction crosshair when looking at interactable object
        foreach (var t in interactionController.InteractionTags)
        {
            //TODO: Some of interactions should be working even if item in hands
            if (hit.collider.CompareTag(t) && !GameManager.itemInHands)
            {
                crosshair.sprite = interactionSprite;
                break;
            }
            crosshair.sprite = basicSprite;
        }

        //Show interaction crosshair when can put item on slot
        if (hits.Length <= 0) return;
        foreach (var h in hits)
        {
            if (h.collider.CompareTag("PickableSlot") && GameManager.itemInHands)
                crosshair.sprite = interactionSprite;
        }
    }
}
