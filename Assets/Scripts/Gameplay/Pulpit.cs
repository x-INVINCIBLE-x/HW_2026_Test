using System;
using UnityEngine;

namespace Doofus.Gameplay
{
    public class Pulpit : MonoBehaviour
    {
        public event Action<Pulpit> Expired;

        public float Lifetime { get; private set; }
        public float ElapsedTime { get; private set; }

        public void Initialize(float lifetime)
        {
            Lifetime = lifetime;
            ElapsedTime = 0f;
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
    }
}