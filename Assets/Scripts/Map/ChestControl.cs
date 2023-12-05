using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Map {
    public class ChestControl:MapObjectController {
        public GameObject chest;
        private bool isOpened;
        public Animator chestAnimator;
        public AudioSource chestAudioSource;
        public AudioClip chestOpen;
        public int id;
        public GameObject FirstTimeItem;
        public GameObject RepeatItem;

        public void Start() {
            isOpened = false;
        }
        public void Update() {
        }

        public override void Interact(int triggerId) {
            if (triggerId == 0 && !isOpened) { 
                isOpened = true;
                chestAnimator.SetTrigger("isOpened");
                chestAudioSource.PlayOneShot(chestOpen);
                //add item
                if (GameManager.Instance.SaveData.isChestOpened[id]) {
                    Instantiate(RepeatItem, transform.position, Quaternion.identity);
                } else {
                    Instantiate(FirstTimeItem, transform.position, Quaternion.identity);
                    GameManager.Instance.SaveData.isChestOpened[id] = true;
                }
                StartCoroutine(ExecuteAfterDelay());
            } 
            else if (triggerId == 1) {
                return;
            }
        }
        IEnumerator ExecuteAfterDelay()
    {
        // 等待一秒
        yield return new WaitForSeconds(2f);

        // 在一秒后执行的代码
        Disappear();
    }
        public void Disappear(){
            GameObject currentGameObject = gameObject;
            currentGameObject.SetActive(false);
        }
        
    }
}

