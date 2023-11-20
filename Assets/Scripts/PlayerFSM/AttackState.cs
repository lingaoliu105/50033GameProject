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
        private Vector2 dir;
        private int frame;
        private bool comboFlag;
        private float maxSpeed;
        public AttackState(PlayerController controller) : base(EActionState.Attack, controller) {
        }

        public override IEnumerator Coroutine() {
            for (int t = 0; t < player.AttackFrames1; t++) {
                yield return null;
            }
            stage = AttackStage.Attack;
            maxSpeed = player.AttackFrames2MaxSpeed;
            for (int t = 0; t < player.AttackFrames2; t++) {
                yield return null;
            }

            stage = AttackStage.AfterAttack;
            maxSpeed = player.AttackFrames3MaxSpeed;
            for (int t = 0; t < player.AttackFrames3; t++) {
                if (player.CanAttack) {
                    comboFlag = true;
                } else { }
                yield return null;
            }
            player.AttackEnd();
            player.SetState((int)EActionState.Normal);
        }

        public override bool IsCoroutine() {
            return true;
        }

        public override void OnBegin() {
            stage = AttackStage.BeforeAttack;
            maxSpeed = player.AttackFrames1MaxSpeed;
            dir = player.LastAim;
            comboFlag = false;
            frame = 0;
        }

        public override void OnEnd() {
        }

        public override EActionState Update(float deltaTime) {
            frame++;
            state = EActionState.Attack;
            player.Speed.x = maxSpeed * dir.x;
            if (comboFlag) { 
                Debug.Log("Combo " + frame);
                return EActionState.Combo;
            }
            return state;
        }
    }
}