using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractEnemyController : MonoBehaviour
{
    public GameConstants gameConstants;
    protected float hp;
    protected Animator anim;
    protected Collider2D detectRange;
    protected bool isAlive = true;
    protected SpriteRenderer _spriteRenderer;
    protected Slider _healthBar;
    protected Material mat;
    protected Rigidbody2D body;
    public GameVariables gameVariables;
    protected void Start()
    {
        anim = GetComponent<Animator>();
        detectRange = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _healthBar = GetComponentInChildren<Slider>();
        body = GetComponent<Rigidbody2D>();
        
        mat = _spriteRenderer.material;
        SetupHp();
    }

    protected abstract void SetupHp();

    protected PlayerController targetPlayer;
    IEnumerator ResetMateial()
    {
        yield return new WaitForSeconds(0.1f);
        mat.SetInt("_BeAttack",0);
    }
    public void TakeDamage(float amount)
    {
        hp -= amount;
        _healthBar.value = hp;
        mat.SetInt("_BeAttack",1);
        StartCoroutine(ResetMateial());
        
        if (hp <= 0)
        {
            anim.SetTrigger("die");
        }  
    }
    public abstract void Attack();
    public abstract void Move();

    public void Die()
    {
        isAlive = false;
        Destroy(gameObject);
    }
    public abstract void Patrol();
    public void OnTriggerEnter2D(Collider2D other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
          targetPlayer = other.gameObject.GetComponent<PlayerController>();
      }
    }
    public void OnTriggerExit2D(Collider2D other)
     {
         if (targetPlayer && other.gameObject == targetPlayer.gameObject)
         {
             targetPlayer = null;
         }
     }
}
