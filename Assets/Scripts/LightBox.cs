using System.Collections;
using UnityEngine;

public class LightBox : MonoBehaviour
{
    [SerializeField] private Light[] secondRoomLights = new Light[3];
    [SerializeField] private Light firstRoomLight;
    [SerializeField] private Light lampLight;

    [SerializeField] private GameObject monsterObject;
    [SerializeField] private AudioController audioController;
    [SerializeField] private AudioSource lightBoxAudioSource;

    [SerializeField] private Highlight highlight;
    [SerializeField] private GameObject pixObject;
    public void Use()
    {
        highlight.SetHighlight(false);
        gameObject.tag = "Untagged";
        firstRoomLight.intensity = 2f;
        StartCoroutine(TurnLightsOn());
    }

    private IEnumerator TurnLightsOn()
    {
        lightBoxAudioSource.Play();
        yield return new WaitForSeconds(3f);
        for (var i=2; i>=0; i--){
            if (i == 2)
            {
                monsterObject.SetActive(true);
                audioController.PlayJumpscare2Sound();
                yield return new WaitForSeconds(0.8f);
                lampLight.intensity = 0f;
                firstRoomLight.intensity = 0f;
                monsterObject.SetActive(false);
            }
            secondRoomLights[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
        }
        firstRoomLight.intensity = 4f;
        lampLight.intensity = 3f;
        yield return new WaitForSeconds(0.5f);
        lightBoxAudioSource.Stop();
        pixObject.tag = "Pix";
        pixObject.GetComponent<Highlight>().SetHighlight(true);
    }
}
