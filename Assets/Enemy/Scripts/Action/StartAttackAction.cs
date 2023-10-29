using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Game {
    [CreateAssetMenu(menuName = "Enemy/StartAttack")]

    public class StartAttackAction : Action {
        public GameObject attackTemplate;
        public override void Act(StateController controller) {
            EnemyController m = (EnemyController)controller;
            m.Attack();
        }
    }
} 