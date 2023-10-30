using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
namespace Game
{
    public enum MoveDirectionX {
        Left = -1,
        Right = 1,
        None = 0
    }
    [Serializable]
    public class AdvancedEnemySettings
    {
        public float enemyAutoPathingEdgeOffsetY = 1f;
        public float enemyAutoPathingEdgeOffsetX = 0.2f;
        [Range(0,1)]
        public float enemyAutoPathingEdgeOffsetX2 = 0.9f;
    }
    public partial class EnemyController : StateController
    {
        public GameConstants gameConstants;
        protected float hp;
        public float dieWaitTime = 0.8f;

        protected SpriteRenderer sprite;
        protected CapsuleCollider2D bodyCollider;
        protected Animator animator;
        protected GameObject targetPlayer;
        protected Rigidbody2D body;

        public Vector2 position
        {
            get
            {
                return new Vector2(transform.position.x, transform.position.y);
            }
            set
            {
                transform.position = new Vector3(value.x, value.y, 0);
            }
        }

        private void FixedUpdate()
        {
            animator.SetFloat("xSpeed", MathF.Abs(body.velocity.x));
            sprite.flipX = facing != Facings.Right;
        }

        public override void Start() {
            base.Start();
            sprite = GetComponent<SpriteRenderer>();
            bodyCollider = GetComponent<CapsuleCollider2D>();
            animator = GetComponent<Animator>();
            body = GetComponent<Rigidbody2D>();
            GameRestart(); // clear powerup in the beginning, go to start state
        }

        private IEnumerator WaitAndDestroy() {
            bodyCollider.enabled = false;
            yield return new WaitForSeconds(dieWaitTime);
            Destroy(gameObject);
        }


        public void TakeDamage(int damage) {
            hp -= damage;
            if (hp <= 0) {
                PlayDeadAnimation();
                StartCoroutine(WaitAndDestroy());
            } else
            {
                HitFlash();
            }
        }

        public void PassAway() {
            Destroy(gameObject);
        }

        // this should be added to the GameRestart EventListener as callback
        public void GameRestart() {
            // set the start state
            TransitionToState(startState);
        }

    }
}
