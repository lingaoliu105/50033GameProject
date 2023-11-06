using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Game
{
    public class TurretController : EnemyController
    {

        public override IEnumerator AttackOneShot(int i)
        {
            yield return new WaitForSeconds(attackTime);
            for (int j = 0; j < 3; j++)
            {
                Vector3 offset = Vector3.up * 0.15f;
                GameObject atkLeft = Instantiate(attackTemplates[i], transform.position+offset, Quaternion.identity);
                GameObject atkRight = Instantiate(attackTemplates[i], transform.position+offset, Quaternion.identity);
            
                Vector3 oldScale = atkLeft.transform.localScale;
                    
                // horizontally flip
                atkLeft.transform.localScale = new Vector3(-oldScale.x, oldScale.y, oldScale.z);
                
                atkLeft.GetComponent<Rigidbody2D>().AddForce(Vector2.left * bulletSpawnForce,ForceMode2D.Impulse);
                atkRight.GetComponent<Rigidbody2D>().AddForce(Vector2.right * bulletSpawnForce,ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.1f);
            }

        }

        public override void Patrol()
        {
            // don't move
        }
        

        public override void Scout()
        {
            // don't move
        }

        public override void PrepareAttack()
        {
            Attack();
        }


    }
}
