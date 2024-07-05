using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace CiGAGJ2024.Character
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] protected ParallaxCamera ParallaxCamera;
        List<ParallaxLayer> _parallaxLayers = new List<ParallaxLayer>();
 
        void Start()
        {
            if (!ParallaxCamera)
                ParallaxCamera = Camera.main?.GetComponent<ParallaxCamera>();
 
            if (ParallaxCamera)
                ParallaxCamera.OnCameraTranslate += Move;
 
            SetLayers();
        }
 
        [Button]
        public void SetLayers()
        {
            _parallaxLayers.Clear();
            foreach(ParallaxLayer p in transform.GetComponentsInChildren<ParallaxLayer>())
            {
                _parallaxLayers.Add(p);
            }
        }
 
        void Move(float deltaX, float deltaY)
        {
            foreach (ParallaxLayer layer in _parallaxLayers)
            {
                layer.Move(deltaX, deltaY);
            }
        }
    }
}