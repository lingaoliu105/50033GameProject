using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game {
    public class JumpCheck {
        private float timer;

        private PlayerController controller;
        public float Timer { get => timer; }
        private bool jumpGrace; //土狼时间

        public JumpCheck(PlayerController controller, bool jumpGrace) {
            this.controller = controller;
            ResetTimer();
            this.jumpGrace = jumpGrace;
        }

        public void ResetTimer() {
            timer = 0;
        }   

        public void Update(float deltatime) {
            if (controller.OnGround) {
                timer = Constants.JumpGraceTime;
            } else {
                if (timer > 0) {
                    timer -= deltatime;
                }   
            }
        }

        public bool AllowJump() {
            return jumpGrace ? timer > 0 : controller.OnGround;
        }

    }
}
