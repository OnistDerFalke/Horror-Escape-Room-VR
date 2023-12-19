using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class TorchPositionManager : MonoBehaviour
{
    [SerializeField] private Transform leftGloveTransform;
    [SerializeField] private Transform rightGloveTransform;

    void Update()
    {
        gameObject.transform.position = new Vector3(
            leftGloveTransform.position.x + Mathf.Cos(leftGloveTransform.eulerAngles.y * Mathf.PI / 180f) * 0.05f,
            leftGloveTransform.position.y - 0.2f,
            leftGloveTransform.position.z
        );
    }
}
