using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class BaseEnemyAttack : MonoBehaviour, IAttack
    {
        [Header("伤害")] public int attackDamage = 1;
        [Header("存在时间")] public int attackTimeByFrame = 60;

        private int attackCount = 0;
        protected Animator anim;
        protected Rigidbody2D body;
        protected Collider2D collider;
        public GameObject hitEffect;
        public virtual void Start()
        {
            attackCount = 0;
            GetComponent<Collider2D>().enabled = true;
            anim = GetComponent<Animator>();
            body = GetComponent<Rigidbody2D>();
            collider = GetComponent<Collider2D>();
            
            // StartCoroutine(WaitAndDestroy());
            // TODO: alternatively, destroy when exceeding the viewport
        }

        public IEnumerator WaitAndDestroy()
        {
            for (int i = 0; i < attackTimeByFrame; i++)
            {
                yield return null;
            }

            Destroy(gameObject);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            body.bodyType = RigidbodyType2D.Static;
            collider.enabled = false;
            Instantiate(hitEffect, transform.position + Vector3.up*0.1f, Quaternion.identity);
            if (collision.gameObject.CompareTag("Player"))
            {
                //Debug.Log("Deal " + attackDamage + " damage to " + collision.gameObject.name);
                //collision.gameObject.GetComponent<PlayerController>().TakeDamage(attackDamage);

            }
            Destroy(gameObject);
        }
    }
}