using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Game { 
    public partial class EnemyController {

    const float DEVIATION = 0.02f;  //碰撞检测误差

    private bool CheckGround() {
        return CheckGround(Vector2.zero);
    }
    private bool CheckGround(Vector2 offset) {
        Vector2 origion = this.position + offset;
        RaycastHit2D hit = Physics2D.BoxCast(origion, boundingBoxSize, 0, Vector2.down, DEVIATION, groundMask);
        if (hit && hit.normal == Vector2.up) {
            return true;
        }
        return false;
    }

    public bool CollideCheck(Vector2 position, Vector2 dir, float dist = 0) {
        Vector2 origion = position;
        return Physics2D.OverlapBox(origion + dir * (dist + DEVIATION), boundingBoxSize, 0, groundMask);
    }

    private float UpdateColliderX(float distX) {
        Vector2 targetPosition = this.position;
        float distance = distX;
        int correctTimes = 1;
        while (true) {
            float moved = MoveXStepWithCollide(distance);
            this.position += Vector2.right * moved;
            if (moved == distance || correctTimes == 0) {
                break;
            }
            float tempDist = distance - moved;
            correctTimes--;
            velocity.x = 0;
            distance = tempDist;
        }
        return distance;
    }

    private float MoveXStepWithCollide(float distX) {
        Vector2 moved = Vector2.zero;
        Vector2 direct = Math.Sign(distX) > 0 ? Vector2.right : Vector2.left;
        Vector2 origion = this.position;
        RaycastHit2D hit = Physics2D.BoxCast(origion, boundingBoxSize, 0, direct, Mathf.Abs(distX) + DEVIATION, groundMask);
        if (hit && hit.normal == -direct) {
            moved += direct * Mathf.Max((hit.distance - DEVIATION), 0);
        } else {
            moved += Vector2.right * distX;
        }
        return moved.x;
    }

    private float UpdateColliderY(float distY) {
        Vector2 targetPosition = this.position;
        float distance = distY;
        int correctTimes = 1; 
        while (true) {
            float moved = MoveYStepWithCollide(distance);
            this.position += Vector2.up * moved;
            if (moved == distance || correctTimes == 0) {
                break;
            } 
            float tempDist = distance - moved;
            correctTimes--;
            velocity.y = 0;
            distance = tempDist;
        }
        return distance;
    }   

    private float MoveYStepWithCollide(float distY) {
        Vector2 moved = Vector2.zero;
        Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;
        Vector2 origion = this.position;
        RaycastHit2D hit = Physics2D.BoxCast(origion, boundingBoxSize, 0, direct, Mathf.Abs(distY) + DEVIATION, groundMask);
        if (hit && hit.normal == -direct) {
            moved += direct * Mathf.Max((hit.distance - DEVIATION), 0);
            
        } else {
            moved += Vector2.up * distY;
        }
        return moved.y;
    }

       

}
}


