using Doofus.Gameplay;
using Doofus.Player;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Doofus.Manager
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private PlatformGenerator platformGenerator;
        [SerializeField] private GameObject playerPrefab;

        private void Start()
        {
            GameManager.Instance.GameStarted += OnGameStarted;
            GameManager.Instance.GameOver += OnGameOver;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameStarted -= OnGameStarted;
                GameManager.Instance.GameOver -= OnGameOver;
            }
        }

        private void OnGameStarted()
        {
            Vector3 spawnPosition = platformGenerator.StartGeneration();

            Instantiate(playerPrefab, spawnPosition + Vector3.up, Quaternion.identity);
        }

        private void OnGameOver()
        {
            platformGenerator.StopGeneration();
        }
    }
}