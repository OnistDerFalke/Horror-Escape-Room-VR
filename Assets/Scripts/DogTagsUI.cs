using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DogTagsUI : MonoBehaviour
{
    [SerializeField] private GameObject dogTagsUIWindow;
    [SerializeField] private Image[] dogTagsOnUI = new Image[5];

    [SerializeField] private Highlight[] corpseTagsHighlights = new Highlight[4];
    [SerializeField] private Highlight[] keyPadHightlights = new Highlight[11];

    private int[] dictionary = {2, 4, 3, 1, 0};
    private void Start()
    {
        foreach(var h in corpseTagsHighlights)
            h.SetHighlight(false);
        foreach(var h in keyPadHightlights)
            h.SetHighlight(false);
    }
    
    public void HideDogTagsUIWindow()
    {
        dogTagsUIWindow.SetActive(false);
        foreach(var h in corpseTagsHighlights)
            h.SetHighlight(false);
    }

    public void ShowDogTagsUIWindow()
    {
        dogTagsUIWindow.SetActive(true);
        foreach (var h in corpseTagsHighlights)
        {
            h.SetHighlight(true);
            h.tag = "CorpseTag";
        }
        foreach (var h in keyPadHightlights)
        {
            h.SetHighlight(true);
            h.tag = "KeyPadBtn";
        }
    }

    public void SetDogTagDown(int index)
    {
        dogTagsOnUI[dictionary[index]].color = Color.gray;
        corpseTagsHighlights[index].SetHighlight(false);
    }
}
