using Enemy;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RightArm : MonoBehaviour
{
    // Start is called before the first frame update
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public GameObject firingPrefab;
    public Vector3 firingOffset = new Vector3(0.4f, -1f, 0);
    private GameObject firing;
    public Facings facing = Facings.Right;
    private WaitForSeconds wait05 =  new WaitForSeconds(0.5f);

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
        if (facing == Facings.Right) {
            transform.localScale = new Vector3(1, 1, 1);
        } else {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    [ContextMenu("Fire")]
    public void StartFire() { 
        isFiring = true;
        animator.SetTrigger("Fire");
        StartCoroutine(FireCoroutine());
    }

    public IEnumerator FireCoroutine() {
        yield return wait05;
        firing = Instantiate(firingPrefab, transform.position + firingOffset * (int)facing, Quaternion.identity);
        firing.transform.parent = transform;
        firing.transform.localScale = new Vector3((int)facing, 1, 1);
    }

    [ContextMenu("Stop")]
    public void StopFire() { 
        isFiring = false;
        Destroy(firing);
        firing = null;
        animator.SetTrigger("Stop");
    }
}
