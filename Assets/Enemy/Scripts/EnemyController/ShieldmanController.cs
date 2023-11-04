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
        var raycastAll = Physics2D.OverlapBoxAll(GetAttackCenter(), attackRange, 0, playerMask);
        return raycastAll.Length > 0;
    }

    private Vector3 GetAttackCenter()
    {
        Vector3 centerOffset =  Vector3.right * attackOffset;
        if (facing == Game.Facings.Left)
        {
            centerOffset.x = -centerOffset.x;
        }

        return transform.position + centerOffset;
    }

    public override IEnumerator AttackOneShot(int i)
    {
        yield return new WaitForSeconds(attackTime);
        GameObject atk = Instantiate(attackTemplates[i], GetAttackCenter(), Quaternion.identity);
        if (facing == Game.Facings.Left)
        {
            atk.GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public override void OnDrawGizmos()
    {
        // Vector3 center = transform.position + Vector3.right * attackOffset;
        // Gizmos.DrawCube(center,attackRange);
    }
}
