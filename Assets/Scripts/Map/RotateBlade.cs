using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class RotateBlade : EnemyAttack
{
    public float rotateSpeed;
    // Start is called before the first frame update
    public override IEnumerator WaitAndDestroy()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        
    }

    public override void Hitting()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, rotateSpeed));
    }
}
