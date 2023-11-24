using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Enemy {
    public class GravityProjectile: MonoBehaviour {
        public GameObject ExplosionPrefab;
        public bool isFading = false;

        public void Update() {
            if (isFading) {
                return;
            }
            if (transform.position.y <= B1Constants.GroundY) {
                Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
                StartCoroutine(WaitAndDestroy(1f)); 
            }

        }

        public IEnumerator WaitAndDestroy(float time) {
            isFading = true;
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }

    }
}
