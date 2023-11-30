using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lift : AbstractInteractiveObject
{
    public float liftHeight;
    public bool flag;

    private IEnumerator LiftUp()
    {
        while (transform.position.y < liftHeight)
        {
            transform.position += Vector3.up * 0.0001f;
            yield return null;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (flag)
        {
            Interact();
        }
    }
    public override void Interact()
    {
        StartCoroutine(LiftUp());
        
    }
}
