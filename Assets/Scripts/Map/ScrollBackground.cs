using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    private SpriteRenderer[] sprites;

    public float movingSpeed;

    public float leftBound;

    public Sprite image;
    public float space;

    // Start is called before the first frame update
    void Start()
    {
        sprites = GetComponentsInChildren<SpriteRenderer>();
        if (image)
        {
            foreach (var s in sprites)
            {
                s.sprite = image;
            }
            
        }

        for (int i = 1; i < sprites.Length;i++)
        {
            SpriteRenderer s = sprites[i];
            // Get the size of the sprite in world units
            float spriteSize = s.sprite.rect.width / s.sprite.pixelsPerUnit;
            // Align the sprite to the right of the previous one
            s.transform.localPosition = sprites[i-1].transform.localPosition + (spriteSize + space) * Vector3.right;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        foreach (SpriteRenderer sr in sprites)
        {
            
            sr.transform.localPosition += movingSpeed * Vector3.left;
            if (sr.transform.position.x < leftBound)
            {
                float spriteSize = sr.sprite.rect.width / sr.sprite.pixelsPerUnit;
                sr.transform.localPosition += (spriteSize+space)* sprites.Length * Vector3.right;
            }
        }
    }
}
