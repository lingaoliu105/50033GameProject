using Assets.Scripts.Attack.PistolAttacks;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Attack {
    public enum KatanaAttackState {
        Default = -1,
        Light1 = 0,
        Light2 = 1,
    }
    [CreateAssetMenu(fileName = "Katana", menuName = "Weapons/Katana", order = 1)]
    public class Katana:  Weapon {
        public IAttack CurrentAttack { get; set; }
        public GameObject[] AttackPrefabs;
        public KatanaAttackState CurrentState { get; set; }
        

        public override IAttack GetCurrentAttack() {
            return CurrentAttack;
        }

        public override IAttack GetNextAttack(AttackKey key) {
            if (key == AttackKey.Light) {
                switch (CurrentState) { 
                    case KatanaAttackState.Default:
                        CurrentAttack = new KatanaAttackLight1(AttackPrefabs[0], Player, BasicDamage);
                        CurrentState = KatanaAttackState.Light1;
                        return CurrentAttack;
                    case KatanaAttackState.Light1:  
                        CurrentAttack = new KatanaAttackLight2(AttackPrefabs[1], Player, BasicDamage);
                        CurrentState = KatanaAttackState.Light2;
                        return CurrentAttack;
                    case KatanaAttackState.Light2:
                        CurrentAttack = new KatanaAttackLight1(AttackPrefabs[0], Player, BasicDamage);
                        CurrentState = KatanaAttackState.Light1;
                        return CurrentAttack;
                }
            }
            else if (key == AttackKey.Break) {
                CurrentState = KatanaAttackState.Default;
            }
            return CurrentAttack;
        }

    }
}
