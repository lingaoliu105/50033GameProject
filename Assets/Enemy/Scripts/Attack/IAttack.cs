using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    internal interface IAttack {
        IEnumerator WaitAndDestroy();

        void OnCollisionEnter2D(Collision2D collision);
    }
}
