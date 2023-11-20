using Assets.Scripts.Buff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Items {
    public enum TubeType { 
        None = -1,
        HP = 1,
        MP = 2,
        InfiMp = 3,
        InfiStamina = 4,
    }
    public class VaccTube: ItemBase {
        public TubeType tubeType;
        public override void Use() {
            if (this.tubeType == TubeType.HP) {
                this.player.RestoreHP((int)(80f * Level - 0.2f * Level * Level - 0.1f * Level * Level * Level + 250f));
            } else if (this.tubeType == TubeType.MP) {
                this.player.RestoreElec((int)(22f * Level - 0.1f * Level * Level - 0.05f * Level * Level * Level + 30f));
            } else if (this.tubeType == TubeType.InfiMp) {
                this.player.AddBuff(new InfinityElec(6f+0.2f*Level));
            } else if (this.tubeType == TubeType.InfiStamina) {
                this.player.AddBuff(new InfinityStamina(10f+0.5f*Level));
            }
        }
        public VaccTube(int Level, TubeType tubeType) {
             this.Level = Level;
            this.tubeType = tubeType;

        }

    }
}
