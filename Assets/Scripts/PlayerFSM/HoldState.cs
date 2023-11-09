using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Game {
    public enum HoldStage { 
        Holding,
        PerfectTiming,
        OverHold,
    }

    public class HoldState : BaseActionState {
        private HoldStage stage;
        private AttackParam param;
        private Vector2 dir;
        private int holdFrame;
        private int perfectFrame;
        private bool comboFlag;
        private float maxSpeed;
        public HoldState(PlayerController controller) : base(EActionState.Attack, controller) {
        }

        public override IEnumerator Coroutine() {
            for (int i = 0; i < holdFrame; i++) {
                if (!GameInput.HeavyAttackButton.Checked()) {
                    break;
                }
                yield return null;
            }

        }

        public override bool IsCoroutine() {
            return true;
        }

        public override void OnBegin() {
            param = player.currentAttackParam;
            holdFrame = param.BeforeAttackFrames;
            perfectFrame = param.AttackFrames;
            perfectFrame = 0;
        }

        public override void OnEnd() {
        }

        public override EActionState Update(float deltaTime) {
            state = EActionState.Hold;
            return state;
        }
    }
}