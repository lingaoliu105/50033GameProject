using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableSM/Decisions/Random")]
public class RandomDecision : Decision {
    public float chance;
    public override bool Decide(StateController controller) {
        return UnityEngine.Random.Range(0f, 1f) < chance;
    }
}