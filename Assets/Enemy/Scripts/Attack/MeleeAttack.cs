using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class MeleeAttack : MonoBehaviour,IAttack,IEffect
{
    public Vector2 range;
    public AudioClip hitAudio;
    private AudioSource attackAudio;
    private void Start()
    {
        GetComponent<BoxCollider2D>().size = range;
        attackAudio = GetComponent<AudioSource>();
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position,GetComponent<BoxCollider2D>().size);
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
    
    public IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            attackAudio.PlayOneShot(hitAudio);
        }
    }
}
