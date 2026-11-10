using UnityEngine;

namespace States
{
    public class IdleState : State
    {
        public IdleState(GameObject host, float duration = -1) : base(host, duration) { }
        public override void OnEnter() 
        {
            base.OnEnter();
            Animator.SetBool("IsIdle", true);
        }

        public override void OnExit() 
        {
            base.OnExit();
            Animator.SetBool("IsIdle", false);
        }
    }
}