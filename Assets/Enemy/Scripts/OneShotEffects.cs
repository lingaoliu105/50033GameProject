using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

namespace Enemy {
    public class OneShotEffects : MonoBehaviour, IEffect {
        public void SelfDestroy() {
            Destroy(gameObject);
        }
    }
}
