using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor.Animations;
using Enemy;

namespace Assets.Scripts.Enemy {
    public class ResistingExplosion: EnemyAttack {
        private int BeforeExplotionWarningFrames = 30;
        public int BeforeExplotionAnimationFrames;
        public int ExplotionColliderFramesAfterAnimationBegin;
        public int ExplotionFrames;
        public int AfterExplotionAnimationFrames;
        public int ExplotionColliderFramesAfterEndAnimationBegin;
        public GameObject ExplosionRenderer;
        public GameObject warningArea;
        private Animator animator;
        private Collider2D collider2d;
        public int ExplosionDamage;
        public int ExplotionMaxDamageFrames;
        public int ResistingAreaDamage;

        public IEnumerator Warning() {
            yield return null;
            warningArea.GetComponent<WarningAreaFlashing>().Show();
            yield return new WaitForSeconds(BeforeExplotionWarningFrames / 60f);
            StartCoroutine(ExplotionAnimation());
            StartCoroutine(ExplotionColliderEnable());
        }
        public IEnumerator ExplotionAnimation() { 
            animator.SetTrigger("Start");
            yield return new WaitForSeconds(BeforeExplotionAnimationFrames / 60f);
            yield return new WaitForSeconds(ExplotionFrames / 60f);
            animator.SetTrigger("End");
            StartCoroutine(ExplotionColliderDisable());
            yield return new WaitForSeconds(AfterExplotionAnimationFrames / 60f);
            Destroy(gameObject);
        }

        public IEnumerator ExplotionColliderEnable() {
            yield return new WaitForSeconds(ExplotionColliderFramesAfterAnimationBegin / 60f);
            collider2d.enabled = true;
            StartCoroutine(DamageDecreasing());
        }

        public IEnumerator ExplotionColliderDisable() {
            yield return new WaitForSeconds(ExplotionColliderFramesAfterEndAnimationBegin / 60f);
            collider2d.enabled = false;
        }

        public IEnumerator DamageDecreasing() {
            Damage = ExplosionDamage;
            yield return new WaitForSeconds(ExplotionMaxDamageFrames / 60f);
            Damage = ResistingAreaDamage;
        }

        public void Start() {
            animator = ExplosionRenderer.GetComponent<Animator>();
            collider2d = GetComponent<Collider2D>();
            collider2d.enabled = false;
            
            StartCoroutine(Warning());
        }

        public override IEnumerator WaitAndDestroy() {
            yield return null;
        }

        public override void Hitting() {
            return;
        }

        public override void OnCollisionEnter2D(Collision2D collision) {
            return;
        }
    }
    

}
