using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : AbstractInteractiveObject
{
    public float liftHeight;

    protected override void Start()
    {
        base.Start();
        col.enabled = false;
    }

    private IEnumerator LiftUp()
    {
        while (transform.position.y < liftHeight)
        {
            transform.position += Vector3.up * 0.01f;
            yield return null;
        }
    }
    
    public override void Interact()
    {
        col.enabled = true;
        StartCoroutine(LiftUp());
    }
}
