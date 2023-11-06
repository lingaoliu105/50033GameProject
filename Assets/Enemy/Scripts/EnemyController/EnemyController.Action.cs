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
        public float attackTime = 0.2f;
        public float patrolRangeL;
        public float patrolRangeR;
        public float patrolSpeed = 3.0f;
        public float scoutSpeed = 3.5f;

        public virtual IEnumerator AttackOneShot(int i)
        {
            yield return null;
        }

        public virtual void Patrol()
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

        public virtual void Attack()
        {
            animator.SetTrigger("attack");
            StartCoroutine(AttackOneShot(0));
        }

        public virtual void Scout()
        {
            if (targetPlayer)
            {
                facing = targetPlayer.transform.position.x > transform.position.x ? Facings.Right : Facings.Left;
                body.velocity = new Vector2((int)facing * scoutSpeed, body.velocity.y);
            }
            else
            {
                Debug.Log("no target player but still in scouting");
            }
        }

        public virtual bool AttackCanReach()
        {
            return true;
        }

        public virtual void PrepareAttack()
        {
            facing = targetPlayer.transform.position.x > transform.position.x ? Facings.Right : Facings.Left;
            // TODO: add charging animation of bullet
            body.velocity = Vector2.zero;
        }

        public bool HasTarget()
        {
            return targetPlayer != null;
        }

        public virtual void StartIdle()
        {
            
        }

        public virtual void StartPatrol()
        {
            
        }

        public virtual bool ReachedDestination()
        {
            return false;
        }

        public virtual void StartScout()
        {
            
        }
    }
}
