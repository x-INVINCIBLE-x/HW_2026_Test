using System.Collections;
using UnityEngine;
using Doofus.Data;

namespace Doofus.Gameplay
{
    public class PlatformGenerator : MonoBehaviour
    {
        [Header("Platform")]
        [SerializeField] private Pulpit pulpitPrefab;
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

        private Pulpit[] pool;
        private int currentIndex;
        private Coroutine spawnCoroutine;

        private bool isGenerating;

        public bool IsGenerating => isGenerating;

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

        private void InitializePool()
        {
            if (pool != null)
                return;

            pool = new Pulpit[2];

            for (int i = 0; i < pool.Length; i++)
            {
                pool[i] = Instantiate(pulpitPrefab, transform);

                pool[i].gameObject.SetActive(false);
            }
        }

        private IEnumerator SpawnLoop()
        {
            while (isGenerating)
            {
                Pulpit currentPulpit = pool[currentIndex];

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

        private void Activate(Pulpit pulpit, Vector3 position)
        {
            pulpit.transform.position = position;
            pulpit.gameObject.SetActive(true);

            float lifetime = Random.Range(config.minPulpitLifetime, config.maxPulpitLifetime);

            pulpit.Initialize(lifetime);
        }

        private Vector3 GetNextPosition(Vector3 previousPosition)
        {
            Vector3 direction = Directions[Random.Range(0, Directions.Length)];

            return previousPosition + direction * platformSize;
        }

        private Vector3 GetStartPosition()
        {
            return startPoint != null ? startPoint.position : transform.position;
        }
    }
}