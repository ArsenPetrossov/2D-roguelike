using UnityEngine;

namespace Completed
{
    public class PlayerAnimation
    {
        private Animator _animator;

        public PlayerAnimation(Animator animator)
        {
            _animator = animator;
        }

        public void SetPlayerChop()
        {
            _animator.SetTrigger("playerChop");
        }

        public void SetPlayerHit()
        {
            _animator.SetTrigger("playerHit");
        }
    }
}