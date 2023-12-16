using System.Collections;
using UnityEngine;

public class Pix : MonoBehaviour
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

    [SerializeField] private GameObject[] dogTagsObjects = new GameObject[5];

    public bool isUnlocked;
    public bool isOpened;

    [SerializeField] private PixUI pixUI;

    private void Start()
    {
        highlight.SetHighlight(false);
    }
    public void OpenPix()
    {
        highlight.SetHighlight(false);
        
        if(isUnlocked)
            StartCoroutine(OpenPixAnimation());
    }

    public void UnlockPix()
    {
        pixUI.ShowPixUIWindow();
    }

    public void TakeDogTags()
    {
        if (!isOpened) return;
        gameObject.tag = "Untagged";
        foreach(var o in dogTagsObjects)
            o.SetActive(false);
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
