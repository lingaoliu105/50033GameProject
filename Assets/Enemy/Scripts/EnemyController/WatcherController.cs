using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class WatcherController : EnemyController
    {
        public Vector2 moveDestination;
        public float verticalMoveRange = 6f;
        public float horizontalMoveRange = 8f;
        public bool isMoving;
        public float attackRange = 6f; //attack range is a circle with this radius
        public override void StartPatrol()
        {
            moveDestination = new Vector2((Random.value - 0.5f) * horizontalMoveRange,
                (Random.value - 0.5f) * verticalMoveRange);
            AdjustOrientation();
            isMoving = true;
        }

        protected override void FixedUpdate()
        {
            if (isMoving)
            {
                body.MovePosition(body.position + (moveDestination-body.position) * (patrolSpeed * Time.fixedDeltaTime));
            }
            animator.SetFloat("speed",body.velocity.magnitude);
            animator.SetBool("hasTarget",targetPlayer!=null);
        }

        public override IEnumerator AttackOneShot(int i)
        {
            yield return new WaitForSeconds(attackTime);
            GameObject atk = Instantiate(attackTemplates[i], transform.position, sprite.transform.rotation);
            if (sprite.transform.localScale.x < 0)
            {
                Vector3 oldScale = atk.transform.localScale;
                // horizontally flip
                atk.transform.localScale = new Vector3(-oldScale.x, oldScale.y, oldScale.z);
            }

        }

        private void AdjustOrientation()
        {
            // if destination at the left, flip horizontally. otherwise reset
            var localScale = sprite.transform.localScale;
            if ((moveDestination.x < transform.position.x && localScale.x > 0) || (moveDestination.x > transform.position.x && localScale.x < 0))
            {
                localScale =
                    new Vector3(-localScale.x, localScale.y, localScale.z);
                sprite.transform.localScale = localScale;
            }

            var angle = Mathf.Clamp(Mathf.Atan((moveDestination.y - transform.position.y) /
                                               (moveDestination.x - transform.position.x)),-Mathf.PI/4,Mathf.PI/4)*Mathf.Rad2Deg;
           
            sprite.transform.rotation = Quaternion.Euler(0,0,angle);
        }
        public override void Attack()
        {
            base.Attack();
            isMoving = false;
            moveDestination = targetPlayer.transform.position;
            AdjustOrientation();
        }

        public override void StartScout()
        {
            if (targetPlayer)
            {
                var angle = Random.Range(0, Mathf.PI/4);
                if (Random.value < 0.5)
                {
                    angle += Mathf.PI/2;
                }

                float distance = Mathf.Max(Random.value,0.3f) * attackRange;
                moveDestination = targetPlayer.transform.position;
                moveDestination += distance * (new Vector2(Mathf.Cos(angle),Mathf.Sin(angle)));
                AdjustOrientation();
                isMoving = true;
            }
            else
            {
                Debug.Log("Watcher: no target but scouting");
            }
        }

        public override bool AttackCanReach()
        {
            if (targetPlayer)
            {
                return (targetPlayer.transform.position - transform.position).magnitude < attackRange;
            }

            return false;
        }
        

        public override void StartIdle()
        {
            body.velocity = Vector2.zero;
            isMoving = false;
            
            // TODO: add smooth rotation transition
            sprite.transform.rotation = Quaternion.identity;
        }

        public override bool ReachedDestination()
        {
            return (body.position - moveDestination).magnitude < 0.1;
        }
    }
}
