using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game {
    public class EffectManager: MonoBehaviour {
        public SceneCamera gameCamera;
        public Volume volume; // 引用包含Volume组件的游戏对象
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

        
        private ChromaticAberration chromaticAberration;

        void Start() {
            // 获取Chromatic Aberration效果
            if (volume.profile.TryGet(out ChromaticAberration ca)) {
                chromaticAberration = ca;
                chromaticAberration.active = false;
            }
        }

        public void ToggleChromaticAberration(bool isEnabled) {
            if (chromaticAberration != null) {
                if(isEnabled) {
                    chromaticAberration.active = true;
                } else {
                    chromaticAberration.active = false;
                }
            }
        }

        private IEnumerator ChromaticAbberationOn(float duration) {
            float timer = 0f;
            chromaticAberration.active = true;
            while (timer < duration) {
                timer += Time.deltaTime;
                chromaticAberration.intensity.value = Mathf.Lerp(0f, 1f, timer / duration);
                yield return null;
            }
        }

        private IEnumerator ChomaticAbberationOff(float duration) {
            float timer = 0f;
            while (timer < duration) {
                timer += Time.deltaTime;
                chromaticAberration.intensity.value = Mathf.Lerp(1f, 0f, timer / duration);
                yield return null;
            }
            chromaticAberration.active = false;
        }

    }
}
