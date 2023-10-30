using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    public class EffectManager: Singleton<EffectManager> {
        public SceneCamera gameCamera;
        private float freezeTime;

        public void Update() {
            float deltaTime = Time.unscaledDeltaTime;
        }
        public void CameraShake(Vector2 dir) {
            this.gameCamera.Shake(dir, 0.2f);
        }
        public bool UpdateTime(float deltaTime) {
            if (freezeTime > 0f) {
                freezeTime = Mathf.Max(freezeTime - deltaTime, 0f);
                return false;
            }
            if (Time.timeScale == 0) {
                Time.timeScale = 1;
            }
            return true;
        }

        //冻帧
        public void Freeze(float freezeTime) {
            this.freezeTime = Mathf.Max(this.freezeTime, freezeTime);
            if (this.freezeTime > 0) {
                Time.timeScale = 0;
            } else {
                Time.timeScale = 1;
            }
        }

    }
}
