
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerParams", menuName = "Player/Player Param")]
public class PlayerParams : ScriptableObject {

    public void SetReloadCallback(Action onReload) {
        
    }

    public void OnValidate() {
        ReloadParams();
    }

    public void ReloadParams() {
        //Debug.Log("=======更新所有Player配置参数");

    }
}