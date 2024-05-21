using Interactions;
using UnityEngine;

public class CorpseTags : Interactable
{
    [SerializeField] private DogTagsUI dogTagsUI;
    
    public int index;
    
    protected override bool HandleFire1()
    {
        dogTagsUI.SetDogTagDown(index);
        return true;
    }
}
