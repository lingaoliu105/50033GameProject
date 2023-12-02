using Enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy {
    public class StillAttackContinuous : EnemyAttack {
        public void Start() {
            StartCoroutine(WaitAndDestroy());
        }
        public override void Hitting() {
            this.GetComponent<Collider2D>().enabled = false;
            return;
        }

        public override void OnCollisionEnter2D(Collision2D collision) {
            return;
        }
        public void OnTriggerEnter2D(Collider2D collision) {
            return;
        }

        public override IEnumerator WaitAndDestroy() {
            yield return null;
        }
    }
}
