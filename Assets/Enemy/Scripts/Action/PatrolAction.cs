using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Enemy/Patrol")]
    public class PatrolAction : Action
    {
        public override void Act(StateController controller)
        {
            EnemyController enemyController = (EnemyController)controller;
            enemyController.Patrol();
        }
    }
}