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
            animator = GetComponent<Animator>();
            animator.SetTrigger("die");
        }

        public void PlayAttackAnimation() {
            animator = GetComponent<Animator>();
            animator.SetTrigger("attack");
        }

        public IEnumerator FlashRedOnce() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            for (int i = 0; i < 7; i++) {
                yield return null;
            }
            spriteRenderer.color = originalColor;
        }

        public void UpdateFacing() {
                        
        }
    }
}
