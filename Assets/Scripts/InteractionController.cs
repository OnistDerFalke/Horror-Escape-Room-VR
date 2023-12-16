using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private AudioController audioController;
    
    [SerializeField] private Camera characterCamera;
    [SerializeField] private Transform slot;
    [SerializeField] private float throwItemSpeed;
    [SerializeField] private float dropItemSpeed;
    [SerializeField] private Text infoText;

    [SerializeField] private Image crosshair;
    [SerializeField] private Sprite normalCrosshair;
    [SerializeField] private Sprite focusedCrosshair;

    [SerializeField] private GameObject placedAxe;
    [SerializeField] private GameObject placedKnife;
    [SerializeField] private GameObject placedPistol;
    [SerializeField] private GameObject placedPills;

    [SerializeField] private DoorController firstDoor;

    [SerializeField] private DogTagsUI dogTagsUI;
    
    private PickableItem _pickedItem;
    private bool isInfoShown;
    
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
                    if (hiddenButton)
                    {
                        StartCoroutine(ShowInfo("Button activated a mechanism."));
                        hiddenButton.Use();
                    }
                }
                else if (hit.collider.CompareTag("LightBox"))
                {
                    var lightBox = hit.transform.GetComponent<LightBox>();
                    if (lightBox)
                    {
                        StartCoroutine(ShowInfo("Power is back."));
                        lightBox.Use();
                    }
                }
                else if (hit.collider.CompareTag("Pix"))
                {
                    var pix = hit.transform.GetComponent<Pix>();
                    if (pix)
                    {
                        if (pix.isOpened)
                        {
                            pix.TakeDogTags();
                            return;
                        }
                        if (pix.isUnlocked)
                            pix.OpenPix();
                        else pix.UnlockPix();
                    }
                }
                else if (hit.collider.CompareTag("CorpseTag"))
                {
                    var corpseTag = hit.transform.GetComponent<CorpseTags>();
                    dogTagsUI.SetDogTagDown(corpseTag.index);
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
                                        GameManager.ItemsFound++;
                                        ShowItemsFound();
                                        break;
                                    case PickableItem.PickableType.KNIFE:
                                        placedKnife.gameObject.SetActive(true);
                                        GameManager.ItemsFound++;
                                        ShowItemsFound();
                                        break;
                                    case PickableItem.PickableType.PISTOL:
                                        placedPistol.gameObject.SetActive(true);
                                        GameManager.ItemsFound++;
                                        ShowItemsFound();
                                        break;
                                    case PickableItem.PickableType.PILLS:
                                        placedPills.gameObject.SetActive(true);
                                        GameManager.ItemsFound++;
                                        ShowItemsFound();
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
        
        if (hit.collider.CompareTag("Pickable") ||
             hit.collider.CompareTag("HiddenButton") || 
             hit.collider.CompareTag("LightBox") || 
             hit.collider.CompareTag("Pix") ||
             hit.collider.CompareTag("CorpseTag")
             && !_pickedItem)
            crosshair.sprite = focusedCrosshair;
        else crosshair.sprite = normalCrosshair;
        
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
        audioController.PlayPickItemSound();
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

    private void ShowItemsFound()
    {
        if (GameManager.ItemsFound == GameManager.ItemsToFind)
        {
            StartCoroutine(ShowInfo("BEWARE! DOOR OPENED"));
            firstDoor.Open();
        }
        else StartCoroutine(ShowInfo($"{GameManager.ItemsFound} of{GameManager.ItemsToFind} placed."));
    }
    private IEnumerator ShowInfo(string info)
    {
        if (isInfoShown) yield return null;
        isInfoShown = true;
        infoText.text = info;
        yield return new WaitForSeconds(2f);
        infoText.text = "";
        isInfoShown = false;
    }
}