using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Items {
    public abstract class ItemBase {
        public string Name;
        public int ID;
        public string Description;
        public int MaxStack = 1;
        public int CurrentStack = 1;
        public int Price;
        public int SellPrice;
        public int Rarity;
        public int Number;
        public int Level = 0;

        public PlayerController player;
        public virtual void Use() { return; }
    }

    public abstract class EquipableItem: ItemBase{
        public int UpdateTime;
        public virtual void OnMeleeAttackHit() {  }
        public virtual void OnMeleeAttack() { }
        public virtual void OnRangedAttackHit() {  }
        public virtual void OnRangedAttack() {  }
        public virtual void OnHurt() {  }
        public virtual void OnDeath() { }
        public virtual void Update() { }
    }
}
