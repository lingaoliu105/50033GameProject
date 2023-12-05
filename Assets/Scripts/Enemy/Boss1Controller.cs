using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Game;
using Unity.VisualScripting;
using UnityEngine.UI;

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
        public static float DashSpeed = 18f;
        public static float MaxRun = 12f;
        public static float GroundY = 0f;
        public static float BossOnGroundY = -1f;
        public static float SizeX = 2f;
        public static float SizeY = 3f;

        public static GameObject CocktailMother;
        public static GameObject GroundWave;
        public static GameObject LandingWave;
        public static GameObject AimingBullet;
        public static GameObject DashAttack;
        public static GameObject MeleeAttack;
        public static GameObject RangeAttack;
    }
    public enum Boss1CommonAttack { 
        JumpAndThrow,
        MeleeAttack,
        BladeWave,
        RangeAttack
    }
    public enum Boss1SpecialAttack { 
        ContinuousDash,
        
        ContinuousShot
    }
    public enum Boss1ExAttack {
        JumpAndThrowMany,
        MeleeAttackLongRange,
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

        public AudioSource BGM;
        public AudioSource SFX;

        public int ATP = 0;
        private int HPMAX = 10000;
        public int HP = 10000;

        public Image HPBar;

        public int Heat = 0;

        public int NextSpecialAttackCountdown = 5;
        public int NextExCountdown = 20;
        public Boss1ExAttack LastEx = 0; 
        public float SpecialAtkRate = 0.5f;
        public float AttackRate = 0.5f;
        public string CurrentState;

        public IEnumerator WaitAndDecide(float time) { 
            CurrentState = "WaitAndDecide";
            yield return new WaitForSeconds(time);
            float random = UnityEngine.Random.Range(0f, 1f);
            NextExCountdown -= 1;
            NextSpecialAttackCountdown -= 1;
            if ( NextExCountdown == 0) {
                if (NextSpecialAttackCountdown == 0) NextSpecialAttackCountdown = 1;
                if ((int)LastEx == 1) {
                    LastEx = Boss1ExAttack.JumpAndThrowMany;
                } else { 
                    LastEx = Boss1ExAttack.MeleeAttackLongRange;
                }
                Heat += 1;
                if (Heat >= 3) {
                    Heat = 3;
                }
                StartCoroutine(ExAttack());
                NextExCountdown = 20 + (int)UnityEngine.Random.Range(-3, 4);
                
                yield break;
            }
            if (NextSpecialAttackCountdown == 0) {
                if (random < SpecialAtkRate) {
                    StartCoroutine(ContinuousDash(3+Heat));
                    SpecialAtkRate -= 0.1f;
                }
                else {
                    StartCoroutine(JumpUpShooting());
                    SpecialAtkRate += 0.1f;
                }
                NextSpecialAttackCountdown = 5 + (int)UnityEngine.Random.Range(-1, 2);
                yield break;
            }
            if (random < AttackRate) {
                StartCoroutine(PlainShoot());
                AttackRate -= 0.1f;
            }
            else {
                StartCoroutine(MoveTowardsPlayer());
                AttackRate += 0.1f;
            }

        }

        public IEnumerator WaitStart(float time) {
            CurrentState = "WaitStart";
            yield return new WaitForSeconds(time);
            BGM.Play();
            StartCoroutine(WaitAndDecide(1f));
        }

        public void PlayOneShot(AudioClip clip) {
            SFX.PlayOneShot(clip);
        }

        public override void TakeDamage(int damage) {
            HP -= damage;
            HPBar.rectTransform.sizeDelta = new Vector2(1000f*((float)HP/(float)HPMAX), 80);
            if (HP <= 0) {
                Destroy(gameObject);
            }
            else {

            }
        }

        public IEnumerator PlainShoot() { 
            if (PlayerController.Position.x > transform.position.x) {
                facing = Facings.Right;
            }
            else {
                facing = Facings.Left;
            }
            if (Math.Abs(PlayerController.Position.x - transform.position.x) > 5f) { 
                float targetX = PlayerController.Position.x - 5f * (int)facing;

                StartCoroutine(MoveTowardsTargetX(targetX, 1f));
                yield return new WaitForSeconds(1.1f);
            }
            CurrentState = "PlainShoot";
            for (int i = 0; i < 3+Heat/2; i++) {
                Shoot();
                yield return new WaitForSeconds(0.5f-0.08f*Heat);
            }
            StartCoroutine(WaitAndDecide(1f-0.2f*Heat));
        }

        public IEnumerator MoveTowardsTargetX(float targetX, float t) {
            CurrentState = "MoveTowardsTargetX";
            velocity.y = 0;
            velocity.x = 0;
            if (targetX > transform.position.x) {
                facing = Facings.Right;
            }
            else {
                facing = Facings.Left;
            }
            float speed = (targetX - transform.position.x) / (t);
            float timer = 0;
            while (timer < t) {
                timer += Time.deltaTime;
                velocity.x = speed;
                yield return null;
            }
            velocity.x = 0;
        }

        public IEnumerator ExAttack() {
            float pos = UnityEngine.Random.Range(0, 2) == 0 ? 11 : -11;
            StartCoroutine(MoveTowardsTargetX(pos, 1f));
            yield return new WaitForSeconds(1.1f);
            if (pos> 0) {
                facing = Facings.Left;
            }
            else {
                facing = Facings.Right;
            }
            velocity.x = 0;
            StartCoroutine(JumpUpCocktail());
            
        }
        public override void Start() {
            base.Start();
            panel.OnValidate();
            //NextExCountdown = 20 + (int)UnityEngine.Random.Range(-3,4);
            LastEx = UnityEngine.Random.Range(0, 2) == 0 ? Boss1ExAttack.JumpAndThrowMany : Boss1ExAttack.MeleeAttackLongRange;
            //NextSpecialAttackCountdown = 5 + (int)UnityEngine.Random.Range(-1, 2);
            facing = Facings.Left;
            velocity = Vector2.zero;
            StartCoroutine(WaitStart(1f));
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
            CurrentState = "JumpUpCocktail";
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
            CurrentState = "JumpUpShooting";
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
            float[] shootTime = { 1f-Heat*0.2f, 0.75f - Heat * 0.15f, 0.5f - Heat * 0.1f, 0.5f - Heat * 0.1f, 0.3f - Heat * 0.03f, 0.2f };
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
            CurrentState = "ContinuousDash";
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
            StartCoroutine(WaitAndDecide(1f-0.1f*Heat));
        }

        public IEnumerator Fall() {
            CurrentState = "Fall";
            while (transform.position.y > DEVIATION + B1Constants.BossOnGroundY) {
                if (velocity.y > B1Constants.MaxFall) {
                    velocity.y -= B1Constants.Gravity * Time.deltaTime;
                }
                if (transform.position.y <= DEVIATION + B1Constants.BossOnGroundY) {

                    break;
                }
                yield return null;
            }
            velocity.y = 0;
            velocity.x = 0;
            yield return null;
            transform.position = new Vector3(transform.position.x, B1Constants.BossOnGroundY + DEVIATION, transform.position.z);
            StartCoroutine(WaitAndDecide(1f-0.1f*Heat));
        }

        public IEnumerator FastFall() { 
            CurrentState = "FastFall";
            while (transform.position.y > DEVIATION + B1Constants.BossOnGroundY) {
                if (velocity.y > B1Constants.FastMaxFall) {
                    velocity.y += (B1Constants.FastMaxAccel - B1Constants.Gravity )* Time.deltaTime;
                }
                
                if (transform.position.y <= DEVIATION +B1Constants.BossOnGroundY) {
                    break;
                }
                yield return null;
            }
            velocity.y = 0;
            velocity.x = 0;
            yield return null;
            transform.position = new Vector3(transform.position.x, B1Constants.BossOnGroundY + DEVIATION, transform.position.z);
            StartCoroutine(LandingAttack());
        }

        public IEnumerator MoveTowardsPlayer() {
            CurrentState = "MoveTowardsPlayer";
            float speed = B1Constants.MaxRun + Heat*1.5f;
            if (Math.Abs(PlayerController.Position.x - transform.position.x) <= 1f) { 
                if (PlayerController.Position.x > transform.position.x) {
                    facing = Facings.Right;
                } else {
                    facing = Facings.Left;
                }
            } else {
                bool isSlowingDown = false;
                while (Math.Abs(PlayerController.Position.x - transform.position.x) > 1f) {
                    if (Math.Abs(PlayerController.Position.x - transform.position.x) < 3f) {
                        isSlowingDown = true;
                    }
                    if (PlayerController.Position.x >= transform.position.x) {
                        facing = Facings.Right;
                    } else {
                        facing = Facings.Left;
                    }
                    if (Math.Abs(velocity.x) < speed && !isSlowingDown) {
                        velocity.x += B1Constants.RunAccel * (int)facing * Time.deltaTime;
                    }
                    if (isSlowingDown && velocity.x * (int)facing > 0) {
                        velocity.x -= B1Constants.RunReduce * (int)facing * Time.deltaTime;
                    }
                    if (isSlowingDown && velocity.x * (int)facing <= 0) { 
                        velocity.x = 0;
                        break;
                    }
                    yield return null;
                }
                while (isSlowingDown && velocity.x * (int)facing > 0) {
                    velocity.x -= B1Constants.RunReduce * (int)facing * Time.deltaTime;
                    if (isSlowingDown && velocity.x * (int)facing <= 0) {
                        velocity.x = 0;
                        break;
                    }
                    yield return null;
                }
            }
            GameObject blade = Instantiate(B1Constants.MeleeAttack, transform.position + new Vector3(B1Constants.SizeX * 0.5f * (int)facing, 0.5f, 0), Quaternion.identity);
            blade.GetComponent<SpriteRenderer>().flipX = facing == Facings.Right;
            StartCoroutine(WaitAndDecide(1f-0.2f*Heat));
        }

        public void Shoot() { 
            if (PlayerController.Position.x > transform.position.x) {
                facing = Facings.Right;
            }
            else {
                facing = Facings.Left;
            }
            GameObject bullet = Instantiate(B1Constants.RangeAttack, transform.position + new Vector3(B1Constants.SizeX * 0.5f * (int)facing, 1f, 0), Quaternion.identity);
            bullet.GetComponent<CommonBullet>().initialVelocity = new Vector2(20f * (int)facing, 0);
        }

        public IEnumerator LandingAttack() {
            CurrentState = "LandingAttack";
            EffectManager.CameraShake(new Vector2(0, 1), 0.5f);
            Instantiate(B1Constants.LandingWave, transform.position-new Vector3(1f,0,0), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            GameObject waveLeft = Instantiate(B1Constants.GroundWave, transform.position + new Vector3(B1Constants.SizeX * 0.5f * -1f, 0.03f, 0), Quaternion.identity);
            waveLeft.GetComponent<SpriteRenderer>().flipX = true;
            waveLeft.GetComponent<MovingWave>().initialVelocity = waveLeft.GetComponent<MovingWave>().initialVelocity * -1f;
            GameObject waveRight = Instantiate(B1Constants.GroundWave, transform.position + new Vector3(B1Constants.SizeX * 0.5f * 1f, 0.03f, 0), Quaternion.identity);
            waveRight.GetComponent<MovingWave>().initialVelocity = waveRight.GetComponent<MovingWave>().initialVelocity * 1f;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(WaitAndDecide(1f));
        }

    }
}
