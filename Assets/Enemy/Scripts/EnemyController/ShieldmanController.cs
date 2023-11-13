using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ShieldmanController : EnemyController
{
    public Vector2 attackRange = new Vector2(0.3f, 1.2f);
    public float attackOffset = 0.3f;
    public override bool AttackCanReach()
    {
        float horizontalDiff = Mathf.Abs(GetPlayerPosition().x - transform.position.x);

        return horizontalDiff < attackOffset + attackRange.x;
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
    
}
