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

            _player.SetInput(KeyboardInput KeyboardInput);
        }
    }
}