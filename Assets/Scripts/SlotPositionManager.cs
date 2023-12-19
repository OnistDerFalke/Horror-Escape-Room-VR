using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class SlotPositionManager : MonoBehaviour
{
    [SerializeField] private Transform leftGloveTransform;
    [SerializeField] private Transform rightGloveTransform;

    void Update()
    {
        gameObject.transform.position = new Vector3(
            rightGloveTransform.position.x - Mathf.Cos(rightGloveTransform.eulerAngles.y * Mathf.PI / 180f) * 0.05f, 
            rightGloveTransform.position.y + 0.05f, 
            rightGloveTransform.position.z
        );
        //(leftGloveTransform.position.x+rightGloveTransform.position.x)/2f,
        //(leftGloveTransform.position.y + rightGloveTransform.position.y) / 2f - 0.05f,
        //(leftGloveTransform.position.z + rightGloveTransform.position.z) / 2f);
    }
}
