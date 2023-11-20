using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.UI {
    public class BuffPanel : MonoBehaviour {
        public bool display;
        public Image buffIconDisplay;
        public Sprite buffIcon;
        public TextMeshProUGUI buffTimeDisplay;
        public float buffTime;

        void Update() {
            if (display) {
                this.gameObject.SetActive(true);
                buffTimeDisplay.text = buffTime.ToString("f1");
                buffIconDisplay.sprite = buffIcon;
            }
            
            if (!display) {
                this.gameObject.SetActive(false);
            } 
        }
    }
}

