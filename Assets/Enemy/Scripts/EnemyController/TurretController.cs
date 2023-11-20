using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class TurretController : EnemyController
    {

        public override IEnumerator AttackOneShot(int i)
        {
            yield return new WaitForSeconds(attackTime);
            for (int j = 0; j < 3; j++)
            {
                Vector3 offset = Vector3.up * 0.5f;
                GameObject atkLeft = Instantiate(attackTemplates[i], transform.position+offset, Quaternion.identity);
                GameObject atkRight = Instantiate(attackTemplates[i], transform.position+offset, Quaternion.identity);
            
                Vector3 oldScale = atkLeft.transform.localScale;
                    
                // horizontally flip
                atkLeft.transform.localScale = new Vector3(-oldScale.x, oldScale.y, oldScale.z);
                yield return new WaitForSeconds(0.3f);
            }

        }

    }
}
