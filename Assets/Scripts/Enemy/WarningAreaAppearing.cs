using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy {
    public class WarningAreaAppearing : MonoBehaviour {
        private SpriteRenderer spriteRenderer;
        private Vector3 targetSize;
        private float targetAlpha;
        private float timer;
        public Vector3 startScaleRate;
        public float appearTime;

        public void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
            timer = 0;
            targetSize = transform.localScale;
            targetAlpha = spriteRenderer.color.a;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
            transform.localScale = new Vector3(targetSize.x * startScaleRate.x, targetSize.y * startScaleRate.y, targetSize.z * startScaleRate.z);
            Show();
        }

        public void Show() {
            
            StartCoroutine(Appear());
        }

        public IEnumerator Appear() { 
            while (timer < appearTime) {
                timer += Time.deltaTime;
                spriteRenderer.enabled = true;
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, (timer / appearTime) * targetAlpha);
                transform.localScale = new Vector3(targetSize.x * startScaleRate.x + (timer / appearTime) * (targetSize.x - targetSize.x * startScaleRate.x), targetSize.y * startScaleRate.y + (timer / appearTime) * (targetSize.y - targetSize.y * startScaleRate.y), targetSize.z * startScaleRate.z + (timer / appearTime) * (targetSize.z - targetSize.z * startScaleRate.z));
                yield return null;
            }
        }
    }
}
