using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Enemy {
    public class PlaneStraightAttack : BasicEnemyProjectile {
        public float speed = 2f;
        public Vector2 dir = Vector2.zero;
        public override void Start() {
            base.Start();
            this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
        public void Update() {
            this.GetComponent<Rigidbody2D>().velocity = dir * speed;
        }
    }
}
