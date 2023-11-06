using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    [CreateAssetMenu(menuName = "Enemy/StartIdle")]
    public class StartIdle : Action {

        public override void Act(StateController controller) {
            EnemyController m = (EnemyController)controller;
            m.StartIdle();
        }
    }
}
