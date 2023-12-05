using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Map {
    public class SceneChange:MapObjectController {
        public int sceneId;
        public override void Interact(int triggerId) {
            GameManager.Instance.ChangeScene(sceneId);
        }
    }
}
