using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Attack {
    public enum AttackKey {
        Break = -1,
        Light = 0,
        Heavy = 1,
        Shoot = 10,
    }
    public interface IWeapon {
        public PlayerController Player { get; set; }
        public abstract IAttack GetNextAttack(AttackKey key);
        public abstract IAttack GetCurrentAttack();
    }
    [Serializable]
    public abstract class Weapon : ScriptableObject, IWeapon {
        public PlayerController Player { get; set; }
        public int BasicAtk;
        public float STRFix;
        public float TECFix;
        public float LUCFix;
        public float DEXFix;
        public float TempMultiplication = 1;
        public int BasicDamage => GetBasicDamage();

        public virtual int GetBasicDamage() {
            return (int)(BasicAtk*TempMultiplication*(1 + STRFix * Player.STRFix + TECFix * Player.TECFix + LUCFix * Player.LUCFix + DEXFix * Player.DEXFix));
        }

        public abstract IAttack GetCurrentAttack();
        public abstract IAttack GetNextAttack(AttackKey key);
    }
}
