using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum Facings
{
    Right = 1,
    Left = -1
}
namespace Game {
    public partial class EnemyController {
        public Facings facing = Facings.Right;
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
    }
}
