using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour 
{
        [ReadOnly] public Vector2 Move;
        public Trigger IsJumping;
        public Trigger IsAttacking;

        public void OnMove(InputValue value) 
        {
                Move = value.Get<Vector2>();
        }

        public void OnJump(InputValue value) 
        {
                if(value.isPressed) IsJumping.Launch();
        }

        public void OnAttack(InputValue value) 
        {
                if (value.isPressed) IsAttacking.Launch();
        }
}