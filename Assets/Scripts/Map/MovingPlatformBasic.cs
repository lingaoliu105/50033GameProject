using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Map {
    public class MovingPlatformBasic: MonoBehaviour {
        public Vector2 Speed;
        public Vector2 JumpBoost;
        public virtual void Update() {
            transform.position += (Vector3)Speed * Time.deltaTime;
        }
    }
}
