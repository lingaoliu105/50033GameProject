using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Enemy/Scout")]
    public class ScoutAction : Action
    {
        public override void Act(StateController controller)
        {
            EnemyController enemyController = (EnemyController)controller;
            enemyController.Scout();
        }
    }
}