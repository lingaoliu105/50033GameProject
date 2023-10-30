using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    public class BaseEnemyAttack : MonoBehaviour, IAttack {
        [Header("伤害")]
        public int attackDamage = 1;
        [Header("存在时间")]
        public int attackTimeByFrame = 60;
        [Header("进行攻击（指触碰时是否会造成伤害）")]
        public bool isAttacking = false;
        [Header("造成伤害x次后时会删除自身")]
        public int destroyAfterAttack = -1;
        [Header("造成伤害x次后时会禁用攻击")]
        public int disableAfterAttack = 1;
        private int attackCount = 0;

        public virtual void Start() {
            attackCount = 0;
            isAttacking = true;
            GetComponent<Collider2D>().enabled = true;
            StartCoroutine(WaitAndDestroy());
        }

        public IEnumerator WaitAndDestroy() {
            for (int i = 0; i < attackTimeByFrame; i++) {
                yield return null;
            }
            Destroy(gameObject);
        }


        public void OnCollisionEnter2D(Collision2D collision) {
            if (isAttacking) {
                if (collision.gameObject.tag == "Player") {
                    //Debug.Log("Deal " + attackDamage + " damage to " + collision.gameObject.name);
                    //collision.gameObject.GetComponent<PlayerController>().TakeDamage(attackDamage);
                    attackCount++;
                    if (attackCount>=destroyAfterAttack&&destroyAfterAttack>0) {
                        Destroy(gameObject);
                    }
                    if (attackCount >= disableAfterAttack&& destroyAfterAttack > 0) {
                        isAttacking = false;
                        this.GetComponent<Collider2D>().enabled = false;
                    }

                }
            }
        }
    }
}
