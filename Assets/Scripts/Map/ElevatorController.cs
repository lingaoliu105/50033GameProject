using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Map {
    public class ElevatorController:MapObjectController {
        public float lowBound;
        public float highBound;
        public float time;

        private float constantMovingTime;
        private float accelerationTime;
        private float maxSpeed;

        public ElevatorPlatform platform;

        public Vector2 Speed;

        public bool isMoving = false;

        public void Launch() {
            isMoving = true;
            StartCoroutine(LaunchCoroutine());
        }



        public void Start() {
            constantMovingTime = time / 2f;
            accelerationTime = time / 4f;
            maxSpeed = (highBound - lowBound) / time;
            Speed = new Vector2(0, 0);
        }

        public IEnumerator LaunchCoroutine() {
            float timePassed = 0;
            float speed = 0;
            while (timePassed < time) {
                if (timePassed < accelerationTime) {
                    speed = maxSpeed * timePassed / accelerationTime;
                } else if (timePassed < accelerationTime + constantMovingTime) {
                    speed = maxSpeed;
                } else if (timePassed < time - accelerationTime) {
                    speed = maxSpeed * (time - timePassed) / accelerationTime;
                } else {
                    speed = 0;
                }
                Speed = new Vector2(0, speed);
                timePassed += Time.deltaTime;
                yield return null;
            }
            Speed = new Vector2(0, 0);
        }
        public void Update() {
            if (transform.position.y > highBound) {
                Speed = new Vector2(0, 0);
                transform.position = new Vector3(transform.position.x, highBound, transform.position.z);
            }
            if (transform.position.y < lowBound) {
                Speed = new Vector2(0, 0);
                transform.position = new Vector3(transform.position.x, lowBound, transform.position.z);
            }
            platform.Speed = Speed;
        }

        public override void Interact(int triggerId) {
            if (isMoving) return;
            if (triggerId == 0) { 
                Launch();
            } 
            else if (triggerId == 1) {
            
            }
        }
    }
}
