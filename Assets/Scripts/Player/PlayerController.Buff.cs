using Assets.Scripts.Buff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public partial class PlayerController {
        public List<BasicBuff> buffs;
        public void AddBuff(BasicBuff buff) {
            if (buff.isStackable) { 
                for (int i = 0; i < this.buffs.Count; i++) {
                    if (this.buffs[i].id == buff.id) {
                        if (this.buffs[i].stack < this.buffs[i].maxStack) {
                            this.buffs[i].stack++;
                            
                            this.buffs[i].currentID = i;
                            this.buffs[i].player = this;

                            this.buffs[i].OnAdd();
                            return;
                        }
                    }
                }
            } else {
                for (int i = 0; i < this.buffs.Count; i++) {
                    if (this.buffs[i].id == buff.id) {
                        
                        this.buffs[i].currentID = i;
                        this.buffs[i].player = this;

                        this.buffs[i].OnAdd();
                        return;
                    } else if (this.buffs[i].currentID == -1) {
                        this.buffs[i] = buff;
                        
                        this.buffs[i].currentID = i;
                        this.buffs[i].player = this;

                        this.buffs[i].OnAdd();
                        return;
                    }
                }
            }
            int length = this.buffs.Count;
            this.buffs.Add(buff);
            
            this.buffs[length].currentID = length;
            this.buffs[length].player = this;

            this.buffs[length].OnAdd();
        }
        public void RemoveBuff(BasicBuff buff) { 
            for (int i = 0; i < this.buffs.Count; i++) {
                if (this.buffs[i].id == buff.id) {
                    this.buffs[i].OnRemove();
                    return;
                }
            }
        }
        public void UpdateBuff(float deltatime) { 
            for (int i = 0; i < this.buffs.Count; i++) {
                if (this.buffs[i].currentID != -1) {
                    this.buffs[i].OnUpdate(deltatime);
                }
            }
        }
    }
}
