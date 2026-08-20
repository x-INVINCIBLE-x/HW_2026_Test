using System;
using Doofus.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Doofus.Manager
{
    // Manages overall game lifecycle and states
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action GameStarted;
        public event Action GameOver;

        [Header("Data")]
        [SerializeField] private string resourcePath = "doofus_diary";
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private PulpitConfig pulpitConfig;

        public bool IsGameRunning { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            LoadGameData();
        }

        /// <summary>
        /// Loads game configuration data from the Doofus Diary JSON.
        /// </summary>
        private void LoadGameData()
        {
            bool success = JsonExtractor.TryLoad(
                resourcePath,
                out DoofusDiaryData data
            );

            if (!success)
            {
                Debug.LogError(
                    $"[GameManager] Failed to load game data from '{resourcePath}'."
                );

                return;
            }

            PopulateConfigs(data);

            Debug.Log("[GameManager] Game data loaded successfully.");
        }

        /// <summary>
        /// Populates runtime ScriptableObject configurations from loaded data.
        /// </summary>
        private void PopulateConfigs(DoofusDiaryData data)
        {
            if (data == null)
            {
                Debug.LogError("[GameManager] Loaded game data is null.");
                return;
            }

            if (playerConfig == null)
            {
                Debug.LogError(
                    "[GameManager] PlayerConfig is not assigned."
                );
            }
            else
            {
                playerConfig.PopulateFrom(data.player_data);
            }

            if (pulpitConfig == null)
            {
                Debug.LogError(
                    "[GameManager] PulpitConfig is not assigned."
                );
            }
            else
            {
                pulpitConfig.PopulateFrom(data.pulpit_data);
            }
        }

        public void StartGame()
        {
            if (IsGameRunning)
                return;

            IsGameRunning = true;

            Debug.Log("[GameManager] Game Started");

            GameStarted?.Invoke();
        }

        public void EndGame()
        {
            if (!IsGameRunning)
                return;

            IsGameRunning = false;

            Debug.Log("[GameManager] Game Over");

            GameOver?.Invoke();
        }

        public void Restart()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}