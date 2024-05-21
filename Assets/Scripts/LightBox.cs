using System.Collections;
using Interactions;
using UnityEngine;

public class LightBox : Interactable
{
    [SerializeField] private Light[] secondRoomLights = new Light[3];
    [SerializeField] private Light firstRoomLight;
    [SerializeField] private Light lampLight;

    [SerializeField] private GameObject monsterObject;
    [SerializeField] private AudioController audioControllerOculus;
    [SerializeField] private AudioController audioControllerMouseNKeyboard;
    [SerializeField] private AudioSource lightBoxAudioSource;

    [SerializeField] private Highlight highlight;
    [SerializeField] private GameObject pixObject;

    protected override bool HandleFire1()
    {
        //TODO: Show info that "Power is back" after moving info to other component
        highlight.SetHighlight(false);
        gameObject.tag = "Untagged";
        firstRoomLight.intensity = 2f;
        StartCoroutine(TurnLightsOn());
        return true;
    }

    private IEnumerator TurnLightsOn()
    {
        lightBoxAudioSource.Play();
        yield return new WaitForSeconds(3f);
        for (var i=2; i>=0; i--){
            if (i == 2)
            {
                monsterObject.SetActive(true);
                switch (GameManager.Controls)
                {
                    case GameManager.ControlsType.OCULUS:
                        audioControllerOculus.PlayJumpscare2Sound();
                        break;
                    case GameManager.ControlsType.MOUSENKEYBOARD:
                        audioControllerMouseNKeyboard.PlayJumpscare2Sound();
                        break;
                    case GameManager.ControlsType.OCULUSNPAD:
                        audioControllerMouseNKeyboard.PlayJumpscare2Sound();
                        break;
                }
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
