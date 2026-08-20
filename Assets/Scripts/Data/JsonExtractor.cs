using System;
using UnityEngine;

namespace Doofus.Data
{
    // Loads and validates game configuration data from the Doofus Diary.
    public static class JsonExtractor
    {
        private const float DefaultMoveSpeed = 5f;
        private const float DefaultMinPulpitLifetime = 3f;
        private const float DefaultMaxPulpitLifetime = 6f;
        private const float DefaultSpawnTime = 1.5f;

        /// <summary>
        /// Loads the Doofus Diary from Resources and falls back to default values if loading or validation fails.
        /// </summary>
        public static bool TryLoad(string resourcePath, out DoofusDiaryData data)
        {
            TextAsset asset = Resources.Load<TextAsset>(resourcePath);

            if (asset == null)
            {
                Debug.LogWarning($"[JsonExtractor] Doofus Diary not found at Resources/{resourcePath}. Using defaults.");
                data = BuildDefaults();
                return false;
            }

            DoofusDiaryData parsed;
            try
            {
                parsed = JsonUtility.FromJson<DoofusDiaryData>(asset.text);
            }
            catch (ArgumentException e)
            {
                Debug.LogWarning($"[JsonExtractor] Failed to parse Doofus Diary JSON: {e.Message}. Using defaults.");
                data = BuildDefaults();
                return false;
            }

            if (parsed?.player_data == null || parsed.pulpit_data == null)
            {
                Debug.LogWarning("[JsonExtractor] Doofus Diary JSON is missing 'player_data' or 'pulpit_data'. Using defaults.");
                data = BuildDefaults();
                return false;
            }

            if (!Validate(parsed, out string reason))
            {
                Debug.LogWarning($"[JsonExtractor] Doofus Diary failed validation ({reason}). Using defaults.");
                data = BuildDefaults();
                return false;
            }

            data = parsed;
            return true;
        }

        // Validates the loaded configuration values.
        private static bool Validate(DoofusDiaryData d, out string reason)
        {
            if (d.player_data.speed <= 0f)
            {
                reason = $"speed must be > 0 (was {d.player_data.speed})";
                return false;
            }

            var p = d.pulpit_data;

            if (p.min_pulpit_destroy_time > p.max_pulpit_destroy_time)
            {
                reason = $"min_pulpit_destroy_time ({p.min_pulpit_destroy_time}) must be <= max_pulpit_destroy_time ({p.max_pulpit_destroy_time})";
                return false;
            }

            if (p.pulpit_spawn_time <= 0f || p.pulpit_spawn_time >= p.min_pulpit_destroy_time)
            {
                reason = $"pulpit_spawn_time ({p.pulpit_spawn_time}) must be > 0 and < min_pulpit_destroy_time ({p.min_pulpit_destroy_time})";
                return false;
            }

            reason = null;
            return true;
        }

        // Creates default configuration values when the diary cannot be loaded.
        private static DoofusDiaryData BuildDefaults() => new DoofusDiaryData
        {
            player_data = new PlayerData { speed = DefaultMoveSpeed },
            pulpit_data = new PulpitData
            {
                min_pulpit_destroy_time = DefaultMinPulpitLifetime,
                max_pulpit_destroy_time = DefaultMaxPulpitLifetime,
                pulpit_spawn_time = DefaultSpawnTime
            }
        };
    }
}