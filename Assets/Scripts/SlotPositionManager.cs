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
        //Debug.Log($"Right glove angles: {rightGloveTransform.eulerAngles}");        
        //Debug.Log($"Right glove position: {rightGloveTransform.position}");

        /*
        var angles = rightGloveTransform.eulerAngles * Mathf.PI / 180f;
        var offset = 0.05f;

        gameObject.transform.position = new Vector3(
            rightGloveTransform.position.x + (Mathf.Cos(angles.y) - Mathf.Cos(angles.z)) * offset,
            rightGloveTransform.position.y - (Mathf.Cos(angles.x) + Mathf.Sin(angles.z)) * offset,
            rightGloveTransform.position.z + (Mathf.Sin(angles.y) + Mathf.Cos(angles.x)) * offset
        );
        */

        
        var angles = rightGloveTransform.eulerAngles;

        gameObject.transform.position = new Vector3(
            rightGloveTransform.position.x
                + (Mathf.Cos(angles.z * Mathf.PI / 180f) - Mathf.Cos(angles.y * Mathf.PI / 180f)) / 2f * 0.05f,
            rightGloveTransform.position.y
                - Mathf.Cos(angles.z * Mathf.PI / 180f) * 0.05f,
            rightGloveTransform.position.z
        );
        

        //gameObject.transform.position = new Vector3(
        //    rightGloveTransform.position.x - Mathf.Cos(rightGloveTransform.eulerAngles.y * Mathf.PI / 180f) * 0.05f, 
        //    rightGloveTransform.position.y + 0.05f, 
        //    rightGloveTransform.position.z
        //);

        //(leftGloveTransform.position.x+rightGloveTransform.position.x)/2f,
        //(leftGloveTransform.position.y + rightGloveTransform.position.y) / 2f - 0.05f,
        //(leftGloveTransform.position.z + rightGloveTransform.position.z) / 2f);
    }
}
