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
            chest.SetActive(false);
        }
        
    }
}

