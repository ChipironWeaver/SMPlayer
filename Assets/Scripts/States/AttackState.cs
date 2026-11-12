using UnityEngine;

namespace States
{
    public class AttackState : State 
    {
        public AttackState(GameObject host, float duration = 0.5f) : base(host, duration) { }

        public override void OnEnter() 
        {
            base.OnEnter();
        
            InputController.IsAttacking.Stop();
            Animator.SetBool("IsAttacking", true);
        }

        public override void OnExit() 
        {
            base.OnExit();

            Animator.SetBool("IsAttacking", false);
            
            PlayerController.OnAttackFinished();
        }
    
    }
}