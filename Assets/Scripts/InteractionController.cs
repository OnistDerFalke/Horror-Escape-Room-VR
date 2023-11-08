using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Camera characterCamera;
    [SerializeField] private Transform slot;
    [SerializeField] private float throwItemSpeed;
    [SerializeField] private float dropItemSpeed;

    [SerializeField] private Image crosshair;
    [SerializeField] private Sprite normalCrosshair;
    [SerializeField] private Sprite focusedCrosshair;

    [SerializeField] private Transform pickableSlotAxe;
    [SerializeField] private Transform pickableSlotKnife;
    [SerializeField] private Transform pickableSlotPistol;
    [SerializeField] private Transform pickableSlotPills;

    [SerializeField] private GameObject pickableAxe;
    [SerializeField] private GameObject pickableKnife;
    [SerializeField] private GameObject pickablePistol;
    [SerializeField] private GameObject pickablePills;
    
    [SerializeField] private GameObject placedAxe;
    [SerializeField] private GameObject placedKnife;
    [SerializeField] private GameObject placedPistol;
    [SerializeField] private GameObject placedPills;
    
    
    private PickableItem _pickedItem;
    
    private void Update()
    {
        CheckCrosshairState();
        if (Input.GetButtonDown("Fire1"))
        {
            if (_pickedItem)
                DropItem(_pickedItem);
            else
            {
                var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);

                if (!Physics.Raycast(ray, out var hit, 2f)) return;

                if (hit.collider.CompareTag("Pickable"))
                {
                    var pickable = hit.transform.GetComponent<PickableItem>();
                    if (pickable)
                        PickItem(pickable);
                }
                else if (hit.collider.CompareTag("HiddenButton"))
                {
                    var hiddenButton = hit.transform.GetComponent<HiddenButton>();
                    if(hiddenButton) 
                        hiddenButton.Use();
                }
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            if (_pickedItem)
            {
                var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
                var hits = Physics.RaycastAll(ray);
                if (hits.Length > 0)
                {
                    var slotFound = false;
                    foreach (var h in hits)
                    {
                        if (h.collider.CompareTag("PickableSlot"))
                        {
                            slotFound = true;
                            if (h.collider.GetComponent<PickableSlot>().type == _pickedItem.type)
                            {
                                _pickedItem.gameObject.SetActive(false);
                                switch (_pickedItem.type)
                                {
                                    case PickableItem.PickableType.AXE:
                                        placedAxe.gameObject.SetActive(true);
                                        break;
                                    case PickableItem.PickableType.KNIFE:
                                        placedKnife.gameObject.SetActive(true);
                                        break;
                                    case PickableItem.PickableType.PISTOL:
                                        placedPistol.gameObject.SetActive(true);
                                        break;
                                    case PickableItem.PickableType.PILLS:
                                        placedPills.gameObject.SetActive(true);
                                        break;
                                }

                                Transform tRef;
                                (tRef = _pickedItem.transform).SetParent(null);
                                _pickedItem.Rb.isKinematic = false;
                                _pickedItem = null;
                                break;
                            }
                        }
                    }
                    if(!slotFound) ThrowItem(_pickedItem);
                }
                else ThrowItem(_pickedItem);
            }
        }
    }

    private void CheckCrosshairState()
    {
        var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
        if (!Physics.Raycast(ray, out var hit, 3f)) return;
        var hits = Physics.RaycastAll(ray);
        
        //if can be picked
        if ((hit.collider.CompareTag("Pickable") || hit.collider.CompareTag("HiddenButton")) && !_pickedItem)
            crosshair.sprite = focusedCrosshair;
        else crosshair.sprite = normalCrosshair;
        
        //if can be put
        if (hits.Length > 0)
        {
            foreach (var h in hits)
            {
                if(h.collider.CompareTag("PickableSlot") && _pickedItem)
                    crosshair.sprite = focusedCrosshair;
            }
        }

    }
   
    private void PickItem(PickableItem item)
    {
        _pickedItem = item;
        item.Rb.isKinematic = true;
        item.Rb.velocity = Vector3.zero;
        item.Rb.angularVelocity = Vector3.zero;
        Transform tRef;
        (tRef = item.transform).SetParent(slot);
        tRef.localPosition = Vector3.zero;
        tRef.localEulerAngles = Vector3.zero;
    }
   
    private void DropItem(PickableItem item)
    {
        _pickedItem = null;
        Transform tRef;
        (tRef = item.transform).SetParent(null);
        item.Rb.isKinematic = false;
        item.Rb.AddForce(characterCamera.transform.forward * dropItemSpeed, ForceMode.VelocityChange);
    }
    
    private void ThrowItem(PickableItem item)
    {
        _pickedItem = null;
        Transform tRef;
        (tRef = item.transform).SetParent(null);
        item.Rb.isKinematic = false;
        item.Rb.AddForce(characterCamera.transform.forward * throwItemSpeed, ForceMode.Impulse);
    }
}