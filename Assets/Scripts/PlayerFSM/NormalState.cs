﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    public class NormalState : BaseActionState {
        public NormalState(PlayerController controller) : base(EActionState.Normal, controller) {
        }

        public override IEnumerator Coroutine() {
            throw new NotImplementedException();
        }

        public override bool IsCoroutine() {
            return false;
        }
         
        public override void OnBegin() {
            this.player.MaxFall = Constants.MaxFall;
        }

        public override void OnEnd() {
        }

        public override EActionState Update(float deltaTime) {
            #region 攀爬
            //Climb
            if (GameInput.GrabButton.Checked() && !player.Ducking) {
                //Climbing
                if (player.Speed.y <= 0 && Math.Sign(player.Speed.x) != -(int)player.Facing) {
                    if (player.inLadderArea) {
                        return EActionState.Ladder;
                    }
                    if (player.ClimbCheck((int)player.Facing)) {
                        player.Ducking = false;
                        return EActionState.Climb;
                    }
                    //非下坠情况，需要考虑向上攀爬吸附
                    if (player.MoveY > -1) {
                        bool snapped = player.ClimbUpSnap();
                        if (snapped) {
                            player.Ducking = false;
                            return EActionState.Climb;
                        }
                    }
                }
            }
            #endregion
            #region 鸭子
            if (player.Ducking) {
                if (player.OnGround && player.MoveY != -1) {
                    if (player.CanUnDuck) {
                        player.Ducking = false;
                    } else if (player.Speed.x == 0) {
                        //根据角落位置，进行挤出操作
                    }
                }
            } else if (player.OnGround && player.MoveY == -1 && player.Speed.y <= 0) {
                player.Ducking = true;
                Debug.Log("Ducking");
                player.PlayDuck(true);
            }
            #endregion
            #region 地面
            if (player.Ducking && player.OnGround) {
                
                player.Speed.x = Mathf.MoveTowards(player.Speed.x, Constants.DuckSpeed * this.player.MoveX, Constants.DuckFriction * deltaTime);
            } 
            else {
                float mult = player.OnGround ? 1 : Constants.AirMult;
                float maxRun = player.Holding == null ? Constants.HoldingMaxRun : Constants.MaxRun;
                if (Math.Abs(player.Speed.x) > maxRun && Math.Sign(player.Speed.x) == this.player.MoveX) {
                    //同方向加速
                    player.Speed.x = Mathf.MoveTowards(player.Speed.x, maxRun * this.player.MoveX, Constants.RunReduce * mult * deltaTime);
                }
                else {
                      //不同方向加速
                    player.Speed.x = Mathf.MoveTowards(player.Speed.x, maxRun*this.player.MoveX, Constants.RunAccel*mult*deltaTime);
                }
            }
            #endregion
            #region 空中移动
            if (player.WasOnGround && !player.OnGround) {
                player.PlayAnimation("Jump");
            }
            float maxFallSpeed = Constants.MaxFall;
            float fastMaxFallSpeed = Constants.FastMaxFall;
            if (player.MoveY == -1 && player.Speed.y <= maxFallSpeed) {
                player.MaxFall = Mathf.MoveTowards(player.MaxFall, fastMaxFallSpeed, Constants.FastMaxAccel * deltaTime);
                //TODO: effect

            } else { 
                this.player.MaxFall = Mathf.MoveTowards(this.player.MaxFall, maxFallSpeed, Constants.FastMaxAccel * deltaTime);
            }
            if (!player.OnGround) { 
                float max = this.player.MaxFall;
                if ((player.MoveX == (int)player.Facing || (player.MoveX == 0 && GameInput.GrabButton.Checked())) && player.MoveY != -1) {
                    //判断是否向下做Wall滑行
                    if (player.Speed.y <= 0 && player.WallSlideTimer > 0 && player.ClimbBoundsCheck((int)player.Facing) && player.CollideCheck(player.Position, Vector2.right * (int)player.Facing) && player.CanUnDuck) {
                        player.Ducking = false;
                        player.WallSlideDir = (int)player.Facing;
                    }
                    if (player.WallSlideDir != 0) {
                        max = Mathf.Lerp(Constants.MaxFall, Constants.WallSlideStartMax, player.WallSlideTimer / Constants.WallSlideTime);
                        //TODO: effect
                        player.PlayAnimation("Slip");
                    } else player.PlayAnimation("Jump");
                }

                
                float mult = (Math.Abs(player.Speed.y) < Constants.HalfGravThreshold && (GameInput.JumpButton.Checked())) ? .5f : 1f;
                player.Speed.y = Mathf.MoveTowards(player.Speed.y, max, Constants.Gravity * mult * deltaTime);
            }
            if (player.VarJumpTimer > 0) {
                if (GameInput.JumpButton.Checked()) {
                    //如果按住跳跃，则跳跃速度不受重力影响。
                    player.Speed.y = Math.Max(player.Speed.y, player.VarJumpSpeed);

                } else
                    player.VarJumpTimer = 0;
            }

            #endregion
            #region 冲刺
            if (player.CanDash) {
                return player.Dash();
            }
            #endregion
            #region 跳跃
            if (GameInput.JumpButton.Pressed()) {
                if (this.player.JumpCheck.AllowJump()) {
                    this.player.Jump();
                } else if (player.CanUnDuck) { 
                    if (player.WallJumpCheck(1)) {
                        if (player.Facing == Facings.Right && GameInput.GrabButton.Checked()) {
                            player.ClimbJump();
                        } else {
                            player.WallJump(-1);
                        }
                    } else if (player.WallJumpCheck(-1)) {
                        if (player.Facing == Facings.Left && GameInput.GrabButton.Checked()) {
                            player.ClimbJump();
                        } else {
                            player.WallJump(1);
                        }
                    }
                }
            }
            #endregion
            #region 攻击
            if (player.CanAttack) {
                return player.Attack(player.CurrentMeleeWeapon);
            }
            if (player.CanShoot) {
                return player.Shoot(player.CurrentRangedWeapon);
            }
            if (player.CanConsume) {
                player.UseTube();
            }
            #endregion


            return EActionState.Normal;
        }
    }
}
