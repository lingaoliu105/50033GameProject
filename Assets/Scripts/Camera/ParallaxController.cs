using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParallaxController : MonoBehaviour
{
    public Vector3 CameraPosition;
    public float moveRateX, moveRateY;

    public float offsetX, offsetY;
 
    void Update()
    {
        transform.position = new Vector2(CameraPosition.x * moveRateX + offsetX, CameraPosition.y * moveRateY + offsetY);
    }
}