using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    public class PlayerSpriteRenderer: Singleton<PlayerSpriteRenderer> {
        public Vector2 position;
        public Facings facing;
        public SpriteRenderer spriteRenderer;
        public Vector2 cameraPos;
        public void SetSprite(Sprite sprite) {
            
        }
        public void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        public void Update() {
            transform.position = position;
            if (facing == Facings.Left) {
                spriteRenderer.flipX = true;        
            } else {
                spriteRenderer.flipX = false;
            }
        }
    }
}
