using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrabController : MonoBehaviour
{
    [SerializeField] private Camera characterCamera;
    [SerializeField] private Transform slot;
    [SerializeField] private float throwItemSpeed;
    [SerializeField] private float dropItemSpeed;

    [SerializeField] private Image crosshair;
    [SerializeField] private Sprite normalCrosshair;
    [SerializeField] private Sprite focusedCrosshair;
    
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

                var pickable = hit.transform.GetComponent<PickableItem>();
                if (pickable)
                    PickItem(pickable);
            }
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            if (_pickedItem)
                ThrowItem(_pickedItem);
        }
    }

    private void CheckCrosshairState()
    {
        var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
        if (!Physics.Raycast(ray, out var hit, 3f)) return;
        if (hit.collider.CompareTag("Pickable") && !_pickedItem)
            crosshair.sprite = focusedCrosshair;
        else crosshair.sprite = normalCrosshair;
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