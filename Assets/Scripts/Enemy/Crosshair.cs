using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy {
    public class Crosshair: MonoBehaviour {
        private SpriteRenderer spriteRenderer;
        public void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1, 1, 1, 0);
            Show();
        }

        public void Show() { 
            StartCoroutine(Appear(0.5f));
        }

        public IEnumerator Appear(float appearTime) {
            yield return null;
            // Size from 3 to 1 and alpha from 0 to 255
            float time = 0;
            while (time < appearTime) {
                time += Time.deltaTime;
                float ratio = time / appearTime;
                transform.localScale = new Vector3(3 - 2 * ratio, 3 - 2 * ratio, 1);
                spriteRenderer.color = new Color(1, 1, 1, ratio);
                transform.Rotate(new Vector3(0, 0, 1), 360 * Time.deltaTime);
                yield return null;
            }
            for (int i = 0; i < 3; i++) {
                spriteRenderer.enabled = true;
                yield return null;
                yield return null;
                yield return null;
                yield return null;
                spriteRenderer.enabled = false;
                yield return null;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
