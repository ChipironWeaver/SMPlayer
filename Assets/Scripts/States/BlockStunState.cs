using UnityEngine;

namespace States
{
    public class BlockStunState : State
    {
        public BlockStunState(GameObject host, float duration = -1) : base(host, duration) { }

        public override void OnEnter()
        {
            base.OnEnter();
            Animator.SetBool("IsBlockStun", true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool("IsBlockStun", false);
        }
    }
}