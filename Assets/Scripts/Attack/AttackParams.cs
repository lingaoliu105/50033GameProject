using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering.Universal;

namespace Game {
    public struct AttackParam {
        public int BeforeAttackFrames;
        public float BeforeAttackMaxSpeed;
        public int AfterAttackFrames;
        public float AfterAttackMaxSpeed;
        public int AttackFrames;
        public float AttackMaxSpeed;
        public int AttackDamage;
        public bool DoScreenShake;
        public bool DoFrozenFrame;
    }
    public static class AttackParams {
        public static float ContinuousAttack = 0.1f;
        public static AttackParam KatanaLightAtk = new AttackParam() {
            BeforeAttackFrames = 6,
            BeforeAttackMaxSpeed = 1f,
            AfterAttackFrames = 18,
            AfterAttackMaxSpeed = 0f,
            AttackFrames = 18,
            AttackMaxSpeed = 0f,
            AttackDamage = 7,
            DoScreenShake = false,
            DoFrozenFrame = false,
        };
        public static AttackParam KatanaLightAtk2 = new AttackParam() {
            BeforeAttackFrames = 6,
            BeforeAttackMaxSpeed = 1f,
            AfterAttackFrames = 18,
            AfterAttackMaxSpeed = 0f,
            AttackFrames = 18,
            AttackMaxSpeed = 0f,
            AttackDamage = 9,
            DoScreenShake = false,
            DoFrozenFrame = false,
        };
        public static AttackParam KatanaHold = new AttackParam() {
            BeforeAttackFrames = 6,
            BeforeAttackMaxSpeed = 0f,
            AfterAttackFrames = 18,
            AfterAttackMaxSpeed = 0f,
            AttackFrames = 18,
            AttackMaxSpeed = 0f,
            AttackDamage = 0,
            DoScreenShake = false,
            DoFrozenFrame = false,
        };
        public static AttackParam KatanaHoldAtk = new AttackParam() {
            BeforeAttackFrames = 30,
            BeforeAttackMaxSpeed = 0f,
            AfterAttackFrames = 18,
            AfterAttackMaxSpeed = 0f,
            AttackFrames = 12,
            AttackMaxSpeed = 0f,
            AttackDamage = 12,
            DoScreenShake = false,
            DoFrozenFrame = false,
        };
        public static AttackParam KatanaHoldAtkP = new AttackParam() {
            BeforeAttackFrames = 0,
            BeforeAttackMaxSpeed = 0f,
            AfterAttackFrames = 18,
            AfterAttackMaxSpeed = 0f,
            AttackFrames = 18,
            AttackMaxSpeed = 0f,
            AttackDamage = 15,
            DoScreenShake = false,
            DoFrozenFrame = false,
        };
    }
}
