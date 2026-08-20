using Doofus.Gameplay;
using UnityEngine;

namespace Doofus.Manager
{
    // Manages level setup and platform generation during the game lifecycle.
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

        // Starts platform generation and spawns the player when the game starts.
        private void OnGameStarted()
        {
            Vector3 spawnPosition = platformGenerator.StartGeneration();

            Instantiate(playerPrefab, spawnPosition + Vector3.up, Quaternion.identity);
        }

        // Stops platform generation when the game ends.
        private void OnGameOver()
        {
            platformGenerator.StopGeneration();
        }
    }
}