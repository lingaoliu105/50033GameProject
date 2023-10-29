using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Decisions/Attack Reachable")]

public class AttackReachableDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        EnemyController enemyController = (EnemyController)controller;
        return enemyController.AttackCanReach();
    }
}
