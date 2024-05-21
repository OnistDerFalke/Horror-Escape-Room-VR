using System.Collections;
using Interactions;
using UnityEngine;

public class Pix : Interactable
{
    [SerializeField] private Highlight highlight;
    [SerializeField] private AudioSource unlockAudioSource;
    [SerializeField] private AudioSource openAudioSource;
    
    [SerializeField] private GameObject strapAxis;
    [SerializeField] private GameObject mainAxis;

    [SerializeField] private float strapSpeed;
    [SerializeField] private float mainSpeed;
    [SerializeField] private float mainAxisMaxAngle;
    [SerializeField] private float strapAxisMaxAngle;

    [SerializeField] private DogTagsUI dogtagsUI;
    [SerializeField] private DogTagsUI dogtagsUIPad;
    [SerializeField] private GameObject[] dogTagsObjects = new GameObject[5];

    public bool isUnlocked;
    public bool isOpened;

    [SerializeField] private PixUI pixUI;
    [SerializeField] private PixUI pixUIPad;

    private void Start()
    {
        highlight.SetHighlight(false);
    }
    
    protected override bool HandleFire1()
    {
        if (isOpened)
        {
            TakeDogTags();
            return true;
        }

        if (isUnlocked)
            OpenPix();
        else UnlockPix();

        return true;
    }

    private void OpenPix()
    {
        highlight.SetHighlight(false);
        
        if(isUnlocked)
            StartCoroutine(OpenPixAnimation());
    }

    private void UnlockPix()
    {
        if (GameManager.Controls == GameManager.ControlsType.OCULUS)
            pixUI.ShowPixUIWindow();
        else if (GameManager.Controls == GameManager.ControlsType.OCULUSNPAD)
            pixUIPad.ShowPixUIWindow();
    }

    private void TakeDogTags()
    {
        if (!isOpened) return;
        gameObject.tag = "Untagged";
        foreach(var o in dogTagsObjects)
            o.SetActive(false);
        
        if (GameManager.Controls == GameManager.ControlsType.OCULUS)
            dogtagsUI.ShowDogTagsUIWindow();
        else if (GameManager.Controls == GameManager.ControlsType.OCULUSNPAD)
            dogtagsUIPad.ShowDogTagsUIWindow();
    }

    private IEnumerator OpenPixAnimation()
    {
        while (strapAxis.transform.eulerAngles.z >= strapAxisMaxAngle)
        {
            strapAxis.transform.eulerAngles += new Vector3(0, 0, -strapSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }
        while (mainAxis.transform.eulerAngles.z >= mainAxisMaxAngle)
        {
            mainAxis.transform.eulerAngles += new Vector3(0, 0, -mainSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }

        isOpened = true;
    }
}
