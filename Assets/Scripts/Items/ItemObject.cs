using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items {
    public class ItemObject: MonoBehaviour {
        public ItemBase Item;
        public PlayerController player;
        public void Start() {
            
        }
        public virtual void PickedUP(PlayerController player) { 
            GetComponent<Collider2D>().enabled = false;
            this.player = player;
            StartCoroutine(Small(0.5f));
        }
        public IEnumerator Small(float time) { 
            //x scale from 1 to 0 in time seconds
            float elapsedTime = 0;
            player.PlayAbsorbEffect();
            Vector3 originalScale = transform.localScale;
            while (elapsedTime < time) {
                transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, (elapsedTime / time));
                elapsedTime += Time.deltaTime;
                //transform.position -= (transform.position - player.transform.position)/ (time - elapsedTime)*Time.deltaTime;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}
