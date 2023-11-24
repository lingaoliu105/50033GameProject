using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Game;

namespace Assets.Scripts.Enemy {
    public static class B1Constants {
        //空气阻力
        public static float AirMult = 0.65f;
        //移动加速度
        public static float RunAccel = 100f;
        //移动减速度
        public static float RunReduce = 40f;
        //重力
        public static float Gravity = 90f;
        //普通最大下落速度
        public static float MaxFall = -16;
        //快速最大下落速度
        public static float FastMaxFall = -40f;
        //快速下落加速度
        public static float FastMaxAccel = -50f;
        //最大跳跃速度
        public static float JumpSpeed = 36f;
        //跳跃持续时间
        public static int VarJumpTime = 12;
        //地面高度
        public static float GroundY = 0f;
        public static float BossOnGroundY = -1f;
        public static float SizeX = 2f;
        public static float SizeY = 3f;

        public static GameObject CocktailMother;
        public static GameObject GroundWave;
        public static GameObject LandingWave;
    }
    public partial class Boss1Controller : StateController {
        public Rigidbody2D body;
        public Facings facing;
        public Vector2 velocity;
        public Boss1Panel panel;
        const float DEVIATION = 0.02f;
        public void Jump(Facings facing = Facings.Left) { 
            StartCoroutine(JumpUp());
        }
        public IEnumerator WaitJump() {
            yield return new WaitForSeconds(1f);
            Jump();
        }
        public override void Start() {
            base.Start();
            panel.OnValidate();
            body = GetComponent<Rigidbody2D>();
            facing = Facings.Left;
            velocity = Vector2.zero;
            Debug.Log("Start");
            StartCoroutine(WaitJump());
        }
        public override void Update() {
            base.Update();
            body.velocity = velocity;
        }
        public IEnumerator JumpUp() { 
            for (int i = 0; i < B1Constants.VarJumpTime; i++) {
                velocity.y = B1Constants.JumpSpeed;
                velocity.x = 0;
                yield return null;
            }
            while (velocity.y > 0) {  
                velocity.y -= B1Constants.Gravity * Time.deltaTime;
                if (velocity.y < 0) {
                    velocity.y = 0;
                }
                yield return null;
            }
            GameObject bomb = Instantiate(B1Constants.CocktailMother, transform.position+new Vector3(B1Constants.SizeX * 0.5f * (int)facing, B1Constants.SizeY * 0.5f, 0), Quaternion.identity);
            bomb.GetComponent<SplitingProjectile>().initialVelocity *= (int)facing;

            yield return new WaitForSeconds(1f);
            StartCoroutine(FastFall());
        }

        public IEnumerator FastFall() { 
            while (velocity.y > B1Constants.FastMaxFall) {
                velocity.y += B1Constants.FastMaxAccel * Time.deltaTime;
                if (transform.position.y <= DEVIATION +B1Constants.BossOnGroundY) { 
                    velocity.y = 0;
                    transform.position = new Vector3(transform.position.x, B1Constants.BossOnGroundY, transform.position.z);
                    StartCoroutine(LandingAttack());
                    break;
                }
                yield return null;
            }
            
        }
        public IEnumerator LandingAttack() {
            Instantiate(B1Constants.LandingWave, transform.position-new Vector3(1f,0,0), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            GameObject waveLeft = Instantiate(B1Constants.GroundWave, transform.position + new Vector3(B1Constants.SizeX * 0.5f * -1f, 0.03f, 0), Quaternion.identity);
            waveLeft.GetComponent<SpriteRenderer>().flipX = true;
            waveLeft.GetComponent<MovingWave>().initialVelocity = waveLeft.GetComponent<MovingWave>().initialVelocity * -1f;
            GameObject waveRight = Instantiate(B1Constants.GroundWave, transform.position + new Vector3(B1Constants.SizeX * 0.5f * 1f, 0.03f, 0), Quaternion.identity);
            waveRight.GetComponent<MovingWave>().initialVelocity = waveRight.GetComponent<MovingWave>().initialVelocity * 1f;
        }

    }
}
