using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

//TODO: This class has to be universal
public class PickableSlot : Interactable
{
    //Type of pickable object that match to the pickable slot
    [SerializeField] public PickableItem.PickableType type;
    
    //Pickable objects on slot position (hidden until player places the pickable item here)
    [SerializeField] private GameObject placedAxe;
    [SerializeField] private GameObject placedKnife;
    [SerializeField] private GameObject placedPistol;
    [SerializeField] private GameObject placedPills;

    protected override bool HandleFire2()
    {
        //Pickable slot type needs to match item in hands type
        if (type == GameManager.itemInHands.type)
        {
            //TODO: Write info that item collected when info implemented
            
            //Game object in player hands disappears, hidden one in slot appears
            GameManager.itemInHands.gameObject.SetActive(false);
            
            //Show the proper hidden object in slot
            switch (type)
            {
                case PickableItem.PickableType.Axe:
                    placedAxe.gameObject.SetActive(true);
                    GameManager.ItemsFound++;
                    break;
                case PickableItem.PickableType.Knife:
                    placedKnife.gameObject.SetActive(true);
                    GameManager.ItemsFound++;
                    break;
                case PickableItem.PickableType.Pistol:
                    placedPistol.gameObject.SetActive(true);
                    GameManager.ItemsFound++;
                    break;
                case PickableItem.PickableType.Pills:
                    placedPills.gameObject.SetActive(true);
                    GameManager.ItemsFound++;
                    break;
            }
            
            //Removing item from player's hands
            GameManager.itemInHands.transform.SetParent(null);
            GameManager.itemInHands.Rb.isKinematic = false;
            GameManager.itemInHands = null;
        }
        return true;
    }
}
