using System.Collections;
using UnityEngine;
using Doofus.Data;
using Doofus.Input;

namespace Doofus.Player
{
    // Handles grid-based player movement using input and player configuration.
    [RequireComponent(typeof(Transform))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private PlayerConfig config;

        [Header("Grid Settings")]
        [SerializeField] private float cellSize = 1f;

        public bool IsMoving { get; private set; }

        private Vector3 targetPosition;

        private void Start()
        {
            targetPosition = transform.position;
        }

        private void Update()
        {
            if (IsMoving) return;
            if (InputManager.Instance == null) return;

            Vector2Int direction = GetDirection(InputManager.Instance.MoveInput);

            if (direction == Vector2Int.zero) return;

            targetPosition += new Vector3(direction.x * cellSize, 0f, direction.y * cellSize);

            Vector3 destination = new(targetPosition.x, transform.position.y, targetPosition.z);

            StartCoroutine(Move(destination));
        }

        // Converts analog input into a single grid movement direction.
        private Vector2Int GetDirection(Vector2 input)
        {
            if (input == Vector2.zero) return Vector2Int.zero;

            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                return new Vector2Int((int)Mathf.Sign(input.x), 0);

            return new Vector2Int(0, (int)Mathf.Sign(input.y));
        }

        /// <summary>
        /// Moves the player smoothly from its current position to the destination.
        /// </summary>
        private IEnumerator Move(Vector3 destination)
        {
            IsMoving = true;

            Vector3 start = transform.position;

            float moveSpeed = config != null ? config.moveSpeed : 1f;
            float duration = cellSize / moveSpeed;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                Vector3 position = Vector3.Lerp(start, destination, t);
                position.y = transform.position.y;
                transform.position = position;

                yield return null;
            }

            transform.position = new Vector3(destination.x, transform.position.y, destination.z);

            IsMoving = false;
        }
    }
}