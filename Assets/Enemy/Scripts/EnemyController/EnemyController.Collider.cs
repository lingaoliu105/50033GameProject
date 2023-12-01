using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Attack;
using UnityEngine.Serialization;

namespace Enemy
{
    public partial class EnemyController
    {
        // TODO: confirm ground layer number
        private int groundMask = 1 << 6;
        public Vector2 groundBoxSize = new Vector2(0.3f, 0.1f);
        public float groundCheckOffset;

        const float DEVIATION = 0.02f; //碰撞检测误差

        private int playerAttackLayer = 11;

        protected bool CheckGround()
        {
            var rayCastAll = Physics2D.OverlapBoxAll(transform.position + groundCheckOffset * Vector3.down,
                groundBoxSize, 0, groundMask);
            return rayCastAll.Length > 0;
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("PlayerProjectile"))
            {
                BasicPlayerProjectile attack = collision.gameObject.GetComponent<BasicPlayerProjectile>();
                if (attack && !attack.hasHit)
                {
                    attack.hasHit = true;
                    PlayHitSound();
                    TakeDamage(attack.attackDamage);
                }
            }

            if (collision.gameObject.CompareTag("Void"))
            {
                TakeDamage(9999999);
            }
        }
        
        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("PlayerProjectile"))
            {
                BasicPlayerProjectile attack = other.gameObject.GetComponent<BasicPlayerProjectile>();
                if (attack && !attack.hasHit)
                {
                    attack.hasHit = true;
                    PlayHitSound();
                    TakeDamage(attack.attackDamage);
                }
            }

            if (other.gameObject.CompareTag("Void"))
            {
                TakeDamage(9999999);
            }
        }
    }
}