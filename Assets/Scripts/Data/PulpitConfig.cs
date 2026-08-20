using UnityEngine;

namespace Doofus.Data
{
    [CreateAssetMenu(fileName = "PulpitConfig", menuName = "Doofus/Pulpit Config")]
    public class PulpitConfig : ScriptableObject
    {
        public float minPulpitLifetime;
        public float maxPulpitLifetime;
        public float pulpitSpawnTime;

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