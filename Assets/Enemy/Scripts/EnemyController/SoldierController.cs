using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class SoldierController : EnemyController {
        public override IEnumerator AttackOneShot(int i) {
            yield return new WaitForSeconds(attackTime);
            Vector3 offset = new Vector3(0.1f, 0.1f, 0);
            if (facing == Facings.Left) {
                offset.x = -offset.x;
            }
            GameObject atk = Instantiate(attackTemplates[i], transform.position + offset, Quaternion.identity);
            if (facing == Facings.Left) {
                Vector3 oldScale = atk.transform.localScale;
                // horizontally flip
                atk.transform.localScale = new Vector3(-oldScale.x, oldScale.y, oldScale.z);
            }
        }
    }
}
