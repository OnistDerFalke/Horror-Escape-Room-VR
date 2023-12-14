using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarySituation : MonoBehaviour
{
    [SerializeField] private GameObject monsterObject;
    [SerializeField] private float monsterSpeed;
    [SerializeField] private float monsterMovementDistance;

    [SerializeField] private Light[] keyLights;
    private float[] lightIntesities;

    private void Start()
    {
        lightIntesities = new float[keyLights.Length];
        for (var i = 0; i < keyLights.Length; i++)
            lightIntesities[i] = keyLights[i].intensity;
    }
    public void MonsterRunEvent()
    {
        monsterObject.SetActive(true);
        StartCoroutine(MonsterRunAnimation());
    }

    private IEnumerator MonsterRunAnimation()
    {
        var dist = 0f;
        var speed = 0.001f * monsterSpeed;
        while (dist < monsterMovementDistance)
        {
            monsterObject.gameObject.transform.position += new Vector3(-speed, 0, 0);
            dist += speed;
            yield return new WaitForSeconds(0.001f * Time.deltaTime);
        }
        monsterObject.SetActive(false);
    }

    public void TurnKeyLights(bool doTurnOn)
    {
        if (doTurnOn)
        {
            for (var i = 0; i < keyLights.Length; i++)
                keyLights[i].intensity = lightIntesities[i];
        }
        else
        {
            foreach (var l in keyLights)
                l.intensity = 0f;
        }
    }
}
