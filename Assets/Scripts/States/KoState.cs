using UnityEngine;

namespace States
{
    public class KoState : State
    {
        public KoState(GameObject host, float duration = -1) : base(host, duration) { }
        public override void OnEnter()
        {
            base.OnEnter();
            Animator.SetBool("IsKO", true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool("IsKO", false);
            
        }
    }
}