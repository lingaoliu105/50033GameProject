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
        
        public GameObject Background;

        public ParallaxController[] parallaxControllers;

        public Volume volume; // 引用包含Volume组件的游戏对象
        public GameObject AbsorbEffectPrefab;
        private float freezeTime;

        public void Update() {
            float deltaTime = Time.unscaledDeltaTime;
            if (parallaxControllers.Length>0){
                for(int i =0; i < parallaxControllers.Length; i++){
                    parallaxControllers[i].CameraPosition = gameCamera.mainCamera.transform.position;
                }
            }
        }
        public void CameraShake(Vector2 dir) {
            this.gameCamera.Shake(dir, 0.2f);
        }
        public void CameraShake(Vector2 dir, float time) {
            this.gameCamera.Shake(dir, time);
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
        public void PlayAbsorbEffect(PlayerController player) {
            GameObject effect = Instantiate(AbsorbEffectPrefab, player.transform.position, Quaternion.identity);
            effect.transform.parent = player.transform;
            effect.transform.localPosition = Vector3.zero;
            effect.transform.localScale = Vector3.one;
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

        public void LoadParallax(){
            parallaxControllers = new ParallaxController[Background.transform.childCount];
            //Debug.Log(Background.transform.childCount);

            for (int i =0; i<= Background.transform.childCount -1; i++) {
                //Debug.Log(i);
                parallaxControllers[i] = Background.transform.GetChild(i).GetComponent<ParallaxController>();
                //Debug.Log("Loading "+transform.GetChild(i));
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
