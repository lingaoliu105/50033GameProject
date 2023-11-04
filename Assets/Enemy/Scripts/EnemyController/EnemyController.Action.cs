﻿using System;
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
        protected float attackTime = 0.2f;
        public float patrolRangeL;
        public float patrolRangeR;
        public float patrolSpeed = 3.0f;
        public float bulletSpawnForce = 3.0f;
        public float scoutSpeed = 3.5f;

        public virtual IEnumerator AttackOneShot(int i)
        {
            yield return new WaitForSeconds(attackTime);
            GameObject atk = Instantiate(attackTemplates[i], transform.position, Quaternion.identity);
            Time.timeScale = 0.0f;
            if (facing == Facings.Left)
            {
                Vector3 oldScale = atk.transform.localScale;
                atk.transform.localScale = new Vector3(-oldScale.x, oldScale.y, oldScale.z);
            }
            atk.GetComponent<Rigidbody2D>().AddForce(facing==Facings.Right ? Vector2.right * bulletSpawnForce : Vector2.left * bulletSpawnForce,ForceMode2D.Impulse);
            atk.transform.parent = transform;
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

        public void Scout()
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
            return targetPlayer && MathF.Abs(targetPlayer.transform.position.x - transform.position.x) < 2;
        }

        public void PrepareAttack()
        {
            facing = targetPlayer.transform.position.x > transform.position.x ? Facings.Right : Facings.Left;
            // TODO: add charging animation of bullet
            body.velocity = Vector2.zero;
        }

        public bool HasTarget()
        {
            return targetPlayer != null;
        }
    }
}