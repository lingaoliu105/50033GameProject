using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.UI.Image;

namespace Game {

    public enum AutoActionState {
        None = 0,
        Patrol = 1,
        Scout = 2,
        Attack = 3,
    }

    public partial class EnemyController
    {
        [FormerlySerializedAs("attackTemplate")]
        public GameObject[] attackTemplates;

        protected GameObject[] attackInstances = new GameObject[3];
        protected float attackTime = 0.2f;
        public float patrolRangeL;
        public float patrolRangeR;
        public float patrolSpeed = 3.0f;

        public IEnumerator AttackOneShot(int i)
        {
            yield return new WaitForSeconds(attackTime);
            attackInstances[0] = Instantiate(attackTemplates[i], transform.position, Quaternion.identity);
            attackInstances[0].transform.parent = transform;
        }

        public void PerformMove(MoveDirectionX move)
        {
            if (move != MoveDirectionX.None)
            {
                facing = (Facings)move;
            }
        }

        public void Patrol()
        {
            if (transform.position.x < patrolRangeL)
            {
                facing = Facings.Right;
            }

            if (transform.position.x > patrolRangeR)
            {
                facing = Facings.Left;
            }

            body.velocity = new Vector2((int)facing * patrolSpeed, body.velocity.y);
        }

        public void Attack()
        {
            animator.SetTrigger("attack");
            StartCoroutine(AttackOneShot(0));
        }

        public void StartScout(float scoutSpeed)
        {
            if (targetPlayer)
            {
                
                body.AddForce(targetPlayer.transform.position.x > transform.position.x? Vector2.right*scoutSpeed : Vector2.left*scoutSpeed,ForceMode2D.Force);
            }
            else
            {
                Debug.Log("no target player but still in scouting");
            }
        }

        public virtual bool AttackCanReach()
        {
            return targetPlayer && MathF.Abs(targetPlayer.transform.position.x - transform.position.x) < 5;
        }

        public void PrepareAttack()
        {
            // TODO: add charging animation of bullet
            body.velocity = Vector2.zero;
        }

        public bool HasTarget()
        {
            return targetPlayer != null;
        }
    }
}
