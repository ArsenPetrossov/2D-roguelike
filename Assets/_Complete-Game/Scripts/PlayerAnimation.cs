using UnityEngine;

namespace Completed
{
    public class PlayerAnimation : ICharacterAnimations
    {
        private Animator _animator;

        public PlayerAnimation(Animator animator)
        {
            _animator = animator;
        }
        

        public void SetPlayerHit()
        {
            _animator.SetTrigger("playerHit");
        }


        public void SetAttack()
        {
            _animator.SetTrigger("playerChop");
        }
    }
}