using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Map {
    public abstract class MapObjectController:MonoBehaviour {
        public abstract void Interact(int triggerId);
    }
}
