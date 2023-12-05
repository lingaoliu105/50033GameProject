using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items {
    public class ChestObject: ItemObject {
        public int type;
        public int Soul;
        public void Start() {
            this.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(wait());
        }
        public IEnumerator wait() {
            yield return new WaitForSeconds(0.5f);
            this.GetComponent<Collider2D>().enabled = true;
        }

        public override void PickedUP(PlayerController player) {
            base.PickedUP(player);
            if(type == 0) {
                player.GainSoul(Soul);
            }
            if (type == 2) {
                player.MaxBlueTubes++;
                player.RechargeAllTubes();
            }
            if (type == 1) {
                player.MaxRedTubes++;
                player.RechargeAllTubes();
            }
        }

    }
}
