using UnityEngine;

namespace States
{
    public class BlockState : State
    {
        public BlockState(GameObject host, float duration = 0.3f) : base(host, duration) { }

        public override void OnEnter()
        {
            Animator.SetBool("IsBlocking", true);
            PlayerController.PlayerHealthController.IsBlocking = true;
            base.OnEnter();
        }

        public override void OnExit()
        {
            Animator.SetBool("IsBlocking", false);
            PlayerController.PlayerHealthController.IsBlocking = false;

            base.OnExit();
        }
    }
}