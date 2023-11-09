using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Game {
    public class ComboState : BaseActionState {
        public ComboState(PlayerController controller) : base(EActionState.Combo, controller) {
        }

        public override IEnumerator Coroutine() {
            return null;
        }

        public override bool IsCoroutine() {
            return false;
        }

        public override void OnBegin() {
        }

        public override void OnEnd() {
        }

        public override EActionState Update(float deltaTime) {
            Debug.Log("Combo");
            return player.ComboAttack();
        }
    }
}