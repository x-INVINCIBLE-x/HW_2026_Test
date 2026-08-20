using Doofus.Gameplay;
using UnityEngine;

namespace Doofus.UI
{
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

        private void Show()
        {
            gameOverPanel.SetActive(true);
        }
    }
}