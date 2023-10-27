using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Game {
    [CreateAssetMenu(menuName = "Enemy/StartAttack")]

    public class StartAttackAction : Action {
        public GameObject attackTemplate;
        public override void Act(StateController controller) {
            EnemyController m = (EnemyController)controller;
            Vector2 dir = m.position.x <= m.playerPos.x ? Vector2.right : Vector2.left;
            GameObject go = Instantiate(attackTemplate, m.position, Quaternion.identity);
            go.GetComponent<PlaneStraightAttack>().dir = dir;
        }
    }
} 