using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace EnemyPro{

    public enum AutoActionState {
        None = 0,
        Patrol = 1,
        Scout = 2,
        Attack = 3,
    }
    public partial class EnemyControllerPro {

        public void StartAttackOneShot(int i) {
            StartCoroutine(AttackOneShot(i));
        }

        public IEnumerator AttackOneShot(int i) {
            PlayAttackAnimate();
            attackInstance[0] = Instantiate(attackTemplate[i], transform.position, Quaternion.identity);
            attackInstance[0].transform.parent = transform;
            for (int t = 0; t < attackTimeByFrame; t++) {
                yield return null;
            }
        }
        public void UpdateVelocity(float deltaTime) {
            float mult = isGrounded ? 1 : Game.Constants.AirMult;
            float maxRun = velocityX;
            if (Math.Abs(velocity.x) > maxRun && Math.Sign(velocity.x) == (int)moveDirectionX) {
                velocity.x = Mathf.MoveTowards(velocity.x, maxRun * (int)moveDirectionX, decelerationX * mult * deltaTime);
            } else {
                velocity.x = Mathf.MoveTowards(velocity.x, maxRun * (int)moveDirectionX, accelerationX * mult * deltaTime);
            }
            float maxFall = Game.Constants.MaxFall;
            if (!gravity) {
                //TODO：飞行
            } else {
                if (!isGrounded) {
                    velocity.y = Mathf.MoveTowards(velocity.y, maxFall, Game.Constants.Gravity * deltaTime);
                }
            }
            

        }
        public void PerformJump() {
            if (isGrounded && !isJumping) {
                StartCoroutine(Jump());
                isJumping = true;
            }
            
        }
        public IEnumerator Jump() {
            for (int t = 0; t < jumpTimeByFrames; t++) {
                velocity.y = jumpspeedY;
                velocity.x += jumpspeedX * (int)moveDirectionX;
                yield return null;
            }
            isJumping = false;
            if (!gravity) {
                velocity = Vector2.zero;
            }
        }

        public void PerformMove(MoveDirectionX move) {
            moveDirectionX = move;
            if (move != MoveDirectionX.None) {
                facing = (Facings)move;
            }
        }
        public void ForceMovement(Vector2 direction, float speed = -1, float mult = -1) {
            direction.Normalize();
            if (speed < 0) {
                speed = forceMovementRate;
            }
            if (mult < 0) {
                mult = forceMovementMult;
            }
            forceMovement = direction * speed;
            StartCoroutine(forceMovementMulting(mult));
        }
        private IEnumerator forceMovementMulting(float mult) {
            while (forceMovement.magnitude > 0.05) {
                forceMovement *= mult;
                yield return null;
            }
        }

        public void SetPatrolRange(float L, float R, float U, float D) {
            patrolRangeD = D;
            patrolRangeL = L;
            patrolRangeR = R;
            patrolRangeU = U;
        }
        public void StartPatrol(float speed = -1) {
            if (speed > 0) {
                velocityX = speed;
            }
            autoAction = AutoActionState.Patrol;
        }
        public void StopPatrol() {
            autoAction = AutoActionState.None;
        }
        public void StartScout(float speed = -1) {
            if (speed > 0) {
                velocityX = speed;
            }
            autoAction = AutoActionState.Scout;
        }
        public void StopScout() {
            autoAction = AutoActionState.None;
        }
        public void StartAttack() {
            autoAction = AutoActionState.Attack;
        }
        public void StopAttack() {
            autoAction = AutoActionState.None;
        }

        private void AutoMoveUpdate() {
            bool obs = DetectObstacle();
            bool wall = DetectWall();
            bool ground = DetectGround();
            switch (autoAction) {
                case AutoActionState.Patrol: {
                        if (position.x < patrolRangeL) {
                            moveDirectionX = MoveDirectionX.Right;
                        }
                        if (position.x > patrolRangeR) {
                            moveDirectionX = MoveDirectionX.Left;
                        }
                        if (position.x < patrolRangeL || position.x > patrolRangeR) {
                            if (obs) {
                                if (!wall) {
                                    PerformJump();
                                } else {
                                    moveDirectionX = MoveDirectionX.None;
                                }
                            } else if (!ground) {
                                if (!wall) {
                                    PerformJump();
                                } else {
                                    moveDirectionX = MoveDirectionX.None;
                                }
                            }
                            break;
                        }
                        if (moveDirectionX == MoveDirectionX.None) {
                            // Random -1 or 1
                            moveDirectionX = (MoveDirectionX)(UnityEngine.Random.Range(0, 2) * 2 - 1);
                        }
                        if (obs || !ground) {

                            moveDirectionX = (MoveDirectionX)(-(int)moveDirectionX);
                        }
                        break;
                    }
                case AutoActionState.Scout: {
                        if (position.x <= playerPos.x) {
                            moveDirectionX = MoveDirectionX.Right;
                        }
                        if (position.x > playerPos.x) {
                            moveDirectionX = MoveDirectionX.Left;
                        }
                        if (obs) {
                            if (!wall) {
                                PerformJump();
                            } else {
                                moveDirectionX = MoveDirectionX.None;
                            }
                        } else if (!ground) {
                            Debug.Log("Ground");
                            if (!wall) {
                                PerformJump();
                            } else {
                                moveDirectionX = MoveDirectionX.None;
                            }
                        }
                    }
                    break;
                case AutoActionState.Attack:
                    moveDirectionX = MoveDirectionX.None;
                    facing = (Facings)(playerPos.x < position.x ? -1 : 1);
                    velocity.x = 0;
                    break;
                default:
                    break;
            }
        
        }



        private bool DetectGround() {
            Vector2 origin = position + Vector2.right * (int)moveDirectionX * offsetX + Vector2.down * boundingBoxSize.y * 1 / 2;
            Vector2 direction = Vector2.down;
            float raycastDistance = offsetY;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, raycastDistance, groundMask);
            Debug.DrawRay(origin, direction * raycastDistance, Color.red);
            if (hit) {
                if (hit.normal == Vector2.up) {
                    return true;
                }
            }
            return false;
        }
        private bool DetectObstacle() {
            Vector2 origin = position + Vector2.right * (int)moveDirectionX * offsetX + Vector2.up * boundingBoxSize.y * 1 / 2;
            Vector2 direction = Vector2.right * (int)moveDirectionX;
            float raycastDistance = boundingBoxSize.x * offsetRange;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, raycastDistance, groundMask);
            Debug.DrawRay(origin, direction * raycastDistance, Color.red);
            if (hit) {
                if (hit.normal == Vector2.right * (int)moveDirectionX) {
                    return true;
                }
            }
            return false;
        }
        private bool DetectWall() {
            Vector2 origin = position + Vector2.up * offsetY + Vector2.up * boundingBoxSize.y * 1 / 2;
            Vector2 direction = Vector2.right * (int)moveDirectionX;
            float raycastDistance = boundingBoxSize.x * offsetRange;
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, raycastDistance, groundMask);
            Debug.DrawRay(origin, direction * raycastDistance, Color.red);
            if (hit) {
                if (hit.normal == Vector2.right * (int)moveDirectionX) {
                    return true;
                }
            }
            return false;
        }
    }
}
