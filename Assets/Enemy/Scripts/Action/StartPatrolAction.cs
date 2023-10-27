using UnityEngine;

namespace Game {
    [CreateAssetMenu(menuName = "Enemy/StartPatrol")]

    public class StartPatrolAction : Action {
        public float patrolSpeed = -1f;
        public float patrolRangeD = -5f;
        public float patrolRangeL = -5f;
        public float patrolRangeR = 5f;
        public float patrolRangeU = 5f;
        public override void Act(StateController controller) {
            EnemyController m = (EnemyController)controller;
            m.SetPatrolRange(patrolRangeL, patrolRangeR, patrolRangeU, patrolRangeD);
            m.StartPatrol(patrolSpeed);
        }
    }
}