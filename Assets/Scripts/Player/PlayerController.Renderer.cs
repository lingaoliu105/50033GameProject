using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    public partial class PlayerController {
        public static Vector2 NORMAL_SPRITE_SCALE = Vector2.one;
        public static Vector2 DUCK_SPRITE_SCALE = new Vector2(1F, 0.75f);
        public Vector2 cameraPos;


        public void PlayDashEffect(Vector3 position, Vector2 dir) {
            EffectManager.Instance.CameraShake(dir);
        }

        public void PlayAnimation(String trigger) {
            PlayerSpriteRenderer.Instance.SetTrigger(trigger);
        }

        public void SetFloat(String name, float value) {
            PlayerSpriteRenderer.Instance.SetFloat(name, value);
        }

        public void SetBool(String name, bool value) {
            PlayerSpriteRenderer.Instance.SetBool(name, value);
        }

        public void SetCameraPos() { 
            if (Position.y <= 0f) {
                cameraPos = new Vector2(Position.x, -1f);
            } else if (Position.y >= 5) {
                cameraPos = new Vector2(Position.x, 6f);
            } else { 
                cameraPos = new Vector2(Position.x, -1f + 7f * (Position.y/5f));
            }
        }

        public void UpdateRender() {
            PlayerSpriteRenderer.Instance.position = Position + collider.position;
            PlayerSpriteRenderer.Instance.facing = Facing;
            SetCameraPos();
            PlayerSpriteRenderer.Instance.cameraPos = cameraPos;
            PlayerSpriteRenderer.Instance.SpeedX = Mathf.Abs(Speed.x);
            PlayerSpriteRenderer.Instance.SpeedY = Speed.y;
            PlayerSpriteRenderer.Instance.Land = OnGround;
            PlayerSpriteRenderer.Instance.Ducking = Ducking;
        }
    }
}
