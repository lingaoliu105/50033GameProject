using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface EnemyBehaviourInterface
{
    void PrimaryAttack();
    void Move();
    float hp { get; set; }
}
