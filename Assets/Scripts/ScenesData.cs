using Assets.Scripts.Camera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[CreateAssetMenu(fileName = "Scenes", menuName = "LevelInfo/Scenes", order = 2)]
public class ScenesData : ScriptableObject {
    public LevelInfo[] Levels;
    public string[] ScenesName;
    public GameObject[] Backgrounds;
}