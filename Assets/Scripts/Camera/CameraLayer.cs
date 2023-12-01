using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Camera {
    [Serializable]
    public struct CameraLayer {
        // Next Layer Bottom/Layer Top
        public bool hasTop;
        public float LayerTop;
        public float LayerBottom;
        public float FixedAreaTop;

        public float FixedY;
        // Transiction Area
        // Fixed Area Top
        // Fixed Area
        // Layer Bottom 

    }

}
