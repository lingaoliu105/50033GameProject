using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : AbstractInteractiveObject
{
    public float liftHeight;

    private IEnumerator LiftUp()
    {
        while (transform.position.y < liftHeight)
        {
            transform.position += Vector3.up * 0.0001f;
            yield return null;
        }
    }
    
    public override void Interact()
    {
        StartCoroutine(LiftUp());
    }
}
