using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Attack {
    [Serializable]
    public class KatanaAttackLight1: MeleeAttack {
        public KatanaAttackLight1(GameObject AttackPrefab, PlayerController player, int Damage) : base(AttackPrefab, player, Damage) {
            BeforeAttackFrames = 6;
            BeforeAttackMaxSpeed = 1f;
            AfterAttackFrames = 18;
            AfterAttackMaxSpeed = 0f;
            AttackFrames = 18;
            AttackMaxSpeed = 0f;
            MotionValue = 0.7f;
            DoScreenShake = false;
            DoFrozenFrame = false;
            StaminaCost = 18;
            AnimationTrigger = "Attack";
        }


    }
    [Serializable]

    public class KatanaAttackLight2 : MeleeAttack { 
        public KatanaAttackLight2(GameObject AttackPrefab, PlayerController player, int Damage) : base(AttackPrefab, player, Damage) {
            BeforeAttackFrames = 6;
            BeforeAttackMaxSpeed = 1f;
            AfterAttackFrames = 18;
            AfterAttackMaxSpeed = 0f;
            AttackFrames = 18;
            AttackMaxSpeed = 0f;
            MotionValue = 1f;
            DoScreenShake = false;
            DoFrozenFrame = false;
            StaminaCost = 18;
            AnimationTrigger = "Combo";
        }

    }
}
