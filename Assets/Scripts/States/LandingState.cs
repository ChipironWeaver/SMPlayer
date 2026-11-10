using UnityEngine;

namespace States
{
    public class LandingState : State
    {
        public LandingState(GameObject host, float duration = 0.1f) : base(host, duration) { }
        public override void OnUpdate() {
            base.OnUpdate();

            PlayerController.transform.Translate(new Vector2(InputController.Move.x, 0) 
                                                 * (Time.deltaTime * PlayerController.Speed));
            if (InputController.Move.x != 0)
            {
                PlayerController.SpriteRenderer.flipX = InputController.Move.x < 0 ;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Animator.SetBool("IsLanding", true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool("IsLanding", false);
        }
    }
}