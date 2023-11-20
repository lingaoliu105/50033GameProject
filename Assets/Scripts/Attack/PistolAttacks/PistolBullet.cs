using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scripts.Attack {
    public class PistolBullet : BasicPlayerProjectile {
        public override void CollideWithEnemy(GameObject enemy) {
            base.CollideWithEnemy(enemy);
        }
        public override void CollideWithScene(GameObject other) {
            base.CollideWithScene(other);
        }
        public override void UpdateDeltaTime(float deltaTime) {
            base.UpdateDeltaTime(deltaTime);
        }
    }
}
