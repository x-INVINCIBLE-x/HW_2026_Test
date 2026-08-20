using UnityEngine;

namespace Doofus.Data
{
    // Stores platform configuration data used at runtime.
    [CreateAssetMenu(fileName = "PulpitConfig", menuName = "Doofus/Platform Config")]
    public class PulpitConfig : ScriptableObject
    {
        public float minPulpitLifetime;
        public float maxPulpitLifetime;
        public float pulpitSpawnTime;

        /// <summary>
        /// Populates the platform configuration from loaded platform data.
        /// </summary>
        public void PopulateFrom(PulpitData data)
        {
            if (data == null)
            {
                Debug.LogWarning("[PulpitConfig] PopulateFrom called with null data.");
                return;
            }

            minPulpitLifetime = data.min_pulpit_destroy_time;
            maxPulpitLifetime = data.max_pulpit_destroy_time;
            pulpitSpawnTime = data.pulpit_spawn_time;
        }
    }
}