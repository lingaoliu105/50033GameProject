using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Enemy/Petrol")]
    public class PetrolAction : Action
    {
        public override void Act(StateController controller)
        {
            EnemyController enemyController = (EnemyController)controller;
            enemyController.Patrol();
        }
    }
}