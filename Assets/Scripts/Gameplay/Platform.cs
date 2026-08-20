using System;
using Doofus.Player;
using UnityEngine;

namespace Doofus.Gameplay
{
    public class Platform : MonoBehaviour
    {
        public event Action<Platform> Expired;
        public event Action<Platform> PlayerEntered;

        public float Lifetime { get; private set; }
        public float ElapsedTime { get; private set; }

        private bool hasPlayerEntered;

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