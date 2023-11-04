using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotEffects : MonoBehaviour
{
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
