using UnityEngine;

namespace Doofus.Data
{
    // Stores player configuration data used at runtime.
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Doofus/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        public float moveSpeed;

        /// <summary>
        /// Populates the player configuration from loaded player data.
        /// </summary>
        public void PopulateFrom(PlayerData data)
        {
            if (data == null)
            {
                Debug.LogWarning("[PlayerConfig] PopulateFrom called with null data.");
                return;
            }

            moveSpeed = data.speed;
        }
    }
}