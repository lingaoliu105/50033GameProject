using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ShieldmanController : EnemyController
{
    public Vector2 attackRange = new Vector2(0.3f, 1.2f);
    public LayerMask playerMask;
    public float attackOffset = 0.3f;
    public override bool AttackCanReach()
    {
        Vector3 centerOffset =  Vector3.right * attackOffset;
        if (facing == Game.Facings.Left)
        {
            centerOffset.x = -centerOffset.x;
        }
        var raycastAll = Physics2D.OverlapBoxAll(transform.position+centerOffset, attackRange, 0, playerMask);
        return raycastAll.Length > 0;
    }

    public override IEnumerator AttackOneShot(int i)
    {
        yield return new WaitForSeconds(1);
        
    }

    public override void OnDrawGizmos()
    {
        Vector3 center = transform.position + Vector3.right * attackOffset;
        Gizmos.DrawCube(center,attackRange);
    }
}
