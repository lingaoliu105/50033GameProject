using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game {
    public class ClimbState : BaseActionState {
        public ClimbState(PlayerController controller) : base(EActionState.Climb, controller) {
        }

        public override IEnumerator Coroutine() {
            throw new NotImplementedException();
        }

        public override bool IsCoroutine() {
            return false;
        }

        public override void OnBegin() {
            
        }

        public override void OnEnd() {
        }

        public override EActionState Update(float deltaTime) {
            #region 尾迹
            #endregion
            #region super
            #endregion

            return EActionState.Normal;
        }
    }
}