using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Buff {
    [Serializable]
    public class BuffIcon {
        public string name;
        public Sprite icon;
    }
    [CreateAssetMenu(fileName = "BuffIconList", menuName = "Buff/BuffIconList", order = 1)]
    public class BuffIconList: ScriptableObject {
        public List<BuffIcon> buffIcons;
    }
}
