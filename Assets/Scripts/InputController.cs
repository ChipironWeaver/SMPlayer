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
                Debug.Log(Move);
        }

        public void OnJump(InputValue value) 
        {
                if(value.isPressed) IsJumping.Launch();
                Debug.Log("jump");
        }

        public void OnAttack(InputValue value) 
        {
                if (value.isPressed) IsAttacking.Launch();
        }
}