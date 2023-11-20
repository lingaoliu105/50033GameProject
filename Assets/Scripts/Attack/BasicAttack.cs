
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

namespace Assets.Scripts.Attack {
    [Serializable]
    public abstract class MeleeAttack : IAttack
        {
        public int BeforeAttackFrames { get; set; }
        public float BeforeAttackMaxSpeed { get; set; }
        public int AfterAttackFrames { get; set; }
        public float AfterAttackMaxSpeed { get; set; }
        public int AttackFrames { get; set; }
        public float AttackMaxSpeed { get; set; }
        public float MotionValue { get; set; }
        public int Damage { get; set; }
        public bool DoScreenShake { get; set; }
        public bool DoFrozenFrame { get; set; }
        public int StaminaCost { get; set; }
        public int ElecCost { get; set; }

        public String AnimationTrigger { get; set; }

        public GameObject AttackPrefab;
        public PlayerController Player;
        public MeleeAttack(GameObject AttackPrefab, PlayerController player, int Damage) {
            this.AttackPrefab = AttackPrefab;
            this.Player = player;
            this.Damage = Damage;
        }

        public virtual void PerformAttack() {
            Player.PlayAnimation(AnimationTrigger);
            Player.StartCoroutine(BeforeAttack());
        }
        private IEnumerator BeforeAttack() {
            for (int i = 0; i < BeforeAttackFrames; i++) {
                yield return null;
            }
            GameObject attack = GameObject.Instantiate(AttackPrefab, Player.transform.position + AttackPrefab.transform.position, Player.transform.rotation);
            attack.GetComponent<BasicPlayerProjectile>().attackDamage = (int)(Damage * this.MotionValue);
            //Debug.Log("Projectile: " + attack + " Damage: " + Damage);
            attack.transform.parent = Player.transform;
            if (Player.Facing == Facings.Left) {
                attack.transform.localScale = new Vector3(-attack.transform.localScale.x, attack.transform.localScale.y, attack.transform.localScale.z);
            }
        }
    }
    [Serializable]
    public abstract class RangedAttack : IAttack { 
        public int ShootingAnimationFrames { get; set; }
        public int ShootingAnimationMaxSpeed { get; set; }

        public float MotionValue { get; set; }
        public int Damage { get; set; }
        public bool DoScreenShake { get; set; }
        public bool DoFrozenFrame { get; set; }
        public String AnimationTrigger { get; set; }

        public GameObject AttackPrefab;
        public PlayerController Player;
        public int StaminaCost { get; set; }
        public int ElecCost { get; set; }

        public RangedAttack(GameObject AttackPrefab, PlayerController player, int Damage) {
            this.AttackPrefab = AttackPrefab;
            this.Player = player;
            this.Damage = Damage;
        }

        public void PerformAttack() {
            Player.PlayAnimation(AnimationTrigger);
            Player.StartCoroutine(BeforeAttack());
        }

        private IEnumerator BeforeAttack() {
            for (int i = 0; i < ShootingAnimationFrames; i++) {
                yield return null;
            }
            Vector2 direction = new Vector2(Player.Facing == Facings.Left ? -1 : 1, 0);
            Vector3 offset = new Vector3((Player.Facing == Facings.Left ? -1 : 1) * AttackPrefab.transform.position.x, AttackPrefab.transform.position.y,0);
            GameObject attack = GameObject.Instantiate(AttackPrefab, Player.transform.position + offset, Player.transform.rotation);
            attack.GetComponent<BasicPlayerProjectile>().direction = direction;
            attack.GetComponent<BasicPlayerProjectile>().attackDamage = (int)(Damage * this.MotionValue);
            //Debug.Log("Projectile: "+attack+" Damage: "+Damage);
            if (Player.Facing == Facings.Left) {
                attack.transform.localScale = new Vector3(-attack.transform.localScale.x, attack.transform.localScale.y, attack.transform.localScale.z);
            }
        }
    }
}
