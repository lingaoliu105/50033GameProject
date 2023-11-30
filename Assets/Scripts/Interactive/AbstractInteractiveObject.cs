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

    // Update is called once per frame
    protected virtual void Update()
    {
        if (player)
        {
            var position = player.transform.position;
            if ((position - transform.position).magnitude <= interactDistance)
            {
                ShowInteractionMessage();
            }
        }
    }

    public abstract void Interact();

    protected virtual void ShowInteractionMessage()
    {
        text.text = interactMessage;
        messageCanvas.enabled = true;
    }
    
}
