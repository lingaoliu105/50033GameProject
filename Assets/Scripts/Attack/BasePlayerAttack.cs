using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BaseEnemyAttack00 : MonoBehaviour{
    public int attackDamage = 1;
    public int attackTimeByFrame = 18;
    public bool isAttacking = false;
    public bool destroyAfterAttack = false;
    public bool disableAfterAttack = false;
    public int rechargeAfterTimeByFrame = -1;
    public void Start() {
        isAttacking = true;
        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy() {
        for (int i = 0; i < attackTimeByFrame; i++) {
            yield return null;
        }
        Destroy(gameObject);
    }

    private IEnumerator WaitAndEnable() {
        for (int i = 0; i < rechargeAfterTimeByFrame; i++) {
            yield return null;
        }
        isAttacking = true;
    }

    public void Update() {
        if (isAttacking) {
            this.GetComponent<Collider2D>().enabled = true;
        }
        else {
            this.GetComponent<Collider2D>().enabled = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (isAttacking) {
            if (collision.gameObject.tag == "Enemy") {
                Debug.Log("Deal " + attackDamage + " damage to " + collision.gameObject.name);
                if (destroyAfterAttack) {
                    Destroy(gameObject);
                }
                if (disableAfterAttack) {
                    isAttacking = false;
                    this.GetComponent<Collider2D>().enabled = false;
                }
                if (rechargeAfterTimeByFrame > 0) {
                    StartCoroutine(WaitAndEnable());
                }
            }
        }
    }
}