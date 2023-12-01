using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class AbstractInteractiveObject : MonoBehaviour,IInteractive
{
    protected SpriteRenderer sprite;

    protected Animator anim;

    protected Collider2D col;

    protected TextMeshProUGUI text;

    protected Canvas messageCanvas;

    protected GameObject player;


    protected bool active;

    public Rect detectRange;
    public string interactMessage;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        messageCanvas = GetComponentInChildren<Canvas>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        messageCanvas.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");

    }

    protected virtual void Update()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            
        }
    
        DetectPlayer();
        if (active && Input.GetKeyDown("f"))
        {
            Interact();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(detectRange.position,detectRange.size);
    }

    protected virtual void DetectPlayer()
    {
        if (detectRange.Contains(player.transform.position))
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Activate();
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Deactivate();
        }
    }

    public abstract void Interact();

    protected virtual void Activate()
    {
        active = true;
        text.text = interactMessage;
        messageCanvas.enabled = true;
    }
    
    protected virtual void Deactivate()
    {
        active = false;
        messageCanvas.enabled = false;
    }
    
}
