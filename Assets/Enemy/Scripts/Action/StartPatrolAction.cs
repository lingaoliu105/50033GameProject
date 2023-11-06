using UnityEngine;

namespace Game
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