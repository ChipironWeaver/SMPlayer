using UnityEngine;

namespace States
{
    public class MoveState : State {
        public MoveState(GameObject host, float duration = -1) : base(host, duration) { }

        public override void OnUpdate() 
        {
            base.OnUpdate();
        
            PlayerController.transform.Translate(InputController.Move * 
                                                 (Time.deltaTime * PlayerController.Speed));
            if (InputController.Move.x != 0)
            {
                PlayerController.SpriteRenderer.flipX = InputController.Move.x < 0 ;
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Animator.SetBool("IsWalking", true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool("IsWalking", false);
        }
    }
}