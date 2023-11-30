using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.UI {
    public class Inventory:MonoBehaviour {
        public PlayerController player;
        void Start() {
            player = GetComponentInParent<PlayerController>();
        }

        private void Update() {
            
        }

    }
}
