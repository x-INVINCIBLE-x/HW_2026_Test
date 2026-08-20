using UnityEngine;

namespace Doofus.Data
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Doofus/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        public float moveSpeed;

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