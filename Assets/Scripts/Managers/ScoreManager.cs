using Doofus.Gameplay;
using System;
using UnityEngine;

namespace Doofus.Manager
{
    // Tracks the player's score based on the platforms they reach.
    public class ScoreManager : MonoBehaviour
    {
        public event Action<int> ScoreChanged;

        [SerializeField] private PlatformGenerator platformGenerator;

        public int Score { get; private set; }

        private void Start()
        {
            platformGenerator.PlatformReached += OnPlatformReached;
        }

        private void OnDestroy()
        {
            if (platformGenerator != null)
                platformGenerator.PlatformReached -= OnPlatformReached;
        }

        // Increases the score when the player reaches a platform.
        private void OnPlatformReached(Platform platform)
        {
            Score++;
            ScoreChanged?.Invoke(Score);
        }

        // Resets the score to zero and notifies listeners.
        public void ResetScore()
        {
            Score = 0;
            ScoreChanged?.Invoke(Score);
        }
    }
}