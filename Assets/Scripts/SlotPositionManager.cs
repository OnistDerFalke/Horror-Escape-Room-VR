using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPositionManager : MonoBehaviour
{
    [SerializeField] private Transform leftGloveTransform;
    [SerializeField] private Transform rightGloveTransform;

    void Update()
    {
        gameObject.transform.position = new Vector3(
            (leftGloveTransform.position.x+rightGloveTransform.position.x)/2f,
            (leftGloveTransform.position.y + rightGloveTransform.position.y) / 2f - 0.05f,
            (leftGloveTransform.position.z + rightGloveTransform.position.z) / 2f);
    }
}
