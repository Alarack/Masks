using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject targetPos;
    public float speed = 2f;

    void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, targetPos.transform.position, Time.deltaTime * speed);
    }
}
