using UnityEngine;

namespace CiGAGJ2024.Character
{
    public class ParallaxCamera : MonoBehaviour
    {
        public delegate void ParallaxCameraDelegate(float deltaMovementX, float deltaMovementY);
        public ParallaxCameraDelegate OnCameraTranslate;

        [SerializeField] protected bool XAxisParallax = true;
        [SerializeField] protected bool YAxisParallax = false;
 
        private float _oldXPos, _oldYPos;
 
        void Start()
        {
            _oldXPos = transform.position.x;
            _oldYPos = transform.position.y;
        }
 
        void Update()
        {
            if (!transform.position.x.Equals(_oldXPos))
            {
                if (!(OnCameraTranslate is null)) // Use direct address comparison
                {
                    float deltaX = _oldXPos - transform.position.x;
                    float deltaY = _oldYPos - transform.position.y;
                    OnCameraTranslate(XAxisParallax ? deltaX : 0f, YAxisParallax ? deltaY : 0f);
                }
 
                _oldXPos = transform.position.x;
                _oldYPos = transform.position.y;
            }
        }
    }
}