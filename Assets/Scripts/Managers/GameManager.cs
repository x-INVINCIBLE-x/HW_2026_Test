using System;
using UnityEngine;

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

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            if (IsGameRunning)
                return;

            IsGameRunning = true;
            Debug.Log("Generta");
            GameStarted?.Invoke();
        }

        public void EndGame()
        {
            if (!IsGameRunning)
                return;

            IsGameRunning = false;
            GameOver?.Invoke();
        }
    }
}