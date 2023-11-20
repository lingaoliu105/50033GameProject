using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Attack;
using Game;

namespace Enemy {
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
        protected float hp;
        public float maxHP = 100;
        public float dieWaitTime = 0.8f;
        public float detectRange = 8f;
        public int SoulAmount = 100;

        protected SpriteRenderer sprite;
        protected CapsuleCollider2D bodyCollider;
        protected Animator animator;
        public GameObject targetPlayer;
        protected Rigidbody2D body;
        protected Slider healthBar;
        protected AudioSource hitAudio;
        
        // for dev only
        public Vector3 playerPosition = new Vector3(0, 0);
        public GameObject player;


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

        protected virtual void FixedUpdate()
        {
            animator.SetFloat("xSpeed", MathF.Abs(body.velocity.x));
            sprite.flipX = facing != Facings.Right;
        }

        public override void Start() {
            base.Start();
            sprite = GetComponentInChildren<SpriteRenderer>();
            bodyCollider = GetComponent<CapsuleCollider2D>();
            animator = GetComponentInChildren<Animator>();
            body = GetComponent<Rigidbody2D>();
            healthBar = GetComponentInChildren<Slider>();
            hitAudio = GetComponent<AudioSource>();
            
            hp = maxHP;
            healthBar.maxValue = hp;
            healthBar.value = hp;
            player = GameObject.FindGameObjectWithTag("Player");
            GameRestart(); // clear powerup in the beginning, go to start state
        }

        private IEnumerator WaitAndDestroy() {
            bodyCollider.enabled = false;
            body.bodyType = RigidbodyType2D.Static;
            yield return new WaitForSeconds(dieWaitTime);
            Destroy(gameObject);
        }

        public override void Update()
        {
            base.Update();
            DetectPlayer();
        }

        protected void DetectPlayer()
        {
            if ((GetPlayerPosition() - transform.position).magnitude < detectRange)
            {
                targetPlayer = GetPlayer();
            }
            else
            {
                targetPlayer = null;
            }
        }

        public GameObject GetPlayer()
        {
            return GameObject.FindGameObjectWithTag("Player");
        }

        public Vector3 GetPlayerPosition()
        {
            return GameObject.FindGameObjectWithTag("Player").transform.position;
        }

        public void TakeDamage(int damage) {
            hp -= damage;
            healthBar.value = hp;
            if (hp <= 0) {
                PlayDeadAnimation();
                GetPlayer().GetComponent<PlayerController>().GainSoul(SoulAmount);
                StartCoroutine(WaitAndDestroy());
            } else
            {
                HitFlash();
            }
        }

        private void PlayHitSound()
        {
            hitAudio.PlayOneShot(hitAudio.clip);
        }

        // this should be added to the GameRestart EventListener as callback
        public void GameRestart() {
            // set the start state
            TransitionToState(startState);
        }

    }
}
