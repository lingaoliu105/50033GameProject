using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy {
    public class WarningAreaFlashing: MonoBehaviour {
        private SpriteRenderer spriteRenderer;
        public void Start() {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void Show() { 
            StartCoroutine(Flash());
        }

        public IEnumerator Flash() {
            for (int i = 0; i < 5; i++) {
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
