using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Player/Player Infomation")]
public class PlayerInfo: ScriptableObject {
    [Header("Auto Updated by Frame")]
    public Vector2 position;
    public Vector2 velocity;
    public bool isAlive = false;

    [Header("Player Resources")]
    public int health = 10;
    public int coin = 0;
    public int mana = 100;
    public int stamina = 100;

    [Header("Record")]
    public int highestScore = 0;
    public int currentLevel = 0;

}

