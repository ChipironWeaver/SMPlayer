using UnityEngine;

namespace States
{
    public class AttackState : State 
    {
        public AttackState(GameObject host, ComboAction comboAction) : base(host, comboAction.AttackTime)
        {
            _comboAction = comboAction;
        }

        private ComboAction _comboAction;
        
        public override void OnEnter() 
        {
            
            PlayerController.PlayerDamageSource.CurrentDamageInfo = _comboAction.DamageInfo;
            base.OnEnter();
            InputController.IsAttacking.Stop();
            Animator.SetBool("IsAttacking", true);
            
            if(_comboAction.AnimatorOverrideController)
            {
                Animator.runtimeAnimatorController = _comboAction.AnimatorOverrideController;
            }
        }

        public override void OnExit() 
        {
            base.OnExit();
            Animator.SetBool("IsAttacking", false);
            
            PlayerController.OnAttackFinished();
            Animator.runtimeAnimatorController = PlayerController.BaseAnimatorController;
        }
    
    }
}