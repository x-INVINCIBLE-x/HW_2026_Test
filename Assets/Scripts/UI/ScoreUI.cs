using Doofus.Gameplay;
using Doofus.Manager;
using TMPro;
using UnityEngine;

namespace Doofus.UI
{
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

        private void OnGameStarted()
        {
            scorePanel.SetActive(true);
            UpdateScore(scoreManager.Score);
        }

        private void OnScoreChanged(int score)
        {
            UpdateScore(score);
        }

        private void UpdateScore(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}