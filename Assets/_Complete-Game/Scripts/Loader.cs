using UnityEngine;
using System.Collections;

namespace Completed
{
    public class Loader : MonoBehaviour
    {
        public GameObject gameManager; //GameManager prefab to instantiate.
        public GameObject soundManager; //SoundManager prefab to instantiate.
        [SerializeField] private Player _player;


        private void Awake()
        {
            if (GameManager.Instance == null)
                Instantiate(gameManager);

            if (SoundManager.Instance == null)
                Instantiate(soundManager);

#if UNITY_STANDALONE || UNITY_WEBPLAYER
            _player.SetInput(new KeyboardInput());
#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
            _player.SetInput(new TouchInput());
#endif
        }
    }
}