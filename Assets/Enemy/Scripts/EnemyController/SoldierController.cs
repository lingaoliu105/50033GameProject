using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class SoldierController : EnemyController
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        hp = gameConstants.soldierMaxHP;
    }

    public override IEnumerator AttackOneShot(int i)
    {
        yield return new WaitForSeconds(attackTime);
        Vector3 offset = new Vector3(0.1f, 0.1f, 0);
        if (facing == Game.Facings.Left)
        {
            offset.x = -offset.x;
        }
        GameObject atk = Instantiate(attackTemplates[i], transform.position+offset, Quaternion.identity);
        if (facing == Game.Facings.Left)
        {
            atk.GetComponent<SpriteRenderer>().flipX = true;
        }
        atk.GetComponent<Rigidbody2D>().AddForce(facing==Game.Facings.Right ? Vector2.right * bulletSpawnForce : Vector2.left * bulletSpawnForce,ForceMode2D.Impulse);
        atk.transform.parent = transform;
    }
}
