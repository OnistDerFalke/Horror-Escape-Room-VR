using System.Collections;
using Interactions;
using UnityEngine;

public class HiddenButton : Interactable
{
    [SerializeField] private GameObject doorWall;
    [SerializeField] private ScarySituation jumpscare;

    [SerializeField] private AudioSource stoneWallSource;
    [SerializeField] private AudioSource buttonSource;
    
    [SerializeField] private Highlight highlight;
    
    protected override bool HandleFire1()
    {
        //TODO: Show info that "Hidden room opened" after moving info to other component
        highlight.SetHighlight(false);
        gameObject.tag = "Untagged";
        jumpscare.TurnKeyLights(false);
        StartCoroutine(OpenDoorAnimation());
        return true;
    }

    private IEnumerator OpenDoorAnimation()
    {
        bool jsRan = false;
        buttonSource.Play();
        while (gameObject.transform.localPosition.z >= -0.15f)
        {
            gameObject.transform.localPosition += new Vector3(0, 0, -0.4f * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }
        gameObject.transform.localScale = Vector3.zero;
        buttonSource.Stop();
        yield return new WaitForSeconds(1f);
        stoneWallSource.Play();
        while (doorWall.gameObject.transform.localPosition.y >= -5)
        {
            if (!jsRan && doorWall.gameObject.transform.localPosition.y < -2)
            {
                jumpscare.MonsterRunEvent();
                jsRan = true;
            }
            doorWall.gameObject.transform.localPosition += new Vector3(0, -0.7f * Time.deltaTime, 0);
            yield return new WaitForSeconds(0.001f);
        }
        stoneWallSource.Stop();
        doorWall.SetActive(false);
        gameObject.SetActive(false);
    }
}
