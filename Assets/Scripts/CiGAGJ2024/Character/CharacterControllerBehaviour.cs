using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CiGAGJ2024.Character
{
    [
        RequireComponent(typeof(CharacterControllerInputBehaviour)),
        RequireComponent(typeof(Rigidbody2D))
    ]
    public class CharacterControllerBehaviour : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected CharacterControllerInputBehaviour InputBehaviour;
        [SerializeField] protected Rigidbody2D Rigidbody;
        [SerializeField] protected Collider2D Collider;
        [SerializeField] protected LayerMask GroundLayer;
        
        [Header("Movement")]
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float runAccelAmount = 75f;
        [SerializeField] private float runDeccelAmount = 50f;
        [SerializeField] private float accelInAir = 0.65f;
        [SerializeField] private float deccelInAir = 0.65f;
        
        [Header("Jump")]
        [SerializeField] private float jumpForce = 11f;
        [SerializeField] private float coyoteTime = 0.1f;
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
        private float _lastGroundedTime = 0f;
        private float _lastJumpPressedTime = 0f;
        private bool _isJumping = false;
        private float _moveInput;

        protected void Awake()
        {
            if (!InputBehaviour) InputBehaviour = GetComponent<CharacterControllerInputBehaviour>();
            if (!Rigidbody) Rigidbody = GetComponent<Rigidbody2D>();
            if (!Collider) Collider = GetComponent<Collider2D>();
        }

        protected void Update()
        {
            JumpCheckCore();
        }

        protected void FixedUpdate()
        {
            MovementCore();
        }

        #region Core Functions
        protected virtual void MovementCore()
        {
            float targetSpeed = _moveInput * maxSpeed;
            float speedDiff = targetSpeed - Rigidbody.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;

            // Apply air acceleration factor
            if (!IsGrounded)
            {
                accelRate *= (Mathf.Abs(targetSpeed) > 0.01f) ? accelInAir : deccelInAir;
            }

            // Calculate force
            float movement = Mathf.Abs(speedDiff) * accelRate * Mathf.Sign(speedDiff);
            
            // Apply force to Rigidbody
            Rigidbody.AddForce(movement * Vector2.right);

            // Clamp velocity
            if (Mathf.Abs(Rigidbody.velocity.x) > maxSpeed)
            {
                Rigidbody.velocity = new Vector2(Mathf.Sign(Rigidbody.velocity.x) * maxSpeed, Rigidbody.velocity.y);
            }
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
            _moveInput = context.ReadValue<float>();
        }

        /// <summary>
        /// Used to listen for cancelling the horizontal axis input
        /// </summary>
        public void ListenMovementCancelled(InputAction.CallbackContext context)
        {
            _moveInput = 0f;
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