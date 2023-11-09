using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    public partial class PlayerController {
        public bool CanAttack {
            get {
                return GameInput.AttackButton.Pressed() && onGround;
            }
        }

        public AttackParam currentAttackParam;
        public AttackParam nextAttackParam;
        private int attackCount = 0;
        

        public EActionState AttackStart() {
            currentAttackParam = AttackParams.KatanaLightAtk;
            nextAttackParam = AttackParams.KatanaLightAtk;
            PlayAnimation("Attack");
            GameInput.AttackButton.ConsumeBuffer();
            attackCount = 1;
            return EActionState.Attack;
        }

        public EActionState ComboAttack() {
            currentAttackParam = AttackParams.KatanaLightAtk;
            nextAttackParam = AttackParams.KatanaLightAtk;
            if (attackCount == 1) {
                // Combo
                attackCount = 0;
                PlayAnimation("Combo");
            } else {
                // Back to start
                attackCount = 1;
                PlayAnimation("Attack");
            }
            GameInput.AttackButton.ConsumeBuffer();
            return EActionState.Attack;
        }
    }
}
