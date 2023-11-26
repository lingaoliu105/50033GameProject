using Assets.Scripts.Enemy;
using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyManager:MonoBehaviour {
    public EffectManager EffectManager;
    public PlayerController PlayerController;
    public GameObject Boss1Prefab;

    public void GenerateBoss1(Vector2 position) {
        GameObject boss1 = Instantiate(Boss1Prefab, position, Quaternion.identity);
        Boss1Controller boss1Controller = boss1.GetComponent<Boss1Controller>();
        boss1Controller.EffectManager = EffectManager;
        boss1Controller.PlayerController = PlayerController;
    }
}

