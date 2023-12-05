using Assets.Scripts.Enemy;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Attack {
    public abstract class BasicPlayerProjectile : MonoBehaviour {
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
        public bool isProjectile = true;
        [Header("子弹速度")]
        public float speed = 15f;
        [Header("子弹方向")]
        public Vector2 direction;

        public bool hasHit = false;
        public float DeltaTime { get; set; }
        private new Collider2D collider;

        public void Start() {
            attackCount = 0;
            isAttacking = true;
            DeltaTime = 1f / 60f;
            collider = GetComponent<Collider2D>();
            collider.enabled = true;
            StartCoroutine(WaitAndDestroy());
        }

        public IEnumerator WaitAndDestroy() {
            for (int i = 0; i < attackTimeByFrame; i++) {
                yield return null;
            }
            Destroy(gameObject);
        }
        public void Update() {
            UpdateDeltaTime(DeltaTime);
        }

        public virtual void UpdateDeltaTime(float deltaTime) {
            if (!isProjectile) {
                return;
            }
            transform.position += (Vector3)direction * speed * deltaTime;
        }

        public virtual void  CollideWithEnemy(GameObject enemy) {
            attackCount++;
            // TODO: 伤害计算
            PlayerController player = GameObject.Find("Player").GetComponent<PlayerController>();
            if (player != null) {
                player.UpdateEquipsOnAttackHit();
            }
            if (enemy.GetComponent<BodyPartController>() != null) { 
                enemy.GetComponent<BodyPartController>().hit(attackDamage);
            }
            if (enemy.GetComponent<Boss1Controller>() != null) {
                enemy.GetComponent<Boss1Controller>().TakeDamage(attackDamage);
            }

            if (attackCount >= destroyAfterAttack && destroyAfterAttack > 0) {
                Destroy(gameObject);
            }
            if (attackCount >= disableAfterAttack && destroyAfterAttack > 0) {
                isAttacking = false;
                collider.enabled = false;
            }
        }
        public virtual void CollideWithScene(GameObject other) { 
            Destroy(gameObject);
         }

        public void OnTriggerEnter2D(Collider2D collision) {
            //Debug.Log("Collide with " + collision.gameObject + ", Tag = " + collision.gameObject.tag);
            if (isAttacking) {
                if (collision.gameObject.tag == "Enemy") {
                    CollideWithEnemy(collision.gameObject);
                }
                else if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "PlayerProjectile") {
                    CollideWithScene(collision.gameObject);
                }
            }
        }
    }
}
