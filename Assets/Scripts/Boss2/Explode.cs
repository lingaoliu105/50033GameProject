using Enemy;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy {
    public class Explode : EnemyAttack {
        public float explosionRadius = 3f;
        public float explosionTime = 1f;

        private Animator animator;
        public void Start() {
            animator = GetComponent<Animator>();
            transform.localScale = new Vector3(explosionRadius, explosionRadius, 1);
            animator.SetFloat("ExplosionSpeed", 1 / explosionTime);
            animator.SetTrigger("Explode");
            StartCoroutine(WaitAndDestroy());
        }
        public override void Hitting() {
            this.GetComponent<Collider2D>().enabled = false;
            return;
        }

        public override void OnCollisionEnter2D(Collision2D collision) {
            return;
        }


        public override IEnumerator WaitAndDestroy() {

            yield return new WaitForSeconds(explosionTime);
            Destroy(gameObject);
        }
    }
}
