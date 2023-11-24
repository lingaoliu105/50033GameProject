using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

namespace Assets.Scripts.Enemy {
    public class SplitingProjectile: MonoBehaviour {
        public float initialVelocity;
        public int[] splitVelocities;
        public float splitTime;
        public GameObject splitPrefab;
        public bool isFading = false;
        public void Split() {

            float velocityX = GetComponent<Rigidbody2D>().velocity.x;
            float velocityY = GetComponent<Rigidbody2D>().velocity.y;
            for (int i = 0; i < splitVelocities.Length; i++) {
                GameObject split = Instantiate(splitPrefab, transform.position, Quaternion.identity);
                split.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityX*(splitVelocities[i]/100f), velocityY);
                //Debug.Log(split.GetComponent<Rigidbody2D>().velocity);
            }
            StartCoroutine(WaitAndDestroy(1f));
        }
        public void Start() {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            StartCoroutine(WaitAndSplit(splitTime));
        }
        public IEnumerator WaitAndSplit(float time) {
            yield return null;
            GetComponent<Rigidbody2D>().velocity = new Vector2(initialVelocity, 0);
            yield return new WaitForSeconds(time);
            Split();
        }
        public void Update() {
            if (isFading) { 
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                GetComponent<Rigidbody2D>().Sleep();
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0);
            }
        }
        public IEnumerator WaitAndDestroy(float time) {
            isFading = true;
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}
