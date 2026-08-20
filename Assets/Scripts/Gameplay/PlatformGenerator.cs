using System.Collections;
using UnityEngine;
using Doofus.Data;

namespace Doofus.Gameplay
{
    public class PlatformGenerator : MonoBehaviour
    {
        [SerializeField] private Pulpit pulpitPrefab;
        [SerializeField] private Transform startPoint;
        [SerializeField] private PulpitConfig config;
        [SerializeField] private float platformSize = 9f;

        private static readonly Vector3[] Directions =
        {
            Vector3.forward, Vector3.back, Vector3.left, Vector3.right
        };

        private Pulpit[] pool;
        private int currentIndex;

        private void Start()
        {
            pool = new Pulpit[2];

            pool[0] = Instantiate(pulpitPrefab);
            pool[1] = Instantiate(pulpitPrefab);

            pool[1].gameObject.SetActive(false);

            currentIndex = 0;

            Vector3 position = startPoint != null ? startPoint.position : transform.position;

            Activate(pool[0], position);

            StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                Pulpit current = pool[currentIndex];

                float lifetime = current.Lifetime;
                float spawnDelay = Mathf.Min(config.pulpitSpawnTime, lifetime);

                yield return new WaitForSeconds(spawnDelay);

                int nextIndex = 1 - currentIndex;

                Activate(pool[nextIndex], GetNextPosition(current.transform.position));

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
    }
}