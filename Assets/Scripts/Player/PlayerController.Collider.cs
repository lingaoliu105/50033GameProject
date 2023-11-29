using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Enemy;
using Assets.Scripts.Map;
using Assets.Scripts.Items;

namespace Game {
    public partial class PlayerController {
        

        const float STEP = 0.1f;  //碰撞检测步长，对POINT检测用
        const float DEVIATION = 0.02f;  //碰撞检测误差

        private readonly Rect normalHitbox = new Rect(0, -0.25f, 0.8f, 1.1f);
        private readonly Rect duckHitbox = new Rect(0, -0.5f, 0.8f, 0.6f);
        private readonly Rect normalHurtbox = new Rect(0f, -0.15f, 0.8f, 0.9f);
        private readonly Rect duckHurtbox = new Rect(8f, 4f, 0.8f, 0.4f);

        private Rect collider;

        void CheckForSomeObject() {
            // 执行矩形区域检测
            Collider2D[] colliders = Physics2D.OverlapBoxAll(this.Position + collider.position, collider.size, 0);

            // 遍历检测到的碰撞器
            foreach (Collider2D collider in colliders) {
                // 检查标签是否为 "EnemyProjectile"
                if (collider.CompareTag("EnemyProjectile")) {
                    // 处理敌方投射物的逻辑
                    var attackInstance = collider.GetComponent<EnemyAttack>();
                    if (attackInstance && !attackInstance.hasHit) // prevent multiple hit caused
                    {
                        int damage = collider.GetComponent<EnemyAttack>().Damage;
                        this.TakeDamage(damage);
                        collider.GetComponent<EnemyAttack>().Hitting();
                        attackInstance.hasHit = true;
                    }
                }
                if (collider.CompareTag("Void")) { 
                    collider.GetComponent<VoidArea>().GetOut(this);
                }
                if (collider.CompareTag("Object")) {
                    Debug.Log("Object");
                    collider.GetComponent<ItemObject>().PickedUP(this);
                }
            }
        }


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

        public bool ClimbHopBlockedCheck() {
            if (CollideCheck(Position, Vector2.up, 0.6f))
                return true;
            return false;
        }

        public bool OverlapPoint(Vector2 position) {
            return Physics2D.OverlapPoint(position, groundMask);
        }

        public bool ClimbCheck(int dir, float yAdd = 0) {
            //获取当前的碰撞体
            Vector2 origion = this.Position + collider.position;
            if (Physics2D.OverlapBox(origion + Vector2.up * (float)yAdd + Vector2.right * dir * (Constants.ClimbCheckDist * 0.1f + DEVIATION), collider.size, 0, groundMask)) {
                return true;
            }
            return false;
        }

        public bool SlipCheck(float addY = 0) {
            int direct = Facing == Facings.Right ? 1 : -1;
            Vector2 origin = this.Position + collider.position + Vector2.up * collider.size.y / 2f + Vector2.right * direct * (collider.size.x / 2f + STEP);
            Vector2 point1 = origin + Vector2.up * (-0.4f + addY);

            if (Physics2D.OverlapPoint(point1, groundMask)) {
                return false;
            }
            Vector2 point2 = origin + Vector2.up * (0.4f + addY);
            if (Physics2D.OverlapPoint(point2, groundMask)) {
                return false;
            }
            return true;
        }

        public RaycastHit2D CollideClimbHop(int dir) {
            Vector2 origion = this.Position + collider.position;
            RaycastHit2D hit = Physics2D.BoxCast(Position, collider.size, 0, Vector2.right * dir, DEVIATION, groundMask);
            return hit;
            //if (hit && hit.normal.x == -dir)
            //{

            //}
        }

        //攀爬时，向上吸附
        public bool ClimbUpSnap() {
            for (int i = 1; i <= Constants.ClimbUpCheckDist; i++) {
                //检测上方是否存在可以攀爬的墙壁，如果存在则瞬移i个像素
                float yOffset = i * 0.1f;
                if (!CollideCheck(this.Position, Vector2.up, yOffset) && ClimbCheck((int)Facing, yOffset + DEVIATION)) {
                    this.Position += Vector2.up * yOffset;
                    Debug.Log($"======Climb Correct");
                    return true;
                }
            }
            return false;
        }

        public void ClimbSnap() {
            Vector2 origion = this.Position + collider.position;
            Vector2 dir = Vector2.right * (int)this.Facing;
            RaycastHit2D hit = Physics2D.BoxCast(origion, collider.size, 0, dir, Constants.ClimbCheckDist * 0.1f + DEVIATION, groundMask);
            if (hit) {
                //如果发生碰撞,则移动距离
                this.Position += dir * Mathf.Max((hit.distance - DEVIATION), 0);
            }
            //for (int i = 0; i < Constants.ClimbCheckDist; i++)
            //{
            //    Vector2 dir = Vector2.right * (int)ctx.Facing;
            //    if (!ctx.CollideCheck(ctx.Position, dir))
            //    {
            //        ctx.AdjustPosition(dir * 0.1f);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
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

        //墙壁上跳检测
        public bool WallJumpCheck(int dir) {
            return ClimbBoundsCheck(dir) && this.CollideCheck(Position, Vector2.right * dir, Constants.WallJumpCheckDist);
        }

        //根据整个关卡的边缘框进行检测,确保人物在关卡的框内.
        public bool ClimbBoundsCheck(int dir) {
            return true;
            //return base.Left + (float)(dir * 2) >= (float)this.level.Bounds.Left && base.Right + (float)(dir * 2) < (float)this.level.Bounds.Right;
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

            if ((this.stateMachine.State == (int)EActionState.Dash)) {
                if (onGround && DuckFreeAt(Position + Vector2.right * distX)) {
                    Ducking = true;
                    return true;
                } else if (this.Speed.y == 0 && this.Speed.x != 0) {
                    for (int i =1; i<= Constants.DashCornerCorrection; i++) {
                        for (int j = 1; j >= -1; j -= 2) {
                            if (!CollideCheck(Position + new Vector2(0, j * i * 0.1f), direct, Mathf.Abs(distX))) {
                                this.Position += new Vector2(distX, j * i * 0.1f);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        private bool CorrectY(float distY) {
            Vector2 origion = this.Position + collider.position;
            Vector2 direct = Math.Sign(distY) > 0 ? Vector2.up : Vector2.down;
            if (this.Speed.y < 0) {
                if ((this.stateMachine.State == (int)EActionState.Dash) && !DashStartedOnGround) {
                    if (this.Speed.x <= 0) {
                        for (int i = -1; i >= -Constants.DashCornerCorrection; i--) {
                            float step = (Mathf.Abs(i * 0.1f) + DEVIATION);
                            if (!CheckGround(new Vector2(-step, 0))) {
                                this.Position += new Vector2(-step, distY);
                                return true;
                            }
                        }
                    }
                    if (this.Speed.x >= 0) {
                        for (int i = 1; i <= Constants.DashCornerCorrection; i++) {
                            float step = (Mathf.Abs(i * 0.1f) + DEVIATION);
                            if (!CheckGround(new Vector2(step, 0))) {
                                this.Position += new Vector2(step, distY);
                                return true;
                            }
                        }
                    }
                }
            } else if (this.Speed.y > 0) {
                if (this.Speed.x <= 0) {
                    for (int i = 1; i <= Constants.UpwardCornerCorrection; i++) {
                        RaycastHit2D hit = Physics2D.BoxCast(origion + new Vector2(-i * 0.1f, 0), collider.size, 0, direct, Mathf.Abs(distY) + DEVIATION, groundMask);
                        if (!hit) {
                            this.Position += new Vector2(-i * 0.1f, 0);
                            return true;
                        }
                    }
                }
                if (this.Speed.x >= 0) {
                    for (int i = 1; i <= Constants.UpwardCornerCorrection; i++) {
                        RaycastHit2D hit = Physics2D.BoxCast(origion + new Vector2(i * 0.1f, 0), collider.size, 0, direct, Mathf.Abs(distY) + DEVIATION, groundMask);
                        if (!hit) {
                            this.Position += new Vector2(i * 0.1f, 0);
                            return true;
                        }
                    }
                }
            }
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
