using Assets.Scripts.Camera;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;

namespace Game {
    public class SceneCamera : MonoBehaviour, ICamera {
        [SerializeField]
        private Camera mainCamera;
        //[SerializeField]
        //private Camera postCamera;

        private Vector2 offset;
        public bool IsLocked  = false;
        public CameraLayer[] CameraLayers;
        [HideInInspector]
        public PlayerSpriteRenderer PlayerSpriteRenderer;
        public Vector2 LockedCameraPos = Vector2.zero;
        public int layer = 0;
        [SerializeField]
        private float ShakeStrength = 1;
        [SerializeField]
        private AnimationCurve ShakeCurve = new AnimationCurve(new Keyframe[]
            {
                new Keyframe(0, -1.4f, -7.9f, -7.9f),
                new Keyframe(0.27f, 0.78f, 23.4f, 23.4f),
                new Keyframe(0.54f, -0.12f, 22.6f, 22.6f),
                new Keyframe(0.75f, 0.042f, 9.23f, 9.23f),
                new Keyframe(0.9f, -0.02f, 5.8f, 5.8f),
                new Keyframe(0.95f, -0.006f, -3.0f, -3.0f),
                new Keyframe(1, 0, 0, 0)
            });


        public void SetCameraPositionByPlayerPosition(Vector2 playerPosition) {
            layer = 0;
            float LayerBottom = 0;
            float LayerTop = 0; // 5f
            float FixedAreaTop = 0;
            float FixedY = 0; // -1f
            float NextFixedY = 0; // 6f
            if (playerPosition.y >= CameraLayers[CameraLayers.Length - 1].LayerBottom) {
                layer = CameraLayers.Length - 1;
                NextFixedY = -1;
                LayerBottom = CameraLayers[CameraLayers.Length - 1].LayerBottom;
                LayerTop = CameraLayers[CameraLayers.Length - 1].hasTop ? CameraLayers[CameraLayers.Length - 1].LayerTop : -1;
                FixedAreaTop = CameraLayers[CameraLayers.Length - 1].FixedAreaTop;
                FixedY = CameraLayers[CameraLayers.Length - 1].FixedY;
            } else {
                for (int i = 0; i < CameraLayers.Length - 1; i++) {
                    if (playerPosition.y >= CameraLayers[i].LayerBottom && playerPosition.y <= CameraLayers[i + 1].LayerBottom) {
                        layer = i;
                        LayerBottom = CameraLayers[i].LayerBottom;
                        LayerTop = CameraLayers[i+1].LayerBottom;
                        FixedAreaTop = CameraLayers[i].FixedAreaTop;
                        FixedY = CameraLayers[i].FixedY;
                        NextFixedY = CameraLayers[i + 1].FixedY;
                        break;
                    }
                }
            }

            Vector2 cameraPos = new Vector2(playerPosition.x, FixedY);

            if (playerPosition.y <= FixedAreaTop) { // 0
                cameraPos = new Vector2(playerPosition.x, FixedY); // -1f
            } else {
                if (NextFixedY == -1) {
                    if (LayerTop == -1) {
                        cameraPos = new Vector2(playerPosition.x, FixedY + (playerPosition.y - LayerBottom));
                    } else { 
                        cameraPos = new Vector2(playerPosition.x, FixedY);
                    }
                } else {
                    cameraPos = new Vector2(playerPosition.x, FixedY + (NextFixedY - FixedY) * ((playerPosition.y - FixedAreaTop) / (LayerTop - FixedAreaTop)));
                }
                
            }
            
            this.mainCamera.transform.position = new Vector3(cameraPos.x + offset.x, cameraPos.y + offset.y, -10);
        }

        public void SetCameraSize(float Size) { 
            mainCamera.orthographicSize = Size;
        }


        public void Update() {
            Vector2 cameraPos = LockedCameraPos;
            if (PlayerSpriteRenderer != null) {
                cameraPos = PlayerSpriteRenderer.position;
            }
            SetCameraPosition(cameraPos);
        }

        public void Shake(Vector2 dir, float duration) {
            StartCoroutine(DoShake(dir, duration));
        }

        public IEnumerator DoShake(Vector2 dir, float duration) {
            float elapsed = 0f;

            while (elapsed < duration) {
                float t = ShakeCurve.Evaluate(elapsed / duration) * ShakeStrength;
                float x = dir.x * t;
                float y = dir.y * t;

                offset = new Vector2(x, y);
                elapsed += Time.deltaTime;
                yield return null;
            }
            offset = Vector2.zero;
        }

        public void SetCameraPosition(Vector2 cameraPosition) {
            if (IsLocked) {
                this.mainCamera.transform.position = new Vector3(LockedCameraPos.x + offset.x, LockedCameraPos.y + offset.y, -10);
            } else {
                SetCameraPositionByPlayerPosition(cameraPosition);
            }
        }
    }
}