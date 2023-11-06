using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Decisions/Destination Reached")]

public class DestinationReachedDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        EnemyController enemyController = (EnemyController)controller;
        return enemyController.ReachedDestination();
    }
}