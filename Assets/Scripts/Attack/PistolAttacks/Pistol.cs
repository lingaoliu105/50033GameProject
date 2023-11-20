using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Attack.PistolAttacks {

    public enum PistolAttackState {
        Default = -1,
        Shoot = 0,
    }
    [CreateAssetMenu(fileName = "Pistol", menuName = "Weapons/Pistol", order = 2)]
    public class Pistol : Weapon {
        public IAttack CurrentAttack { get; set; }
        public GameObject[] AttackPrefabs;
        public PistolAttackState CurrentState { get; set; }
        public override IAttack GetCurrentAttack() {
            return CurrentAttack;
        }


        public override IAttack GetNextAttack(AttackKey key) {
            if (key == AttackKey.Shoot) {
                switch (CurrentState) {
                    case PistolAttackState.Default:
                        CurrentAttack = new PistolAttack(AttackPrefabs[0], Player, BasicDamage);
                        CurrentState = PistolAttackState.Shoot;
                        return CurrentAttack;
                    case PistolAttackState.Shoot:
                        CurrentAttack = new PistolAttack(AttackPrefabs[0], Player, BasicDamage);
                        CurrentState = PistolAttackState.Shoot;
                        return CurrentAttack;
                }
            }
            return null;
        }
    }
}
