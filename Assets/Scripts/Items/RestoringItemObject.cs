using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items {
    public class RestoringItemObject: ItemObject {
        public int HP;
        public int MP;

        public override void PickedUP(PlayerController player) {
            base.PickedUP(player);
            player.RestoreHP(HP);
            player.RestoreElec(MP);
        }

    }
}
