using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Attack;

namespace Enemy { 
    public partial class EnemyController {
            
        // TODO: confirm ground layer number
        private int groundMask = 6;
        private Vector2 boundingBoxSize = new Vector2(0.3f,0.1f);

        const float DEVIATION = 0.02f;  //碰撞检测误差

        private int playerAttackLayer = 11;

    private bool CheckGround() {
        return CheckGround(Vector2.zero);
    }
    private bool CheckGround(Vector2 offset) {
        Vector2 origin = new Vector2(transform.position.x,transform.position.y) + offset;
        RaycastHit2D hit = Physics2D.BoxCast(origin, boundingBoxSize, 0, Vector2.down, DEVIATION, groundMask);
        return hit && hit.normal == Vector2.up;
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {   
        
    }
        public void OnCollisionEnter2D(Collision2D collision) {
            if (collision.gameObject.tag == "PlayerProjectile") {
                PlayHitSound();
                TakeDamage(collision.gameObject.GetComponent<BasicPlayerProjectile>().attackDamage);
            }
            if (collision.gameObject.tag == "Void") {
                TakeDamage(9999999);
            }
        }
    }
}


