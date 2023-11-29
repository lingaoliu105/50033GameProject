using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Enemy {
    internal interface IAttack {
        IEnumerator WaitAndDestroy();

        void OnCollisionEnter2D(Collision2D collision);
    }

    public abstract class EnemyAttack: MonoBehaviour, IAttack
    {
        public abstract IEnumerator WaitAndDestroy();

        public int Damage; 
        public float TimeToDestroy;
        public bool hasHit = false;
        public abstract void Hitting();
        public abstract void  OnCollisionEnter2D(Collision2D collision);
    }

    interface IEffect
    {
        void SelfDestroy();
    }
}
