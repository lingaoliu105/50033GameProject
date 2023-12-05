using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    public class ClimbLadderState : BaseActionState {
        public ClimbLadderState(PlayerController controller) : base(EActionState.Ladder, controller) {
        }

        public override IEnumerator Coroutine() {
            yield return null;
        }

        public override bool IsCoroutine() {
            return false;
        }

        public override void OnBegin() {
            player.Speed.x = 0;
            player.Speed.y *= Constants.ClimbGrabYMult;
            player.WallSlideTimer = Constants.WallSlideTime;
            player.WallBoost.ResetTime();
            player.ClimbNoMoveTimer = Constants.ClimbNoMoveTime;

            //player.ClimbSnap();
            player.ClimbLadderSnap();

            // player.PlayAnimation("Climb");
            player.SetBool("Climbing", true);
        }

        public override void OnEnd() {
            player.SetBool("Climbing", false);
        }

        public override EActionState Update(float deltaTime) {
            player.ClimbNoMoveTimer -= deltaTime;
            if (GameInput.JumpButton.Pressed() && (!player.Ducking || player.CanUnDuck)) {
                 player.ClimbJump();
                return EActionState.Normal;
            }
            if (player.CanDash) {
                return this.player.Dash();
            }
            if (!GameInput.GrabButton.Checked())
            {
                player.PlayAnimation("Jump");
                //Speed += LiftBoost;
                //Play(Sfxs.char_mad_grab_letgo);
                return EActionState.Normal;
            }
            if (!player.inLadderArea) {
                if (player.Speed.y < 0) {
                    //
                    //ClimbHop();
                }
                return EActionState.Normal;
            }
            float target = 0;
            bool trySlip = false;
            if (player.ClimbNoMoveTimer <= 0) {
                if (player.MoveY == 1) {
                    target = Constants.ClimbUpSpeed;
                    if (player.CollideCheck(player.Position, Vector2.up)) {
                        Debug.Log("=====ClimbSlip_Type1");
                        player.Speed.y = Mathf.Min(player.Speed.y, 0);
                        target = 0;
                        trySlip = true;
                    } 
                } else if (player.MoveY == -1) {
                    target = Constants.ClimbDownSpeed;
                    if (player.OnGround) {
                        player.Speed.y = Mathf.Max(player.Speed.y, 0);
                        target = 0;
                    } else {
                        // TODO: effect
                    }
                } else {
                    trySlip = true;
                }
                
            } else {
                trySlip = true;
            }
            if (trySlip && player.SlipCheck()) {
                Debug.Log("=======ClimbSlip_Type4");
                target = Constants.ClimbSlipSpeed;
            }
            player.Speed.y = Mathf.MoveTowards(player.Speed.y, target, Constants.ClimbAccel * deltaTime);
            //TrySlip导致的下滑在碰到底部的时候,停止下滑
            if (player.MoveY != -1 && player.Speed.y < 0 && !player.CollideCheck(player.Position, new Vector2((int)player.Facing, -1))) {
                player.Speed.y = 0;
            }
            //TODO Stamina
            return state;
        }


        private void ClimbHop() {
            Debug.Log("=====ClimbHop");
            //TODO: 播放
            //playFootstepOnLand = 0.5f;
            player.PlayAnimation("Hop");
            //获取目标的落脚点
            bool hit = player.CollideCheck(player.Position, Vector2.right * (int)player.Facing);
            if (hit) {
                player.HopWaitX = (int)player.Facing;
                player.HopWaitXSpeed = (int)player.Facing * Constants.ClimbHopX;
            }
            //ctx.ClimbHopSolid = ctx.CollideClimbHop((int)ctx.Facing);
            //if (ctx.ClimbHopSolid)
            //{
            //    //climbHopSolidPosition = climbHopSolid.Position;
            //    ctx.HopWaitX = (int)ctx.Facing;
            //    ctx.HopWaitXSpeed = (int)ctx.Facing * Constants.ClimbHopX;
            //}
            else {
                player.HopWaitX = 0;
                player.Speed.x = (int)player.Facing * Constants.ClimbHopX;
            }

            player.Speed.y = Math.Max(player.Speed.y, Constants.ClimbHopY);
            player.ForceMoveX = 0;
            player.ForceMoveXTimer = Constants.ClimbHopForceTime;
        }
    }
}