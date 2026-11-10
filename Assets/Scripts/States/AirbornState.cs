using UnityEngine;

namespace States
{
    public class AirbornState : State
    {
        public AirbornState(GameObject host, float duration = -1) : base(host, duration) { }
        
        public override void OnUpdate() {
            base.OnUpdate();

            PlayerController.transform.Translate(new Vector2(InputController.Move.x, 0) 
                                                 * (Time.deltaTime * PlayerController.AirControl));
            if (InputController.Move.x != 0)
            {
                PlayerController.SpriteRenderer.flipX = InputController.Move.x < 0 ;
            }
            Animator.SetBool("IsAirBorn", false);
            Animator.SetBool("IsRising", false);
            Animator.SetBool("IsFalling", false);
            
            if(PlayerController.Rigidbody2D.linearVelocityY < -1f)
            {
                Animator.SetBool("IsFalling", true);
                Animator.SetBool("IsAirBorn", false);
                Animator.SetBool("IsRising", false);
            }
            else if (PlayerController.Rigidbody2D.linearVelocityY > 1f)
            {
                Animator.SetBool("IsAirBorn", false);
                Animator.SetBool("IsRising", true);
                Animator.SetBool("IsFalling", false);
            }
            else
            {
                Animator.SetBool("IsAirBorn", true);
                Animator.SetBool("IsRising", false);
                Animator.SetBool("IsFalling", false);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool("IsAirBorn", false);
            Animator.SetBool("IsRising", false);
            Animator.SetBool("IsFalling", false);
        }
    }
}