using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Game;
using Unity.VisualScripting;

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
        public static float DashSpeed = 15f;
        public static float GroundY = 0f;
        public static float BossOnGroundY = -1f;
        public static float SizeX = 2f;
        public static float SizeY = 3f;

        public static GameObject CocktailMother;
        public static GameObject GroundWave;
        public static GameObject LandingWave;
        public static GameObject AimingBullet;
        public static GameObject DashAttack;
    }
    public enum Boss1CommonAttack { 
        JumpAndThrow,
        MeleeAttack,
        RangeAttack
    }
    public enum Boss1SpecialAttack { 
        ContinuousDash,
        BladeWave,
        ContinuousShot
    }
    public partial class Boss1Controller : StateController {
        public Rigidbody2D body;
        public Facings facing;
        public Vector2 velocity;
        public Boss1Panel panel;
        public PlayerController PlayerController;
        public SpriteRenderer spriteRenderer;
        public EffectManager EffectManager;
        const float DEVIATION = 0.02f;

        public int ATP = 0;
        public int HPMAX = 1000;
        public int HP = 1000;

        
        public void Jump(Facings facing = Facings.Left) { 
            StartCoroutine(ContinuousDash());
        }
        public IEnumerator WaitJump() {
            yield return new WaitForSeconds(1f);
            Jump();
        }
        public override void Start() {
            base.Start();
            panel.OnValidate();
            facing = Facings.Left;
            velocity = Vector2.zero;
            StartCoroutine(WaitJump());
        }
        public override void Update() {
            base.Update();
            if (facing == Facings.Right) {
                spriteRenderer.flipX = false;
            }
            else {
                spriteRenderer.flipX = true;
            }
            
            body.velocity = velocity;
        }
        public IEnumerator JumpUpCocktail() { 
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

        public IEnumerator JumpUpShooting() {
            for (int i = 0; i < B1Constants.VarJumpTime/2f; i++) {
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
            float[] shootTime = { 1f, 0.75f, 0.5f, 0.5f, 0.3f, 0.2f };
            for (int i=0; i<6; i++) {
                GameObject bullet = Instantiate(B1Constants.AimingBullet,new Vector3(0,0,0),  Quaternion.identity);
                bullet.GetComponent<AimAndShoot>().StartPos = transform.position + new Vector3(B1Constants.SizeX * 0.5f * (int)facing, B1Constants.SizeY * 0.5f, 0);
                bullet.GetComponent<AimAndShoot>().EndPos = PlayerController.Position;
                yield return new WaitForSeconds(shootTime[i]);
            }
            StartCoroutine(Fall());
        }

        public IEnumerator DashAndAttack() { 
            velocity.y = 0;
            GameObject dash = Instantiate(B1Constants.DashAttack, transform.position+ new Vector3(0, 0.4f, 0), Quaternion.identity);
            dash.transform.parent = transform;
            if (facing == Facings.Right) {
                dash.GetComponent<SpriteRenderer>().flipX = true;
            }
            dash.transform.localPosition = new Vector3(0, 0.4f, 0);
            dash.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.2f);
            Debug.Log("DashStart" + facing);
            velocity.x = B1Constants.DashSpeed * (int)facing;
            dash.GetComponent<SpriteRenderer>().enabled = true;
            float timer = 0;
            while (timer < 0.3f) {
                timer += Time.deltaTime;
                dash.transform.localPosition = new Vector3(0, 0.4f, 0);
                yield return null;
            }
            while (Math.Abs(velocity.x) > 0) {
                velocity.x -= B1Constants.RunAccel * (int)facing * Time.deltaTime;
                dash.transform.localPosition = new Vector3(0, 0.4f, 0);
                //Debug.Log("Dash"+velocity.x);
                //Debug.Log("Dash"+(int)facing);
                if (Math.Abs(velocity.x) < 0f || velocity.x * (int)facing <= 0) {
                    velocity.x = 0;
                    Destroy(dash);
                }
                yield return null;
            }
        }

        public IEnumerator ContinuousDash(int times = 3) {
            for (int i = 0; i < times; i++) {
                
                
                if (PlayerController.Position.x > transform.position.x) {
                    facing = Facings.Right;
                }
                else {
                    facing = Facings.Left;
                }
                StartCoroutine(DashAndAttack());
                yield return new WaitForSeconds(1f);
            }
        }

        public IEnumerator Fall() {
            while (transform.position.y > DEVIATION + B1Constants.BossOnGroundY) {
                if (velocity.y > B1Constants.MaxFall) {
                    velocity.y -= B1Constants.Gravity * Time.deltaTime;
                }
                if (transform.position.y <= DEVIATION + B1Constants.BossOnGroundY) {
                    velocity.y = 0;
                    transform.position = new Vector3(transform.position.x, B1Constants.BossOnGroundY, transform.position.z);
                    break;
                }
                yield return null;
            }

        }

        public IEnumerator FastFall() { 
            while (transform.position.y > DEVIATION + B1Constants.BossOnGroundY) {
                if (velocity.y > B1Constants.FastMaxFall) {
                    velocity.y += (B1Constants.FastMaxAccel - B1Constants.Gravity )* Time.deltaTime;
                }
                
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
            EffectManager.CameraShake(new Vector2(0, 1), 0.5f);
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
