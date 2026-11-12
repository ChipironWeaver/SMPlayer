using UnityEngine;

namespace States
{
    public class StunState : State
    {
        public StunState(GameObject host, float duration = 0.2f) : base(host, duration) { }

        public override void OnEnter()
        {
            base.OnEnter();
            Animator.SetBool("IsHitStun", true);
            PlayerController.SpriteRenderer.color = Color.red;
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool("IsHitStun", false);
            PlayerController.SpriteRenderer.color = Color.white;
        }
    }
}