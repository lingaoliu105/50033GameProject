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

    public float interactDistance;

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

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ShowInteractionMessage();
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HideInteractionMessage();
        }
    }

    public abstract void Interact();

    protected virtual void ShowInteractionMessage()
    {
        text.text = interactMessage;
        messageCanvas.enabled = true;
    }
    
    protected virtual void HideInteractionMessage()
    {
        messageCanvas.enabled = false;
    }
    
}
