using System.Collections;
using UnityEngine;
using Doofus.Data;
using Doofus.Input;

namespace Doofus.Player
{
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

            Vector3 destination = targetPosition + new Vector3(direction.x, 0f, direction.y) * cellSize;
            StartCoroutine(Move(destination));
        }

        private Vector2Int GetDirection(Vector2 input)
        {
            if (input == Vector2.zero) return Vector2Int.zero;

            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                return new Vector2Int((int)Mathf.Sign(input.x), 0);

            return new Vector2Int(0, (int)Mathf.Sign(input.y));
        }

        private IEnumerator Move(Vector3 destination)
        {
            IsMoving = true;

            Vector3 start = targetPosition;
            targetPosition = destination;

            float moveSpeed = config != null ? config.moveSpeed : 1f;
            float duration = cellSize / moveSpeed;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                transform.position = Vector3.Lerp(start, destination, t);
                yield return null;
            }

            transform.position = destination;
            IsMoving = false;
        }
    }
}