using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy {
    public class SlimeController : EnemyController {
        public float attackRange = 5f;
        public float jumpHeight = 3f;
        public override bool AttackCanReach() {
            float horizontalDiff = Mathf.Abs(GetPlayerPosition().x - transform.position.x);
            return horizontalDiff < attackRange;
        }
        
        public override IEnumerator AttackOneShot(int i) {
            yield return new WaitForSeconds(attackTime);
            var targetPosition = GetPlayerPosition();
            facing = targetPosition.x > transform.position.x ? Facings.Right : Facings.Left;
            float horizontalDistance = targetPosition.x - transform.position.x;
            float upwardV = MathF.Sqrt(Physics2D.gravity.magnitude * jumpHeight);
            float horizontalV = 2*horizontalDistance * Physics2D.gravity.magnitude / upwardV;
            body.AddForce(body.mass*new Vector2(horizontalV,upwardV),ForceMode2D.Impulse);
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            Debug.Log(body.velocity);
            if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.layer == 6) &&
                body.velocity.y < 0.01)
            {
                Instantiate(attackTemplates[0], transform.position, Quaternion.identity);
            }
        }
    }
}