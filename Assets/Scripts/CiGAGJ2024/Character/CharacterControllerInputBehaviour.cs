using System;
using UnityEngine;

namespace CiGAGJ2024.Character
{
    public class CharacterControllerInputBehaviour : MonoBehaviour
    {
        [SerializeField] protected CharacterControllerBehaviour Controller;
        protected CharacterControls InputMap;

        protected void Awake()
        {
            if(!Controller) Controller = GetComponent<CharacterControllerBehaviour>();
            InputMap = new CharacterControls();
        }

        protected void OnEnable()
        {
            if (InputMap is null) InputMap = new CharacterControls();
            
            InputMap.Enable();
            InputMap.Primary.Movement.performed += Controller.ListenMovementPerformed;
            InputMap.Primary.Movement.canceled += Controller.ListenMovementCancelled;
            InputMap.Primary.Jump.performed += Controller.ListenJump;
        }

        protected void OnDisable()
        {
            InputMap.Disable();
            InputMap.Primary.Movement.performed -= Controller.ListenMovementPerformed;
            InputMap.Primary.Movement.canceled -= Controller.ListenMovementCancelled;
            InputMap.Primary.Jump.performed -= Controller.ListenJump;
        }
    }
}