using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; 
    public Vector3 offset = new Vector3(0f, 2f, -3f); 
    public float smoothTime = 0.3f; 
    private Vector3 velocity = Vector3.zero;
    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}