using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Attack.PistolAttacks {
    public class PistolAttack: RangedAttack{
        public PistolAttack(GameObject AttackPrefab, PlayerController player, int Damage) : base(AttackPrefab, player, Damage) {
            this.ShootingAnimationFrames = 12;
            this.ShootingAnimationMaxSpeed = 0;
            this.MotionValue = 1;
            this.DoScreenShake = false;
            this.DoFrozenFrame = false;
            this.AnimationTrigger = "Shoot";
            this.ElecCost = 7;
        }
    }
}
