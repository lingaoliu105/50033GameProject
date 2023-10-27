using UnityEngine;

namespace Game {
    [CreateAssetMenu(menuName = "Enemy/StartScout")]

    public class StartScoutAction : Action {
        public float scoutSpeed = -1f;
        public override void Act(StateController controller) {
            EnemyController m = (EnemyController)controller;
            m.StartScout(scoutSpeed);
        }
    }
} 