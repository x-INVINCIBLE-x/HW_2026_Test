using Doofus.Data;
using System;
using System.Collections;
using UnityEngine;

namespace Doofus.Gameplay
{
    // Generates and manages a sequence of platforms using a small object pool.
    public class PlatformGenerator : MonoBehaviour
    {
        [Header("Platform")]
        [SerializeField] private Platform pulpitPrefab;
        [SerializeField] private PulpitConfig config;
        [SerializeField] private float platformSize = 9f;

        [Header("Start")]
        [SerializeField] private Transform startPoint;

        private static readonly Vector3[] Directions =
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right
        };

        private Platform[] pool;
        private int currentIndex;
        private Coroutine spawnCoroutine;

        private bool isGenerating;

        public event Action<Platform> PlatformReached;
        public bool IsGenerating => isGenerating;

        /// <summary>
        /// Starts platform generation and returns the starting position.
        /// </summary>
        public Vector3 StartGeneration()
        {
            if (isGenerating)
            {
                Debug.LogWarning("[PlatformGenerator] Generation is already running.");

                return GetStartPosition();
            }

            InitializePool();

            Vector3 startPosition = GetStartPosition();

            currentIndex = 0;

            Activate(pool[currentIndex], startPosition);

            isGenerating = true;
            spawnCoroutine = StartCoroutine(SpawnLoop());

            return startPosition;
        }

        /// <summary>
        /// Stops the platform generation loop.
        /// </summary>
        public void StopGeneration()
        {
            if (!isGenerating)
                return;

            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }

            isGenerating = false;
        }

        // Creates the platform pool used during generation.
        private void InitializePool()
        {
            if (pool != null)
                return;

            pool = new Platform[2];

            for (int i = 0; i < pool.Length; i++)
            {
                pool[i] = Instantiate(pulpitPrefab, transform);

                pool[i].gameObject.SetActive(false);
            }
        }

        // Continuously spawns the next platform after the configured delay.
        private IEnumerator SpawnLoop()
        {
            while (isGenerating)
            {
                Platform currentPulpit = pool[currentIndex];

                float lifetime = currentPulpit.Lifetime;

                float spawnDelay = Mathf.Min(config.pulpitSpawnTime, lifetime);

                yield return new WaitForSeconds(spawnDelay);

                if (!isGenerating)
                    yield break;

                int nextIndex = 1 - currentIndex;

                Vector3 nextPosition = GetNextPosition(currentPulpit.transform.position);

                Activate(pool[nextIndex], nextPosition);

                currentIndex = nextIndex;
            }
        }

        // Activates and initializes a platform at the specified position.
        private void Activate(Platform platform, Vector3 position)
        {
            platform.transform.position = position;

            float lifetime = UnityEngine.Random.Range(
                config.minPulpitLifetime,
                config.maxPulpitLifetime
            );

            platform.Initialize(lifetime);
            platform.gameObject.SetActive(true);

            platform.PlayerEntered -= OnPlatformPlayerEntered;
            platform.PlayerEntered += OnPlatformPlayerEntered;
        }

        private void OnPlatformPlayerEntered(Platform platform)
        {
            PlatformReached?.Invoke(platform);
        }

        private Vector3 GetNextPosition(Vector3 previousPosition)
        {
            Vector3 direction = Directions[UnityEngine.Random.Range(0, Directions.Length)];

            return previousPosition + direction * platformSize;
        }

        private Vector3 GetStartPosition()
        {
            return startPoint != null ? startPoint.position : transform.position;
        }
    }
}