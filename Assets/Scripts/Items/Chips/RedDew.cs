using Assets.Scripts.Buff;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Items {
    public class RedDew : EquipableItem {
        public BasicBuff buff = new RedDewBuff();
        public override void Update() {
            base.Update();
            if (player == null) {
                return;
            }
            if (((float)player.HP / (float)player.MaxHP)<=0.3f) { 
                player.AddBuff(buff);
            } else {
                player.RemoveBuff(buff);
            }
        }
    }

    public class RedDewBuff : BasicBuff {
        public RedDewBuff() {
            this.id = 3;
            this.icon = 3;
            this.name = "Add Attack";
            this.description = "";
            this.isStackable = false;
            this.maxStack = 1;
            this.stack = 1;
            this.isPermanent = true;
            this.currentID = -1;
        }
        public override void OnAdd() {
            base.OnAdd();
        }
        public override void OnRemove() {
            base.OnRemove();
            this.player.CurrentMeleeWeapon.TempMultiplication = 1f;
        }
        public override void OnUpdate(float deltatime) {
            base.OnUpdate(deltatime);
            this.player.CurrentMeleeWeapon.TempMultiplication = 2f;
        }
    }
}
