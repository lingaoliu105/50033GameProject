using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BodyPartController:MonoBehaviour {
    public void hit(int Damage) { 
      Debug.Log($"===hit {this.gameObject}===Damage {Damage}");
    }
    public void Update() { 
        
    }   
    public void OnCollisionEnter(Collision collision) {
    }
}

