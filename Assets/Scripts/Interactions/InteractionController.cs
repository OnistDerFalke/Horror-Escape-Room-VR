using System;
using System.Collections;
using NavKeypad;
using UnityEngine;
using UnityEngine.UI;

namespace Interactions
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] private AudioController audioController;

        [SerializeField] private Camera characterCamera;
        [SerializeField] private Transform slot;
        [SerializeField] private float throwItemSpeed;
        [SerializeField] private float dropItemSpeed;
        [SerializeField] private Text infoText;

        [SerializeField] private GameObject placedAxe;
        [SerializeField] private GameObject placedKnife;
        [SerializeField] private GameObject placedPistol;
        [SerializeField] private GameObject placedPills;

        [SerializeField] private DoorController firstDoor;

        [SerializeField] private DogTagsUI dogTagsUI;

        private bool isInfoShown;
    
        //Tags of interactive objects
        public string[] InteractionTags { get; } =
        {
            "Pickable", 
            "HiddenButton", 
            "LightBox", 
            "Pix", 
            "CorpseTag", 
            "KeyPadBtn", 
            "ExitDoor",
            "PickableSlot"
        };

        private string[] Inputs { get; } =
        {
            "Fire1",
            "Fire2"
        };

        //Classes that interactive objects operate on
        private Type[] InteractionClasses { get; } =
        {
            typeof(PickableItem),
            typeof(HiddenButton),
            typeof(LightBox),
            typeof(Pix),
            typeof(CorpseTags),
            typeof(KeypadButton),
            typeof(DoorPass),
            typeof(PickableSlot)
        };


        private void Update()
        {
            HandleInteractions();
        }

        private void HandleInteractions()
        {
            foreach (var input in Inputs)
            {
                if (Input.GetButtonDown(input))
                {
                    //If item in hand and LMB clicked - drop item
                    if (GameManager.itemInHands && input == "Fire1")
                    {
                        GameManager.itemInHands.DropItem();
                        continue;
                    }

                    var breakAll = false;

                    //Use raycast to hit interactable objects
                    var ray = characterCamera.ViewportPointToRay(Vector3.one * 0.5f);
                    var hits = Physics.RaycastAll(ray);

                    //Find first interactable hit and interact with it only
                    foreach (var hit in hits)
                    {
                        for (var i = 0; i < InteractionTags.Length; i++)
                        {
                            if (hit.collider.CompareTag(InteractionTags[i]))
                            {
                                var interactionClass = InteractionClasses[i];
                                var component = hit.transform.GetComponent(interactionClass);
                                var interactiveObject = (Interactable)component;
                                breakAll = interactiveObject.Run(input);
                                if (breakAll)
                                    break;
                            }
                        }
                        if (breakAll)
                            break;
                    }
                    
                    //If item in hand and RMB clicked - throw item
                    if (GameManager.itemInHands && input == "Fire2")
                        GameManager.itemInHands.ThrowItem();
                }
            }
        }
        
        private void ShowItemsFound()
        {
            if (GameManager.ItemsFound == GameManager.ItemsToFind)
            {
                StartCoroutine(ShowInfo("BEWARE! DOOR OPENED"));
                firstDoor.Open();
            }
            else StartCoroutine(ShowInfo($"{GameManager.ItemsFound} of{GameManager.ItemsToFind} placed"));
        }
    
        private IEnumerator ShowInfo(string info)
        {
            infoText.gameObject.SetActive(true);
            if (isInfoShown) yield return null;
            isInfoShown = true;
            infoText.text = info;
            yield return new WaitForSeconds(2.5f);
            infoText.text = "";
            isInfoShown = false;
            infoText.gameObject.SetActive(false);
        }
    }
}