using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Enemy
{
    public class BasicEnemyProjectile : EnemyAttack
    {
        [Header("存在时间")] public int attackTimeByFrame = 60;

        private int attackCount = 0;
        protected Animator anim;
        protected Rigidbody2D body;
        protected Collider2D collider;
        public GameObject hitEffect;
        public float speed = 2f;
        public virtual void Start()
        {
            attackCount = 0;
            GetComponent<Collider2D>().enabled = true;
            anim = GetComponent<Animator>();
            body = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
            
            StartCoroutine(WaitAndDestroy());
            // TODO: alternatively, destroy when exceeding the viewport
        }

        private void Update()
        {
            body.MovePosition(transform.position + transform.right * (transform.localScale.x * (Time.deltaTime * speed)));
        }

        public override IEnumerator WaitAndDestroy()
        {
            for (int i = 0; i < attackTimeByFrame; i++)
            {
                yield return null;
            }

            Destroy(gameObject);
        }

        public override void Hitting() {
            body.bodyType = RigidbodyType2D.Static;
            collider.enabled = false;
            Instantiate(hitEffect, transform.position + Vector3.up * 0.1f, Quaternion.identity);
            Destroy(gameObject);
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Enemy")) {
                return;
            }
            if (collision.gameObject.CompareTag("PlayerProjectile") || collision.gameObject.CompareTag("EnemyProjectile")) {
                return;
            }
            Hitting();
        }
    }
}