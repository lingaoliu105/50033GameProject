using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy {
    public class CommonBullet : EnemyAttack {
        public Vector2 initialVelocity;
        public void Start() {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            StartCoroutine(WaitAndDestroy());
        }
        public override void Hitting() {
            Destroy(gameObject);
        }

        public override void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag == "Ground") {
                Destroy(gameObject);
            }
        }

        public override IEnumerator WaitAndDestroy() {
            yield return null;
            GetComponent<Rigidbody2D>().velocity = initialVelocity;
        }
    }
}
