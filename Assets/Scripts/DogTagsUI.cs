using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogTagsUI : MonoBehaviour
{
    [SerializeField] private GameObject dogTagsUIWindow;

    public void HideDogTagsUIWindow()
    {
        dogTagsUIWindow.SetActive(false);
    }

    public void ShowDogTagsUIWindow()
    {
        dogTagsUIWindow.SetActive(true);
    }
}
