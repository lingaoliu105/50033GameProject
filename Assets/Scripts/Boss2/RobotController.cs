using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public static class RobotControllerParams { 
    static public int MaxHP = 8000;
    static public int[] RocketNumber = new int[4] {3, 3, 6, 9};
    static public float[] RocketFlyingTime = new float[4] { 2f, 2f, 1f, 1f};
    static public float[] RocketInterval = new float[4] { 0.5f, 0.2f, 0.2f, 0.2f};
    static public int[] MissileNumber = new int[4] { 2, 4, 6, 6};
    static public float[] MissileSpeed = new float[4] { 5f, 5f, 5f, 8f};
    static public int[] MissileLargeNumber = new int[4] { 1, 2, 2, 2};
    static public float[] NetInterval = new float[4] { 0.5f, 0.4f, 0.2f, 0.2f};
}
public enum RobotCommonAttack {
    MGLeft,
    MGRight,
    Rocket,
}
public enum Boss1SpecialAttack {
    Missile,
    LargeMissile,
}
public enum Boss1ExAttack {
    LaserNet,
    LaserCircle
}
public class RobotController: MonoBehaviour {
    public int[] bodyPartDamage;
    public int HP;

    public RobotArm LArm;
    public RobotArm RArm;
    public RobotBody Body;
    public Animator LAnimaror;
    public Animator RAnimaror;

    public int Heat = 0;
    public float HeatRate = 0f;

    public AudioSource SFX;
    public AudioClip hitSound;

    public GameObject LaserNetA1Prefab;
    public GameObject LaserNetA2Prefab;
    public GameObject LaserNetB1Prefab;
    public GameObject LaserNetB2Prefab;
    private Vector2 NetPos = new Vector2(48f, 5.25f);
    private WaitForSeconds NetInterval = new WaitForSeconds(0.5f);

    public GameObject LaserCirclePrefab;
    public Image HPBar;

    private WaitForSeconds SkillCD = new WaitForSeconds(1f);

    public int AtkCount = 0;
    public int SpcCount = 0;

    public Facings facing = Facings.Right;
    public float Velocity = 0f;

    public GameObject DeadRobot;

    public void Start() {
        bodyPartDamage = new int[(int)BodyPartType.Size];
        HP = RobotControllerParams.MaxHP;
        SetHeat(0);
        StartCoroutine(WaitAndLaunch());
    }
    public void Update() {
        int tmpDamage = 0;
        for (int i = 0; i < bodyPartDamage.Length; i++) {
            if (bodyPartDamage[i] > tmpDamage) {
                tmpDamage = bodyPartDamage[i];
                
            }
            bodyPartDamage[i] = 0;
        }
        if (tmpDamage > 0) {
            //Debug.Log($"===hit {this.gameObject} ===Damage {tmpDamage}");
            HP -= tmpDamage;
            SFX.PlayOneShot(hitSound);
            HPBar.rectTransform.sizeDelta = new Vector2(1000f * ((float)HP / (float)RobotControllerParams.MaxHP), 80);
            if (HP <= 0) {
                GameManager.Instance.SaveData.isBoss2Beaten = true;
                Instantiate(DeadRobot, transform.position, Quaternion.identity);
                PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
                player.GainSoul(50000);
                Destroy(this.gameObject);
            }
        }
        LArm.facing = facing;
        RArm.facing = facing;
        Body.facing = facing;
        if (facing == Facings.Right) {
            transform.localScale = new Vector3(1, 1, 1);
        } else {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        transform.position += Velocity * Vector3.right * Time.deltaTime * (int)facing;
        LAnimaror.SetFloat("Speed", Velocity);
        RAnimaror.SetFloat("Speed", Velocity);
    }
    public IEnumerator WaitAndLaunch() {
        yield return SkillCD;
        Vector2 target = Vector2.zero;
        if (GameObject.FindWithTag("Player")!= null) {
            target = GameObject.FindWithTag("Player").transform.position;
        }
        facing = target.x > transform.position.x ? Facings.Right : Facings.Left;
        if (AtkCount < 2) {
            AtkCount++;
            StartCoroutine(CommonAttack());
        } else if (SpcCount < 2) {
            AtkCount = 0;
            SpcCount++;
            StartCoroutine(SpecialAttack());
        } else {
            AtkCount = 0;
            SpcCount = 0;
            StartCoroutine(ExAttack());
        }
    }
    public void OnDestory() {
        StopAllCoroutines();
        
    }

    public void hit(int Damage, BodyPartType bodyPartType) {
        //Debug.Log($"===hit {this.gameObject}===Damage {Damage}");
        if (Damage > 0) {
            bodyPartDamage[(int)BodyPartType.Body] = Damage;
        }
    }
    float commonAttackRate = 0.5f;
    int lastAtk = 0;
    [ContextMenu("Attack0")]
    public void Attack0() {
        StartCoroutine(CommonAttack());
    }
    public IEnumerator CommonAttack() {
        if (Heat < 1) {
            if (lastAtk == 0) {
                yield return LeftArmShoot();
            }
            if (lastAtk == 1) { 
                yield return RightArmShoot();
            }
            if (lastAtk == 2) {
                yield return LeftArmRocket();
                yield return RightArmRocket();
            }
            lastAtk = (lastAtk + 1) % 3;
        } else { 
            float r = UnityEngine.Random.Range(0f, 1f);
            if (r > commonAttackRate) { 
                commonAttackRate += 0.1f;
                StartCoroutine(LeftArmRocket());
                yield return RightArmShoot();
            } else { 
                commonAttackRate -= 0.1f;
                StartCoroutine(RightArmRocket());
                yield return LeftArmShoot();
            }
        }
        StartCoroutine(WaitAndLaunch());
    }
    float specialAttackRate = 0.5f;
    [ContextMenu("Attack1")]
    public void Attack1() { 
        StartCoroutine(SpecialAttack());
    }
    [ContextMenu("Attack2")]
    public void Attack2() {
        StartCoroutine(ExAttack());
    }
    public IEnumerator SpecialAttack() {
        float r = UnityEngine.Random.Range(0f, 1f);
        if (r > specialAttackRate) {
            specialAttackRate += 0.1f;
            yield return Missiles();
        } else {
            specialAttackRate -= 0.1f;
            yield return LargeMissile();
        }
        StartCoroutine(WaitAndLaunch());
    }
    public IEnumerator ExAttack() {
        facing = NetPos.x > transform.position.x ? Facings.Right : Facings.Left;
        float maxVelocity = Heat * 2f + 9f;
        while (Math.Abs(this.transform.position.x - NetPos.x) >= 0.1f && (NetPos.x - transform.position.x)*(int)facing>=0) {
            Velocity = Math.Min(Velocity + 1.5f, maxVelocity);
            yield return null;
        }
        Velocity = 0f;
        float r = UnityEngine.Random.Range(0f, 1f);
        if (Heat == 0) {
            if (r > 0.5) {
                yield return LaserNetA();
            } else {
                yield return LaserNetB();
            }
            HeatRate += 1f;
        } else if (Heat == 1) {
            if (r < 0.3) {
                yield return LaserNetA();
            } else if (r < 0.6) {
                yield return LaserNetB();
            } else {
                yield return Circle();
            }
            HeatRate += 0.5f;
        } else if (Heat == 2) {
            if (r < 0.3) {
                yield return LaserNetA();
                yield return LaserNetB();
            } else if (r < 0.6) {
                yield return LaserNetB();
                yield return LaserNetA();
            } else {
                yield return Circle();
            }
            HeatRate += 0.5f;
        } else {
            if (r < 0.3) {
                yield return LaserNetA();
                yield return LaserNetB();
                yield return Circle();
            } else if (r < 0.6) {
                yield return LaserNetB();
                yield return LaserNetA();
                yield return Circle();
            } else {
                yield return Circle();
                yield return LaserNetA();
                yield return LaserNetB();
            }
        } 
        SetHeat((int)HeatRate);
        yield return new WaitForSeconds(2f);
        StartCoroutine(WaitAndLaunch());
    }

    public void SetHeat(int h) {
        if (h >= 3) Heat = 3;
        else if (h <= 0) Heat = 0;
        else Heat = h;
        RArm.RocketCount = RobotControllerParams.RocketNumber[Heat];
        RArm.RocketFlyingTime = RobotControllerParams.RocketFlyingTime[Heat];
        RArm.RocketInterval = new WaitForSeconds(RobotControllerParams.RocketInterval[Heat]);
        LArm.RocketCount = RobotControllerParams.RocketNumber[Heat];
        LArm.RocketFlyingTime = RobotControllerParams.RocketFlyingTime[Heat];
        LArm.RocketInterval = new WaitForSeconds(RobotControllerParams.RocketInterval[Heat]);
        Body.MissileCount = RobotControllerParams.MissileNumber[Heat];
        Body.MissileSpeed = RobotControllerParams.MissileSpeed[Heat];
        Body.MissileInterval = new WaitForSeconds(RobotControllerParams.RocketInterval[Heat]);
        NetInterval = new WaitForSeconds(RobotControllerParams.NetInterval[Heat]);
    }
    public IEnumerator LeftArmRocket() { 
        LArm.LaunchRocket();
        yield return new WaitForSeconds(RobotControllerParams.RocketInterval[Heat] * RobotControllerParams.RocketNumber[Heat] +1f);
    }
    public IEnumerator RightArmRocket() {
        RArm.LaunchRocket();
        yield return new WaitForSeconds(2f);
    }
    public IEnumerator RightArmShoot() {
        Vector2 target = GameObject.FindWithTag("Player").transform.position;
        facing = target.x > transform.position.x ? Facings.Right : Facings.Left;
        float maxVelocity = Heat * 2f + 9f;
        while (Math.Abs(this.transform.position.x - target.x) >= 1) {
            Velocity = Math.Min(Velocity + 1.5f, maxVelocity);
            yield return null;
        }
        Velocity = 0f;
        RArm.StartFire();
        yield return new WaitForSeconds(2f);
    }
    public IEnumerator LeftArmShoot() {
        Vector2 target = GameObject.FindWithTag("Player").transform.position;
        facing = target.x > transform.position.x ? Facings.Right : Facings.Left;
        float maxVelocity = Heat * 2f + 9f;
        while (Math.Abs(this.transform.position.x - target.x) >= 2) {
            Velocity = Math.Min(Velocity + 1.5f, maxVelocity);
            yield return null;
        }
        Velocity = 0f;
        LArm.StartFire();
        yield return new WaitForSeconds(2f);
    }
    public IEnumerator Missiles() {
        Body.LaunchMissile();
        yield return new WaitForSeconds(RobotControllerParams.RocketInterval[Heat] * RobotControllerParams.MissileNumber[Heat] + 1f);
    }
    public IEnumerator LargeMissile() {
        Body.LaunchLargeMissile();
        yield return new WaitForSeconds(2f);
    }
    public IEnumerator LaserNetA() { 
        float r = UnityEngine.Random.Range(0f, 1f);
        GameObject a, b;
        if (r > 0.5) {
            a = Instantiate(LaserNetA1Prefab, transform.position + LaserNetA1Prefab.transform.position, Quaternion.identity);
            b = Instantiate(LaserNetA2Prefab, transform.position + LaserNetA2Prefab.transform.position, Quaternion.identity);
        } else { 
            a = Instantiate(LaserNetA2Prefab, transform.position + LaserNetA2Prefab.transform.position, Quaternion.identity);
            b = Instantiate(LaserNetA1Prefab, transform.position + LaserNetA1Prefab.transform.position, Quaternion.identity);
        }
        yield return NetInterval;
        a.GetComponent<DronesController>().Launch();
        yield return NetInterval;
        b.GetComponent<DronesController>().Launch();
        yield return new WaitForSeconds(2f);
        Destroy(a);
        Destroy(b);
    }

    public IEnumerator LaserNetB() {
        float r = UnityEngine.Random.Range(0f, 1f);
        GameObject a, b;
        if (r > 0.5) {
            a = Instantiate(LaserNetB1Prefab, transform.position + LaserNetB1Prefab.transform.position, Quaternion.identity);
            b = Instantiate(LaserNetB2Prefab, transform.position + LaserNetB2Prefab.transform.position, Quaternion.identity);
        } else {
            a = Instantiate(LaserNetB2Prefab, transform.position + LaserNetB2Prefab.transform.position, Quaternion.identity);
            b = Instantiate(LaserNetB1Prefab, transform.position + LaserNetB1Prefab.transform.position, Quaternion.identity);
        }
        yield return NetInterval;
        a.GetComponent<DronesController>().Launch();
        yield return NetInterval;
        b.GetComponent<DronesController>().Launch();
        yield return new WaitForSeconds(2f);
        Destroy(a);
        Destroy(b);
    }

    public IEnumerator Circle() {
        float r = UnityEngine.Random.Range(0f, 1f);
        GameObject a;
        if (r > 0.5f) {
            a = Instantiate(LaserCirclePrefab, transform.position + LaserCirclePrefab.transform.position + new Vector3(0, 2f, 0), Quaternion.identity);
        } else {
            a = Instantiate(LaserCirclePrefab, transform.position + LaserCirclePrefab.transform.position + new Vector3(0, -2f, 0), Quaternion.identity);
        }
        
        yield return new WaitForSeconds(1f);
        a.GetComponent<DronesCircle>().LaunchOneByOne();
        yield return new WaitForSeconds(4f*(Heat+1));
        a.GetComponent<DronesCircle>().Stop();
        yield return new WaitForSeconds(1f);
        Destroy(a);
    }

}
