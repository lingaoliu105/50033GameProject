using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Enemy {
    public class MeleeAttack : EnemyAttack, IEffect {
        
        public bool hasHit = false;

        public void SelfDestroy() {
            Destroy(gameObject);
        }

        public override IEnumerator WaitAndDestroy() {
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }
        
        public void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                attackAudio.PlayOneShot(hitAudio);
            }
        }

        public override void Hitting() {
            attackAudio.PlayOneShot(hitAudio);
            hasHit = true;
        }

        public override void OnCollisionEnter2D(Collision2D collision) {
            return;
        }

        public override bool CheckActive()
        {
            return !hasHit;
        }
    }
}
