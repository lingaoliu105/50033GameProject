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

        public void UpdateRender() {
            PlayerSpriteRenderer.Instance.position = Position + collider.position;
            PlayerSpriteRenderer.Instance.facing = Facing;
            PlayerSpriteRenderer.Instance.cameraPos = cameraPos;
        }
    }
}
