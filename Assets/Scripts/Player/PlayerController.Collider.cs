using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    public partial class PlayerController {
        

        const float STEP = 0.1f;  //碰撞检测步长，对POINT检测用
        const float DEVIATION = 0.02f;  //碰撞检测误差

        private readonly Rect normalHitbox = new Rect(0, -0.25f, 0.8f, 1.1f);
        private readonly Rect duckHitbox = new Rect(0, -0.5f, 0.8f, 0.6f);
        private readonly Rect normalHurtbox = new Rect(0f, -0.15f, 0.8f, 0.9f);
        private readonly Rect duckHurtbox = new Rect(8f, 4f, 0.8f, 0.4f);

        private Rect collider;


        private bool CheckGround() {
            return CheckGround(Vector2.zero);
        }
        //针对横向,进行碰撞检测.如果发生碰撞,
        private bool CheckGround(Vector2 offset) {
            Vector2 origion = this.Position + collider.position + offset;
            RaycastHit2D hit = Physics2D.BoxCast(origion, collider.size, 0, Vector2.down, DEVIATION, groundMask);
            if (hit && hit.normal == Vector2.up) {
                return true;
            }
            return false;
        }

        public bool CollideCheck(Vector2 position, Vector2 dir, float dist = 0) {
            Vector2 origion = position + collider.position;
            return Physics2D.OverlapBox(origion + dir * (dist + DEVIATION), collider.size, 0, groundMask);
        }

        public void PlayDuck(bool enable) {
            if (enable) {
                //SpriteControl.Scale(new Vector2(1.4f, .6f));
                //SpriteControl.SetSpriteScale(DUCK_SPRITE_SCALE);
            } else {
                if (this.OnGround && MoveY != 1) {
                    //SpriteControl.Scale(new Vector2(.8f, 1.2f));
                }
                //SpriteControl.SetSpriteScale(NORMAL_SPRITE_SCALE);
            }
        }

        private void UpdateColliderX(float distX) {
            Vector2 targetPosition = this.Position;
            float distance = distX;
            int correctTimes = 1;
            while (true) {
                float moved = MoveXStepWithCollide(distance);
                this.Position += Vector2.right * moved;
                if (moved == distance || correctTimes == 0) {
                    break;
                }
                float tempDist = distance - moved;
                correctTimes--;
                if (!CorrectX(tempDist)) {
                    this.Speed.x = 0;//未完成校正，则速度清零

                    break;
                }
                distance = tempDist;
            }
        }

         

        private void UpdateColliderY(float distY) {
              Vector2 targetPosition = this.Position;
            bool collided = true;
            float distance = distY;
            int correctTimes = 1;
            while (true) {
                float moved = MoveYStepWithCollide(distance);
                this.Position += Vector2.up * moved;
                if (moved == distance || correctTimes == 0) {
                    collided = false;
                    break;
                }
                float tempDist = distance - moved;
                correctTimes--;
                if (!CorrectY(tempDist)) {
                    this.Speed.y = 0;//未完成校正，则速度清零
                    break;
                }
                distance = tempDist;
            }
            //落地时候，进行缩放
            if (collided && distY < 0) {
                //if (this.stateMachine.State != (int)EActionState.Climb) {
                    //this.PlayLandEffect(this.SpritePosition, speedY);
                //}
            }
        }

        private float MoveXStepWithCollide(float distX) {
            Vector2 moved = Vector2.zero;
            Vector2 direct = Math.Sign(distX) > 0 ? Vector2.right : Vector2.left;
            Vector2 origion = this.Position + collider.position;
            RaycastHit2D hit = Physics2D.BoxCast(origion, collider.size, 0, direct, Mathf.Abs(distX) + DEVIATION, groundMask);
            if (hit && hit.normal == -direct) {
                moved += direct * Mathf.Max((hit.distance - DEVIATION), 0);
            } else {
                moved += Vector2.right * distX;
            }
            return moved.x;
        }

        private float MoveYStepWithCollide(float distY) {
            Vector2 moved = Vector2.zero;
            Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;
            Vector2 origion = this.Position + collider.position;
            RaycastHit2D hit = Physics2D.BoxCast(origion, collider.size, 0, direct, Mathf.Abs(distY) + DEVIATION, groundMask);
            if (hit && hit.normal == -direct) {
                moved += direct * Mathf.Max((hit.distance - DEVIATION), 0);
            } else {
                moved += Vector2.up * distY;
            }
            return moved.y;
        }

        private bool CorrectX(float distX) {
            Vector2 origion = this.Position + collider.position;
            Vector2 direct = Math.Sign(distX) > 0 ? Vector2.right : Vector2.left;

            if (true) { }
            return false;
        }
        private bool CorrectY(float distY) {
            Vector2 origion = this.Position + collider.position;
            Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;

            if (true) { }
            return false;
        }

        void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Vector2 vector2 = this.Position + collider.position + Vector2.down * DEVIATION;
            Vector3 vector3 = new Vector3(vector2.x, vector2.y, 0);
            Gizmos.DrawCube(vector3, collider.size);
        }
    }
}
