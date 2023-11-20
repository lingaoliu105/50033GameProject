using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Enemy {
    [CreateAssetMenu(menuName = "PluggableSM/Decisions/Target Exist")]
    public class TargetExistDecision : Decision {
        public override bool Decide(StateController controller) {
            EnemyController enemyController = (EnemyController)controller;
            return enemyController.HasTarget();
        }
    }
}
