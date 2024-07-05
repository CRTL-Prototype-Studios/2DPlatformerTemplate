using System;
using UnityEngine;

namespace CiGAGJ2024.Character
{
    [RequireComponent(typeof(CharacterControllerInputBehaviour))]
    public class CharacterControllerBehaviour : MonoBehaviour
    {
        [SerializeField] protected CharacterControllerInputBehaviour InputBehaviour;

        protected void Awake()
        {
            InputBehaviour = GetComponent<CharacterControllerInputBehaviour>();
        }
    }
}