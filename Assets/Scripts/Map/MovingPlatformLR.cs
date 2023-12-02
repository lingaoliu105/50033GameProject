using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Map {
    public class MovingPlatformLR: MovingPlatformBasic {
        public override void Update() {
            if (transform.position.x > 5) {
                Speed.x = -Math.Abs(Speed.x);
            }
            if (transform.position.x < -5) {
                Speed.x = Math.Abs(Speed.x);
            }
            if (transform.position.y > 2) {
                Speed.y = -Math.Abs(Speed.y);
            }
            if (transform.position.y < 0) {
                Speed.y = Math.Abs(Speed.y);
            }
            base.Update();
        }
    }
}
