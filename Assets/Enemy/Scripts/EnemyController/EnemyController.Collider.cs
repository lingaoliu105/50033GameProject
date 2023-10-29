using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Game { 
    public partial class EnemyController {
        
        // TODO: confirm ground layer number
        private int groundMask = 1;
        private Vector2 boundingBoxSize = new Vector2(0.3f,0.1f);

        const float DEVIATION = 0.02f;  //碰撞检测误差

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
        if (other.gameObject.CompareTag("Player"))
        {
            targetPlayer = other.gameObject;
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (targetPlayer && other.gameObject == targetPlayer.gameObject)
        {
            targetPlayer = null;
        }
    }

}
}


