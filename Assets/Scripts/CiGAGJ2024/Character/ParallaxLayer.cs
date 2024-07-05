using UnityEngine;

namespace CiGAGJ2024.Character
{
    public class ParallaxLayer : MonoBehaviour
    {
        [SerializeField] protected float parallaxFactor;
 
        public void Move(float deltaX, float deltaY)
        {
            Vector3 newPos = transform.localPosition;
            newPos.x -= deltaX * parallaxFactor;
            newPos.y -= deltaY * parallaxFactor;
 
            transform.localPosition = newPos;
        }
    }
}