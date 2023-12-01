using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainShake : MonoBehaviour
{
    private Rigidbody2D[] carriages;
    // Start is called before the first frame update
    void Start()
    {
        carriages = GetComponentsInChildren<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var rand = Random.value;
        if (rand < 0.1)
        {
            var rand2 = Mathf.RoundToInt(Random.value * 5);
            carriages[rand2].AddForce(3*((Random.value) - 0.5f)*Vector2.up + ((Random.value) - 0.5f)*Vector2.right,ForceMode2D.Impulse);
        }

    }
}
