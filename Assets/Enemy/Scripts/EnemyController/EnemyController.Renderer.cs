using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Enemy {
    public enum Facings {
        Left = -1,
        Right = 1,
    }
    public partial class EnemyController {
        protected Facings facing = Facings.Right;
        public void PlayDeadAnimation() {
            animator.SetTrigger("die");
        }

        public void PlayAttackAnimation() {
            animator.SetTrigger("attack");
        }
        
        private void HitFlash()
        {
            animator.SetTrigger("hit");
        }

        public void PassAway()
        {
            Destroy(gameObject);
        }
    }
}
