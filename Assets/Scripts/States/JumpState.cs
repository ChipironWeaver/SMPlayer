using UnityEngine;
using UnityEngine.InputSystem;

namespace States
{
    public class JumpState : State {
        public JumpState(GameObject host, float duration = -1) : base(host, duration) { }
    
        public override void OnEnter() 
        {
            base.OnEnter();
            PlayerController.Rigidbody2D.AddForce(Vector2.up * PlayerController.JumpForce, 
                ForceMode2D.Impulse);

            Animator.SetBool("IsJumping", true);
            
        }

        public override void OnUpdate() 
        {
            base.OnUpdate();
            PlayerController.transform.Translate(new Vector2(InputController.Move.x, 0) 
                                                 * (Time.deltaTime * PlayerController.AirControl));
            if (InputController.Move.x != 0)
            {
                PlayerController.SpriteRenderer.flipX = InputController.Move.x < 0 ;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool("IsJumping", false);
        }
    }
}