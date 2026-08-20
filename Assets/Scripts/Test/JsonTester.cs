using UnityEngine;
using Doofus.Data;

namespace Doofus.Test
{
    public class JsonTester : MonoBehaviour
    {
        [SerializeField] private string resourcePath = "doofus_diary";

        void Start()
        {
            bool ok = JsonExtractor.TryLoad(resourcePath, out DoofusDiaryData data);

            Debug.Log($"[JsonTester] TryLoad returned {ok}");
            Debug.Log($"[JsonTester] speed={data.player_data.speed}, minDestroyTime={data.pulpit_data.min_pulpit_destroy_time}, maxDestroyTime={data.pulpit_data.max_pulpit_destroy_time}, pulpitSpawnTime={data.pulpit_data.pulpit_spawn_time}");
        }
    }
}