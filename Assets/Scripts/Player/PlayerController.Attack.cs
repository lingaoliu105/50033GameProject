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
        GameObject attack;
        

        public EActionState AttackStart() {
            currentAttackParam = AttackParams.KatanaLightAtk;
            nextAttackParam = AttackParams.KatanaLightAtk;
            PlayAnimation("Attack");
            GameInput.AttackButton.ConsumeBuffer();
            attackCount = 1;
            attack = Instantiate(attack1, transform.position + attack1.transform.position, Quaternion.identity);
            attack.transform.parent = transform;
            return EActionState.Attack;
        }

        public EActionState ComboAttack() {
            currentAttackParam = AttackParams.KatanaLightAtk;
            nextAttackParam = AttackParams.KatanaLightAtk;
            if (attackCount == 1) {
                // Combo
                attackCount = 0;
                PlayAnimation("Combo");
                attack = Instantiate(attack2, transform.position + attack2.transform.position, Quaternion.identity);
                attack.transform.parent = transform;

            } else {
                // Back to start
                attackCount = 1;
                PlayAnimation("Attack");
                attack = Instantiate(attack1, transform.position + attack1.transform.position, Quaternion.identity);
                attack.transform.parent = transform;
            }
            GameInput.AttackButton.ConsumeBuffer();
            return EActionState.Attack;
        }
    }
}
