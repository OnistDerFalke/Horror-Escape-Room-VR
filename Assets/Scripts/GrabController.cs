using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    [SerializeField]
    private Camera characterCamera;
   
    [SerializeField]
    private Transform slot;
    private PickableItem _pickedItem;
    
    private void Update()
    {
        if (!Input.GetButtonDown("Fire1")) return;
        
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
        item.Rb.AddForce(tRef.forward * 2, ForceMode.VelocityChange);
    }
}