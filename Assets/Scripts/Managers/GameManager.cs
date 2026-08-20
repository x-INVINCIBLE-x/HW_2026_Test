using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Doofus.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action GameStarted;
        public event Action GameOver;

        public bool IsGameRunning { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void StartGame()
        {
            if (IsGameRunning)
                return;

            IsGameRunning = true;
            GameStarted?.Invoke();
        }

        public void EndGame()
        {
            if (!IsGameRunning)
                return;

            IsGameRunning = false;
            GameOver?.Invoke();
        }

        public void Restart()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}