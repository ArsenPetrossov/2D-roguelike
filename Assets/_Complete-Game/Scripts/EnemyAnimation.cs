using UnityEngine;

namespace Completed
{
    public class EnemyAnimation : ICharacterAttackAnimations
    {
        private Animator _animator;

        public EnemyAnimation(Animator animator)
        {
            _animator = animator;
        }

        public void SetAttack()
        {
            _animator.SetTrigger("enemyAttack");
        }

    }
}