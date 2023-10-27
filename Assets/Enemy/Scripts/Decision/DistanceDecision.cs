
using UnityEngine;
using System;
using Game;
using Unity.Mathematics;

[CreateAssetMenu(menuName = "PluggableSM/Decisions/Horizental Close")]
public class DistanceHorizentalCloseToDecision : Decision {
    public PlayerController Target;
    public float MinDistance;

    public override bool Decide(StateController controller) {
        EnemyController m = (EnemyController)controller;
        float target = Target.transform.position.x;
        float current = m.position.x;
        float distance = math.abs(target - current);
        return distance < MinDistance;
    }
}

[CreateAssetMenu(menuName = "PluggableSM/Decisions/Horizental Far")]
public class DistanceHorizentalFarAsDecision : Decision {
    public PlayerController Target;
    public float MaxDistance;

    public override bool Decide(StateController controller) {
        EnemyController m = (EnemyController)controller;
        float target = Target.transform.position.x;
        float current = m.position.x;
        float distance = math.abs(target - current);
        return distance > MaxDistance;
    }
}


