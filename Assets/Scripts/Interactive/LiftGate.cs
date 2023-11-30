using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftGate : AbstractInteractiveObject
{
    private GameObject gate;

    public float liftHeight;
    public bool flag;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        foreach (Transform childTrans in transform)
        {
            var childObj = childTrans.gameObject;
            if (childObj.name == "Gate")
            {
                gate = childObj;
                return;
            }
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

    protected IEnumerator LiftUp()
    {
        while (gate.transform.localPosition.y < liftHeight)
        {
            gate.transform.localPosition += Vector3.up * 0.001f;
            yield return null;
        }

        col.enabled = false;
    }

    public override void Interact()
    {
        StartCoroutine(LiftUp());
    }
}
