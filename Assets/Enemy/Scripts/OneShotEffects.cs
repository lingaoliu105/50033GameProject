using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class OneShotEffects : MonoBehaviour,IEffect
{
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
