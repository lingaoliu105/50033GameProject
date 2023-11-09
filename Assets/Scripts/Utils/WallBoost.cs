using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    public class WallBoost {
        private float timer;
        private int dir;
        private PlayerController controller;
        public float Timer => timer;
        public WallBoost(PlayerController playerController) {
            this.controller = playerController;
            this.dir = 0;
            this.ResetTime();
        }

        public void ResetTime() {
            this.timer = 0;
        }

        public void Update(float deltaTime) {
            if (timer > 0) {
                timer -= deltaTime;
                if (controller.MoveX == dir) {
                    controller.Speed.x = Constants.WallJumpHSpeed * controller.MoveX;
                    timer = 0;
                }
            }
        }

        //跳跃时，激活
        public void Active() {
            if (controller.MoveX == 0) {
                Debug.Log("====WallBoost");
                this.dir = -(int)controller.Facing;
                this.timer = Constants.ClimbJumpBoostTime;
            }
        }


    }
}
