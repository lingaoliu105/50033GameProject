using Game;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RobotArm : MonoBehaviour
{
    // Start is called before the first frame update
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public Vector3 firingOffset = new Vector3(0.4f, -1f, 0);
    private GameObject firing;
    public Facings facing = Facings.Right;
    private WaitForSeconds BeforeFiringInterval =  new WaitForSeconds(0.5f);
    private WaitForSeconds FiringTime =  new WaitForSeconds(2f);
    public WaitForSeconds RocketInterval = new WaitForSeconds(0.3f);
    public float RocketFlyingTime = 1f;
    public int RocketCount = 3;

    public GameObject firingPrefab;
    public GameObject rocketPrefab;

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
    [ContextMenu("Fire")]
    public void StartFire() { 
        isFiring = true;
        animator.SetTrigger("Fire");
        StartCoroutine(FireCoroutine());
    }

    [ContextMenu("Rocket")]
    public void LaunchRocket() { 
        isFiring = true;
        animator.SetTrigger("Fire");
        StartCoroutine(RocketCoroutine());
    }

    public IEnumerator RocketCoroutine() {
        yield return BeforeFiringInterval;
        for (int i = 0; i < RocketCount; i++) {
            GameObject rocket = Instantiate(rocketPrefab, transform.position + new Vector3(firingOffset.x * (int)facing, firingOffset.y, 0), Quaternion.identity);
            rocket.GetComponent<ABRocket>().target = GameObject.FindWithTag("Player").transform.position;
            rocket.GetComponent<ABRocket>().t = RocketFlyingTime;
            yield return RocketInterval;
        }
        animator.SetTrigger("Stop");
        isFiring = false;
    }

    public IEnumerator FireCoroutine() {
        yield return BeforeFiringInterval;
        firing = Instantiate(firingPrefab, transform.position + new Vector3(firingOffset.x*(int)facing, firingOffset.y,0), Quaternion.identity);
        firing.transform.localScale = new Vector3((int)facing, 1, 1);
        firing.transform.parent = transform;
        yield return FiringTime;
        StopFire();
    }

    [ContextMenu("Stop")]
    public void StopFire() { 
        isFiring = false;
        Destroy(firing);
        firing = null;
        animator.SetTrigger("Stop");
    }
}
