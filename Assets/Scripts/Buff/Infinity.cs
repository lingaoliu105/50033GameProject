using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Buff {
    public class InfinityElec: BasicBuff {
        public InfinityElec() {
            this.id = 0;
            this.icon = 0;
            this.name = "Infinity Mana";
            this.description = "You have infinity mana";
            this.isStackable = false;
            this.maxStack = 1;
            this.stack = 1;
            this.duration = 0;
            this.currentID = -1;
            this.player = null;
        }
        public InfinityElec(float duration) {
            this.id = 0;
            this.icon = 0;
            this.name = "Infinity Mana";
            this.description = "You have infinity mana";
            this.isStackable = false;
            this.maxStack = 1;
            this.stack = 1;
            this.duration = duration;
            this.currentID = -1;
        }
        private int currentElec;
        public override void OnAdd() {
            base.OnAdd();
            currentElec = this.player.Elec;
        }
        public override void OnRemove() {
            base.OnRemove();
        }
        public override void OnUpdate(float deltatime) {
            base.OnUpdate(deltatime);
            this.player.Elec = currentElec;
        }
    }
    public class Invinsible: BasicBuff {
        public Invinsible() {
            this.id = 1;
            this.icon = 1;
            this.name = "Invinsible";
            this.description = "You are invinsible";
            this.isStackable = false;
            this.maxStack = 1;
            this.stack = 1;
            this.duration = 0;
            this.currentID = -1;
            this.player = null;
        }
        public Invinsible(float duration) {
            this.id = 1;
            this.icon = 1;
            this.name = "Invinsible";
            this.description = "You are invinsible";
            this.isStackable = false;
            this.maxStack = 1;
            this.stack = 1;
            this.duration = duration;
            this.currentID = -1;
        }
        private int currentHP;
        public override void OnAdd() {
            base.OnAdd();
            currentHP = this.player.HP;
        }
        public override void OnRemove() {
            base.OnRemove();
        }
        public override void OnUpdate(float deltatime) {
            base.OnUpdate(deltatime);
            this.player.HP = currentHP;
        }
        
    }
    public class InfinityStamina : BasicBuff {
        public InfinityStamina() {
            this.id = 2;
            this.icon = 2;
            this.name = "Infinity Stamina";
            this.description = "You have infinity stamina";
            this.isStackable = false;
            this.maxStack = 1;
            this.stack = 1;
            this.duration = 0;
            this.currentID = -1;
            this.player = null;
        }
        public InfinityStamina(float duration) {
            this.id = 2;
            this.icon = 2;
            this.name = "Infinity Stamina";
            this.description = "You have infinity stamina";
            this.isStackable = false;
            this.maxStack = 1;
            this.stack = 1;
            this.duration = duration;
            this.currentID = -1;
        }
        private int currentStamina;
        public override void OnAdd() {
            base.OnAdd();
            currentStamina = this.player.MaxStamina;
        }
        public override void OnRemove() {
            base.OnRemove();
        }
        public override void OnUpdate(float deltatime) {
            base.OnUpdate(deltatime);
            this.player.Stamina = currentStamina;
        }
    }
}
