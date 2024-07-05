using System;
using NaughtyAttributes;
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
        [SerializeField] protected LayerMask GroundLayer;
        
        [Header("Attributes")]
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float acceleration = 30f;
        [SerializeField] private float deceleration = 60f;
        [SerializeField] private float airControlFactor = 0.5f;
        [SerializeField] private float jumpForce = 11f;
        [SerializeField] private float coyoteTime = 0.2f;
        [SerializeField] private float jumpBufferTime = 0.1f;

        public bool IsGrounded
        {
            get
            {
                bool grounded = Collider.IsTouchingLayers(GroundLayer);
                if (grounded)
                {
                    _lastGroundedTime = Time.time;
                    _isJumping = false;
                }
                return grounded;

            }
        }

        private Vector2 _velocity;
        private float _lastGroundedTime;
        private float _lastJumpPressedTime;
        private bool _isJumping;
        private float _movementValue = 0f, _movementTargetValue = 0f;

        protected void Awake()
        {
            if (!InputBehaviour) InputBehaviour = GetComponent<CharacterControllerInputBehaviour>();
            if (!Rigidbody) Rigidbody = GetComponent<Rigidbody2D>();
            if (!Collider) Collider = GetComponent<Collider2D>();
        }

        protected void Update()
        {
            MovementCore();
            JumpCheckCore();
        }

        protected void FixedUpdate()
        {
            MovementTickCore();
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
        
        protected void JumpCheckCore()
        {
            if (Time.time - _lastGroundedTime <= coyoteTime && !_isJumping && Time.time - _lastJumpPressedTime <= jumpBufferTime)
            {
                Jump();
            }
        }
        #endregion

        #region Player Actions

        /// <summary>
        /// Performs the actual jump results.
        /// </summary>
        /// <remarks>
        /// Please do not call this function anywhere else other than <see cref="JumpCheckCore"/>, otherwise it would work improperly
        /// </remarks>
        /// <seealso cref="ListenJump"/>
        private void Jump()
        {
            _isJumping = true;
            _lastJumpPressedTime = 0; // Reset jump buffer
            Rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        #endregion
        
        #region Input Listeners
        /// <summary>
        /// Used to listen for performing the horizontal axis input
        /// </summary>
        public void ListenMovementPerformed(InputAction.CallbackContext context)
        {
            _movementTargetValue = context.ReadValue<float>();
        }

        /// <summary>
        /// Used to listen for cancelling the horizontal axis input
        /// </summary>
        public void ListenMovementCancelled(InputAction.CallbackContext context)
        {
            _movementTargetValue = 0f;
        }

        /// <summary>
        /// Used to listen for performing the jump input
        /// </summary>
        public void ListenJump(InputAction.CallbackContext context)
        {
            _lastJumpPressedTime = Time.time; // This is how to call the jump method properly without ignoring the coyote time
        }
        #endregion
    }
}