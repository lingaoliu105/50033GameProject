using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Game {
    public enum AttackStage {
        BeforeAttack,
        Attack,
        AfterAttack,
    }
    public class AttackState : BaseActionState {
        private AttackStage stage;
        private AttackParam param;
        private Vector2 dir;
        private int frame;
        private bool comboFlag;
        private float maxSpeed;
        public AttackState(PlayerController controller) : base(EActionState.Attack, controller) {
        }

        public override IEnumerator Coroutine() {
            for (int t = 0; t < param.BeforeAttackFrames; t++) {
                yield return null;
            }
            stage = AttackStage.Attack;
            maxSpeed = param.AttackMaxSpeed;
            for (int t = 0; t < param.AttackFrames; t++) {
                yield return null;
            }

            stage = AttackStage.AfterAttack;
            maxSpeed = param.AfterAttackMaxSpeed;
            for (int t = 0; t < param.AfterAttackFrames; t++) {
                if (player.CanAttack) {
                    Debug.Log("Combooo " + frame);
                    comboFlag = true;
                } else { }
                yield return null;
            }
            player.PlayAnimation("AttackEnd");
            player.SetState((int)EActionState.Normal);
        }

        public override bool IsCoroutine() {
            return true;
        }

        public override void OnBegin() {
            stage = AttackStage.BeforeAttack;
            param = player.currentAttackParam;
            maxSpeed = param.BeforeAttackMaxSpeed;
            dir = player.LastAim;
            comboFlag = false;
            frame = 0;
        }

        public override void OnEnd() {
        }

        public override EActionState Update(float deltaTime) {
            frame++;
            state = EActionState.Attack;
            player.Speed.x = Mathf.MoveTowards(player.Speed.x, maxSpeed * dir.x, Constants.DuckFriction * deltaTime);
            if (comboFlag) { 
                Debug.Log("Combo " + frame);
                return EActionState.Combo;
            }
            return state;
        }
    }
}