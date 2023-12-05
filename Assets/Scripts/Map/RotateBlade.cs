using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class RotateBlade : MonoBehaviour
{
    public float rotateSpeed;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, rotateSpeed));
    }
}
