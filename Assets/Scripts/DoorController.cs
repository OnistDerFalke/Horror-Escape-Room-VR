using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float openingSpeed;

    public void Open()
    {
        StartCoroutine(OpeningProcess());
    }

    private IEnumerator OpeningProcess()
    {
        while (door.gameObject.transform.localRotation.eulerAngles.y < 90)
        {
            var currentRotation = door.gameObject.transform.localRotation.eulerAngles;
            currentRotation.y += openingSpeed * Time.deltaTime;
            door.gameObject.transform.localRotation = Quaternion.Euler(currentRotation);
            yield return new WaitForSeconds(0.001f);
        }
    }
}
