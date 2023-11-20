using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Buff {
    public interface IBuff {
        void OnAdd();
        void OnRemove();
        void OnUpdate(float deltaTime);
    }
    [Serializable]
    public class BasicBuff: IBuff {
        public int id;
        public int icon;
        public int currentID;
        public string name;
        public string description;
        public float duration;
        public float time;
        public bool isPermanent;
        public bool isStackable;
        public int maxStack;
        public int stack;
        public bool isDebuff;
        public PlayerController player;
        public BasicBuff() {
            this.id = 0;
            this.name = "";
            this.description = "";
            this.duration = 0;
            this.time = 0;
            this.isPermanent = false;
            this.isStackable = false;
            this.maxStack = 0;
            this.stack = 0;
        }
        public virtual void OnAdd() {
            if (this.isPermanent) {
                this.time = 999;
            } else {
                this.time = this.duration;
            }
        }

        public virtual void OnRemove() {
            if (this.currentID == -1) return;
            this.currentID = -1;
        }

        public virtual void OnUpdate(float deltaTime) {
            if (this.currentID == -1) return;
            if (this.isPermanent) {
                this.time = 999;
            } else {
                this.time -= deltaTime;
                
            }
            if (this.time <= 0) {
                this.player.RemoveBuff(this);
            }

        }
        
    }
}
