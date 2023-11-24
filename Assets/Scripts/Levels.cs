using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "LevelInfo/Level", order = 1)]
public class LevelInfo : ScriptableObject { 
    public Vector2 PlayerPosition;
    public Vector2 CameraStartPos;
    public bool CameraLocked;
    public float CameraSize;
}