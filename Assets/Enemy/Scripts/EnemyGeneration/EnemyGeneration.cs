using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGeneration", menuName = "Level/EnemyGeneration")]
public class EnemyGeneration : ScriptableObject {
    public EnemyWave[] enemyWaves;
}