using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CiGAGJ2024.Character
{
    [
        RequireComponent(typeof(CharacterControllerInputBehaviour)),
        RequireComponent(typeof(Rigidbody2D)),
        RequireComponent(typeof(Collider2D))
    ]
    public class CharacterControllerBehaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected CharacterControllerInputBehaviour InputBehaviour;
        [SerializeField] protected Rigidbody2D Rigidbody;
        [SerializeField] protected Collider2D Collider;
        
        [Header("Attributes")]
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float acceleration = 30f;
        [SerializeField] private float deceleration = 60f;
        [SerializeField] private float airControlFactor = 0.5f;
        
        public bool IsGrounded { get; set; }
        
        private Vector2 _velocity;
        private float _movementValue = 0f, _movementTargetValue = 0f;

        protected void Awake()
        {
            if (!InputBehaviour) InputBehaviour = GetComponent<CharacterControllerInputBehaviour>();
            if (!Rigidbody) Rigidbody = GetComponent<Rigidbody2D>();
            if (!Collider) Collider = GetComponent<Collider2D>();
        }
        
        #region Core Functions
        protected virtual void MovementCore()
        {
            float targetSpeed = _movementTargetValue * maxSpeed;
            float speedDiff = targetSpeed - _velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

            // Apply air control factor when not grounded
            if (!IsGrounded)
            {
                accelRate *= airControlFactor;
            }

            // Calculate movement
            float movement = speedDiff * accelRate * Time.deltaTime;

            // Apply movement
            _velocity.x += movement;

            // Apply gravity
            if (!IsGrounded)
            {
                _velocity.y += Physics2D.gravity.y * Time.deltaTime;
            }

            // Clamp velocity
            _velocity.x = Mathf.Clamp(_velocity.x, -maxSpeed, maxSpeed);

            // Apply velocity to Rigidbody
            Rigidbody.velocity = _velocity;
        }

        protected virtual void MovementTickCore()
        {
            _movementValue = Mathf.Lerp(_movementValue, _movementTargetValue, Time.fixedDeltaTime);
            _velocity = Rigidbody.velocity;
        }
        #endregion
        
        #region Input Listeners
        public void ListenMovementPerformed(InputAction.CallbackContext context)
        {
            _movementTargetValue = context.ReadValue<float>();
        }

        public void ListenMovementCancelled(InputAction.CallbackContext context)
        {
            _movementTargetValue = 0f;
        }

        public void ListenJump(InputAction.CallbackContext context)
        {
            context.ReadValue<bool>();
        }
        #endregion
    }
}