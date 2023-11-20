using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(menuName = "Enemy/Start Patrol")]
    public class StartPatrolAction : Action
    {
        public override void Act(StateController controller)
        {
            EnemyController enemyController = (EnemyController)controller;
            enemyController.StartPatrol();
        }
    }
}