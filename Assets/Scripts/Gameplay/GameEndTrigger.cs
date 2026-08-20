using Doofus.Player;
using System;
using UnityEngine;

namespace Doofus.Gameplay
{
    public class GameEndTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerController _))
            {
                GameManager.Instance.EndGame();
            }
        }
    }
}