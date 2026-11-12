using UnityEngine;

namespace States
{
    public class RecoveryState : State
    {
        public RecoveryState(GameObject host, float duration = 0.1f) : base(host, duration) { }

        public override void OnEnter()
        {
            Animator.SetBool("IsRecovery", true);
            base.OnEnter();
        }

        public override void OnExit()
        {
            Animator.SetBool("IsRecovery", false);
            base.OnExit();
        }
    }
}