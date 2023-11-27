using Assets.Scripts.Buff;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items {

    public class Cat : EquipableItem {//101
        public override void OnHurt() {
            base.OnHurt();
            if (player == null) {
                return;
            }
            if (player.DamageTag == "Fallen") {
                Debug.Log("Cat: ");
                player.DamageToTake = 1;
            }
        }
    }
}
