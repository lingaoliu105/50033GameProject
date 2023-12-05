using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public enum BodyPartType {
    Body,
    LeftArm,
    RightArm,
    LeftLeg,
    RightLeg,
    Size
}
public class BodyPartController:MonoBehaviour {
    public RobotController robotController;
    public BodyPartType bodyPartType;
    public void Start() {
        robotController = GetComponentInParent<RobotController>();
    }
    public void hit(int Damage) { 
      //Debug.Log($"===hit {this.gameObject}===Damage {Damage}");
      if (robotController != null) {
          robotController.hit(Damage, bodyPartType);
      }
    }
    public void Update() { 
        
    }   
    public void OnCollisionEnter(Collision collision) {
    }
}

