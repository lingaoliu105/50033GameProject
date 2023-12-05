using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Game;
namespace Assets.Scripts.Map {
    public class VoidArea: MonoBehaviour{
        public Vector2 BackTo;
        public int Damage;
        public AudioSource audioSource;
        public AudioClip clipToPlay;

        public void GetOut(PlayerController player) { 
            player.Speed = Vector2.zero;
            player.Position = BackTo;
            player.TakeDamage(Damage, "Fallen");
            audioSource.PlayOneShot(clipToPlay);
        }
    }
}
