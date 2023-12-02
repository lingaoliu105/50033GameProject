using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Enemy {

    public abstract class EnemyAttack: MonoBehaviour
    {
        public abstract IEnumerator WaitAndDestroy();
        public AudioClip hitAudio;

        public int Damage; 
        public float TimeToDestroy;
        protected AudioSource attackAudio;

        protected virtual void Start()
        {
            attackAudio = GetComponent<AudioSource>();
        }

        public abstract void Hitting();
        public abstract void  OnCollisionEnter2D(Collision2D collision);

        public virtual bool CheckActive()
        {
            return true;
        }
    }

    interface IEffect
    {
        void SelfDestroy();
    }
}
