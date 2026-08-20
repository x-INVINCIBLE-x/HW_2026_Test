using Doofus.Gameplay;
using Doofus.Manager;
using UnityEngine;

namespace Doofus.UI
{
    // Displays the game over panel when the game ends.
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;

        private void Start()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.GameOver += Show;
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.GameOver -= Show;
        }

        private void Awake()
        {
            gameOverPanel.SetActive(false);
        }

        // Shows the game over panel.
        private void Show()
        {
            gameOverPanel.SetActive(true);
        }
    }
}