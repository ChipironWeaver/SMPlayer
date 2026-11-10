using UnityEngine;

namespace States
{
    public class HitStunState : State
    {
        public HitStunState(GameObject host, float duration = -1) : base(host, duration) { }

        public override void OnEnter()
        {
            base.OnEnter();
            Animator.SetBool("IsHitStun", true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool("IsHitStun", false);
        }
    }
}