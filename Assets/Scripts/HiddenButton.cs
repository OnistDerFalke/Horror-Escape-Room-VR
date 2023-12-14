using System.Collections;
using UnityEngine;

public class HiddenButton : MonoBehaviour
{
    [SerializeField] private GameObject doorWall;
    [SerializeField] private ScarySituation jumpscare;
    public void Use()
    {
        gameObject.tag = "Untagged";
        jumpscare.TurnKeyLights(false);
        StartCoroutine(OpenDoorAnimation());
    }

    private IEnumerator OpenDoorAnimation()
    {
        bool jsRan = false;
        
        while (gameObject.transform.localPosition.z >= -0.15f)
        {
            gameObject.transform.localPosition += new Vector3(0, 0, -0.4f * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }
        yield return new WaitForSeconds(1f);
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
        doorWall.SetActive(false);
        gameObject.SetActive(false);
        jumpscare.TurnKeyLights(true);
    }
}
