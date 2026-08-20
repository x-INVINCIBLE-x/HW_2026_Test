using System;
using Doofus.Player;
using UnityEngine;

namespace Doofus.Gameplay
{
    // Controls the lifetime and player interaction state of a platform.
    public class Platform : MonoBehaviour
    {
        public event Action<Platform> Expired;
        public event Action<Platform> PlayerEntered;

        public float Lifetime { get; private set; }
        public float ElapsedTime { get; private set; }

        private bool hasPlayerEntered;

        /// <summary>
        /// Initializes the platform with its lifetime and resets its state.
        /// </summary>
        public void Initialize(float lifetime)
        {
            Lifetime = lifetime;
            ElapsedTime = 0f;
            hasPlayerEntered = false;
        }

        private void Update()
        {
            ElapsedTime += Time.deltaTime;

            if (ElapsedTime >= Lifetime)
            {
                Expired?.Invoke(this);
                gameObject.SetActive(false);
            }
        }

        // Detects when the player first enters the platform.
        private void OnCollisionEnter(Collision collision)
        {
            if (hasPlayerEntered)
                return;

            if (!collision.gameObject.TryGetComponent<PlayerController>(out _))
                return;

            hasPlayerEntered = true;
            PlayerEntered?.Invoke(this);
        }
    }
}