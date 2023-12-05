using Game;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RobotBody : MonoBehaviour
{
    // Start is called before the first frame update
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public Vector3 firingOffset = new Vector3(0.4f, -1f, 0);
    private GameObject firing;
    public Facings facing = Facings.Right;
    private WaitForSeconds BeforeFiringInterval =  new WaitForSeconds(0.5f);
    public WaitForSeconds MissileInterval = new WaitForSeconds(0.3f);
    public int MissileCount = 3;
    public float MissileSpeed = 5f;

    //public GameObject firingPrefab;
    public GameObject missilePrefab;
    public GameObject missileLargePrefab;

    [SerializeField]
    private bool isFiring = false;
    void Start()
    {
        boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        animator = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update(){
        //if (facing == Facings.Right) {
            //transform.localScale = new Vector3(1, 1, 1);
        //} else {
            //transform.localScale = new Vector3(-1, 1, 1);
        //}
    }
    // [ContextMenu("Fire")]
    public void StartFire() { 
        //isFiring = true;
        //animator.SetTrigger("Fire");
        //StartCoroutine(FireCoroutine());
    }

    [ContextMenu("M.O.A.B")]
    public void LaunchLargeMissile() { 
        isFiring = true;
        animator.SetTrigger("Fire");
        StartCoroutine(LargeMissileCoroutine());
    }
    [ContextMenu("Missile")]
    public void LaunchMissile() {
        isFiring = true;
        animator.SetTrigger("Fire");
        StartCoroutine(MissileCoroutine());
    }

    public IEnumerator MissileCoroutine() {
        yield return BeforeFiringInterval;
        for (int i = 0; i < MissileCount; i++) {
            GameObject rocket = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            rocket.GetComponent<Missile>().targetPlayer = GameObject.FindWithTag("Player").transform;
            if (facing == Facings.Left) {
                rocket.GetComponent<Missile>().BeforeLaunchingSpeed = -rocket.GetComponent<Missile>().BeforeLaunchingSpeed;
            }
            yield return MissileInterval;
        }
        animator.SetTrigger("Stop");
        isFiring = false;
    }

    public IEnumerator LargeMissileCoroutine() {
        yield return BeforeFiringInterval;
        GameObject rocket = Instantiate(missileLargePrefab, transform.position, Quaternion.identity);
        rocket.GetComponent<LargeMissile>().targetPlayer = GameObject.FindWithTag("Player").transform;
        animator.SetTrigger("Stop");
        isFiring = false;
    }

    //public IEnumerator FireCoroutine() {
        //yield return BeforeFiringInterval;
        //firing = Instantiate(firingPrefab, transform.position + firingOffset * (int)facing, Quaternion.identity);
        //firing.transform.parent = transform;
        //firing.transform.localScale = new Vector3((int)facing, 1, 1);
    //}

    //[ContextMenu("Stop")]
    //public void StopFire() { 
    //    isFiring = false;
     //   Destroy(firing);
     //   firing = null;
     //   animator.SetTrigger("Stop");
    //}
}
