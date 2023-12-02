using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndergroundDoor : AbstractInteractiveObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        col.enabled = false;
        StartCoroutine(Disappear());
        Deactivate();
    }

    IEnumerator Disappear()
    {
        int n = 8;
        for (int i = 0; i < n; i++)
        {
            sprite.enabled = !sprite.enabled;
            sprite.color -= new Color(0, 0, 0, 1f / n);
            yield return new WaitForSeconds(0.3f);
        }

        sprite.enabled = false;

    }
}
