using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy {
    [CreateAssetMenu(fileName = "Boss1Panel", menuName = "BossPanel/Boss1", order = 1)]
    public class Boss1Panel : ScriptableObject { 
        public float AirMult = 0.65f;
        public float RunAccel = 100f;
        public float RunReduce = 40f;
        public float Gravity = 90f;
        public float MaxFall = -16;
        public float FastMaxFall = -40f;
        public float FastMaxAccel = -50f;
        public float JumpSpeed = 18f;
        public int VarJumpTime = 18;
        public float GroundY = 0f;
        public float DashSpeed = 30f;
        public GameObject CocktailMother;
        public GameObject GroundWave;
        public GameObject LandingWave;
        public GameObject AimingBullet;
        public GameObject DashAttack;
        public void OnValidate() {
            B1Constants.AirMult = AirMult;
            B1Constants.RunAccel = RunAccel;
            B1Constants.RunReduce = RunReduce;
            B1Constants.Gravity = Gravity;
            B1Constants.MaxFall = MaxFall;
            B1Constants.FastMaxFall = FastMaxFall;
            B1Constants.FastMaxAccel = FastMaxAccel;
            B1Constants.JumpSpeed = JumpSpeed;
            B1Constants.VarJumpTime = VarJumpTime;
            B1Constants.GroundY = GroundY;
            B1Constants.CocktailMother = CocktailMother;
            B1Constants.GroundWave = GroundWave;
            B1Constants.LandingWave = LandingWave;
            B1Constants.AimingBullet = AimingBullet;
            B1Constants.DashSpeed = DashSpeed;
            B1Constants.DashAttack = DashAttack;
        }
    }

}
