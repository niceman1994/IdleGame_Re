using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] GameObject cameraTargetObject;
    [SerializeField] float cameraMoveProgress = 0.2f;

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(cameraTargetObject.transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, cameraMoveProgress);
    }
}
