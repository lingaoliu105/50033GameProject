using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Map {
    public class MapObjectTrigger: MonoBehaviour {
        public MapObjectController mapObject;
        public GameObject tipButton;
        public string tipText;
        public bool playerInTrigger = false;
        public int triggerId;

        public void Start() {
            tipButton.SetActive(false);
        }

        public void Update() {
            if (playerInTrigger) {
                tipButton.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E)) {
                    Debug.Log("Player pressed E");
                    mapObject.Interact(triggerId);
                }
            } else {
                tipButton.SetActive(false);
            }
        }
        public void OnTriggerEnter2D(Collider2D collision) {
            if (collision.gameObject.tag == "Player") {
                Debug.Log("Player entered trigger");
                playerInTrigger = true;
            }
        }
        public void OnTriggerExit2D(Collider2D collision) {
            if (collision.gameObject.tag == "Player") {
                Debug.Log("Player exited trigger");
                playerInTrigger = false;
            }
        }

    }
}
