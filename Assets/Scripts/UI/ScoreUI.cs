using Doofus.Gameplay;
using Doofus.Manager;
using TMPro;
using UnityEngine;

namespace Doofus.UI
{
    // Displays the current player score during gameplay.
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private GameObject scorePanel;
        [SerializeField] private TextMeshProUGUI scoreText;

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameStarted += OnGameStarted;
            }

            if (scoreManager != null)
            {
                scoreManager.ScoreChanged += OnScoreChanged;
            }

            scoreText.text = "0";
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameStarted -= OnGameStarted;
            }

            if (scoreManager != null)
            {
                scoreManager.ScoreChanged -= OnScoreChanged;
            }
        }

        // Shows the score panel when the game starts.
        private void OnGameStarted()
        {
            Debug.Log("SA");
            scorePanel.SetActive(true);
            UpdateScore(scoreManager.Score);
        }

        // Updates the displayed score when the score changes.
        private void OnScoreChanged(int score)
        {
            UpdateScore(score);
        }

        // Updates the score text.
        private void UpdateScore(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}