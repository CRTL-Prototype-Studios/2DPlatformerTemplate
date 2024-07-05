using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace CiGAGJ2024.SceneControls
{
    public class LevelScene : MonoBehaviour
    {
        [SerializeField, Scene] private string nextScene;
        
        public void NextScene()
        {
            SceneController.Instance.LoadScene(nextScene);
        }
        
        [CanBeNull]
        public static LevelScene GetLevelScene()
        {
            return FindObjectOfType<LevelScene>();
        }
    }
}