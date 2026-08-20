using Doofus.Player;
using UnityEngine;

namespace Doofus.Gameplay
{
    public class GameEndTrigger : MonoBehaviour
    {
        private PlayerController player;

        private void Start()
        {
            player = FindFirstObjectByType<PlayerController>();
        }

        private void LateUpdate()
        {
            if (player == null)
                return;

            Vector3 playerPosition = player.transform.position;

            transform.position = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<PlayerController>(out _))
                return;

            GameManager.Instance.EndGame();
        }
    }
}