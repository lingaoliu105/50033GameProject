using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    public class PlayerSpriteRenderer: MonoBehaviour {
        public Vector2 position;
        public Facings facing;
        public SpriteRenderer spriteRenderer;
        private Vector2 offset;
        private Animator animator;
        public float SpeedX;
        public float SpeedY;
        public bool Ducking;
        public bool Land;
        public void SetSprite(Sprite sprite) {
            
        }
        public void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }
        public void Update() {
            
            // transform.position = position;
            if (facing == Facings.Left) {
                spriteRenderer.flipX = true;        
            } else {
                spriteRenderer.flipX = false;
            }
            animator.SetFloat("SpeedX", SpeedX);
            animator.SetFloat("SpeedY", SpeedY);
            animator.SetBool("Land", Land);
            animator.SetBool("Ducking", Ducking);
        }

        public void SetTrigger(String trigger) {
            animator.SetTrigger(trigger);
        }

        public void SetFloat(String name, float value) {
            animator.SetFloat(name, value);
        }

        public void SetBool(String name, bool value) {
            animator.SetBool(name, value);
        }


    }
}
