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
        public PlayerSpriteRenderer SpriteRenderer;
        public EffectManager EffectManager;
        public AudioSource playerAudioSource;
        public AudioClip[] playerAudioClips;

        public void Flash() {
            SpriteRenderer.Flash();
        }

        public void PlayDashEffect(Vector3 position, Vector2 dir) {
            EffectManager.CameraShake(dir);
        }

        public void PlayAbsorbEffect() {
            EffectManager.PlayAbsorbEffect(this);
        }

        public void PlayAnimation(String trigger) {
            SpriteRenderer.SetTrigger(trigger);
            if (trigger=="Jump"&&this.onGround==true){
                playerAudioSource.PlayOneShot(playerAudioClips[0]);
            }else if(trigger=="Attack"){
                playerAudioSource.PlayOneShot(playerAudioClips[1]);
            }else if(trigger=="Shoot"){
                playerAudioSource.PlayOneShot(playerAudioClips[2]);
            }
        }

        public void SetFloat(String name, float value) {
            SpriteRenderer.SetFloat(name, value);
        }

        public void SetBool(String name, bool value) {
            SpriteRenderer.SetBool(name, value);
        }

        public void SetCameraPos() { 

        }

        public void UpdateRender() {
            if (SpriteRenderer == null) return;
            SpriteRenderer.position = Position + collider.position;
            SpriteRenderer.facing = Facing;
            SetCameraPos();
            SpriteRenderer.SpeedX = Mathf.Abs(Speed.x);
            SpriteRenderer.SpeedY = Speed.y;
            SpriteRenderer.Land = OnGround;
            SpriteRenderer.Ducking = Ducking;
        }
        
    }
}
