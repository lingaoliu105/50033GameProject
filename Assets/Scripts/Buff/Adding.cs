using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Buff {
    public class AddAtk : BasicBuff {
        public float multiplier;
        public AddAtk(float duration, float multiplier) {
            this.id = 3;
            this.icon = 3;
            this.name = "Add Attack";
            this.description = "";
            this.isStackable = false;
            this.maxStack = 1;
            this.stack = 1;
            this.duration = duration;
            this.currentID = -1;
            this.multiplier = multiplier;
        }
        public override void OnAdd() {
            base.OnAdd();
        }
        public override void OnRemove() {
            base.OnRemove();
            this.player.CurrentMeleeWeapon.TempMultiplication = 1;
        }
        public override void OnUpdate(float deltatime) {
            base.OnUpdate(deltatime);
            this.player.CurrentMeleeWeapon.TempMultiplication = multiplier;
        }
    }
}
