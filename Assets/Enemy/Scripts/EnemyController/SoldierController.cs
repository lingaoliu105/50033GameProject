using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class SoldierController : EnemyController
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        hp = gameConstants.soldierMaxHP;
    }

}
