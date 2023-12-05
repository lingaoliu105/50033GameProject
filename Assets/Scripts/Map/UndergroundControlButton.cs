using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Map {
    public class UnderGroundControlButton:MapObjectController {
        public GameObject poision;
        public GameObject blocker;
        private bool isBlocked;

        public void Start() {
            isBlocked = false;
        }
        public void Update() {
        }

        public override void Interact(int triggerId) {
            if (triggerId == 0 && !isBlocked) { 
                isBlocked = true;
                poision.SetActive(false);
                blocker.SetActive(true);
                //play sound effect
                this.transform.position -= Vector3.up*0.5f;
            } 
            else if (triggerId == 1) {
                return;
            }
        }
    }
}

