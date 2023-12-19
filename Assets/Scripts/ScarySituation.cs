using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarySituation : MonoBehaviour
{
    [SerializeField] private GameObject monsterObject;
    [SerializeField] private float monsterSpeed;
    [SerializeField] private float monsterMovementDistance;

    [SerializeField] private Light[] keyLights;
    [SerializeField] private AudioController audioControllerOculus;
    [SerializeField] private AudioController audioControllerMouseNKeyboard;

    public void MonsterRunEvent()
    {
        monsterObject.SetActive(true);
        GameManager.SoundType = GameManager.GlobalSoundType.Storage;
        StartCoroutine(MonsterRunAnimation());
    }

    private IEnumerator MonsterRunAnimation()
    {
        var dist = 0f;
        var speed = 0.1f * monsterSpeed;
        switch(GameManager.Controls)
        {
            case GameManager.ControlsType.OCULUS:
                audioControllerOculus.PlayJumpscare1Sound();
                break;
            case GameManager.ControlsType.MOUSENKEYBOARD:
                audioControllerMouseNKeyboard.PlayJumpscare1Sound();
                break;
            case GameManager.ControlsType.OCULUSNPAD:
                break;
        }
  
        while (dist < monsterMovementDistance)
        {
            monsterObject.gameObject.transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            dist += speed * Time.deltaTime;
            yield return new WaitForSeconds(0.001f * Time.deltaTime);
        }
        monsterObject.SetActive(false);
        TurnKeyLights(true);
    }

    public void TurnKeyLights(bool doTurnOn)
    {
        if (doTurnOn)
        {
            foreach (var t in keyLights)
            {
                t.intensity = 1.5f;
            }
        }
        else
        {
            foreach (var l in keyLights)
                l.intensity = 0f;
        }
    }
}
