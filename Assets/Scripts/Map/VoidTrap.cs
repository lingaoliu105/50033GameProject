using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Game;
namespace Assets.Scripts.Map {
    public class VoidTrap: MonoBehaviour{
        public float damageInterval = 1.0f; // 伤害触发间隔，单位秒
        private float lastDamageTime;
        public Vector2 BackDist;
        public int Damage;
        public AudioSource audioSource;
        public AudioClip clipToPlay;
        void Update()
        {
            lastDamageTime += Time.deltaTime; // 更新时间
        }

        public void GetOut(PlayerController player) { 
            if (lastDamageTime >= damageInterval)
            {
                player.Speed = Vector2.zero;
                player.Position -= BackDist;
                player.TakeDamage(Damage, "Fallen");
                audioSource.PlayOneShot(clipToPlay);
                // 重置计时器
                lastDamageTime = 0;
            }
            else{
                return;
            }
        }
    }
}
