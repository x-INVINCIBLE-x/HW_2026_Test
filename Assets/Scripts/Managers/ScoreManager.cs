using Doofus.Gameplay;
using System;
using UnityEngine;

namespace Doofus.Manager

{
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

        private void OnPlatformReached(Platform platform)
        {
            Score++;
            ScoreChanged?.Invoke(Score);
        }

        public void ResetScore()
        {
            Score = 0;
            ScoreChanged?.Invoke(Score);
        }
    }
}