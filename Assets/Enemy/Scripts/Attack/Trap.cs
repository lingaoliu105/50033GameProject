using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class Trap : EnemyAttack
{
    public float damageInterval;
    private float lastEffectTime;

    private void Awake()
    {
        lastEffectTime = Time.time;
    }

    public override IEnumerator WaitAndDestroy()
    {
        throw new System.NotImplementedException();
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        throw new System.NotImplementedException();
    }

    public override bool CheckActive()
    {
        float t = Time.time;
        if ((t - lastEffectTime) > damageInterval)
        {
            lastEffectTime = t;
            return true;
        }

        return false;
    }

    public override void Hitting()
    {
        attackAudio.PlayOneShot(hitAudio);
    }
}
