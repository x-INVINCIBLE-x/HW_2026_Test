using Doofus.Gameplay;
using TMPro;
using UnityEngine;

namespace Doofus.UI
{
    public class PlatformTimerUI : MonoBehaviour
    {
        [SerializeField] private Platform platform;
        [SerializeField] private TMP_Text timerText;

        private void Awake()
        {
            if (platform == null)
                platform = GetComponentInParent<Platform>();
        }

        private void Update()
        {
            if (platform == null || timerText == null)
                return;

            float remainingTime = Mathf.Max(0f, platform.Lifetime - platform.ElapsedTime);

            timerText.text = remainingTime.ToString("0.0");
        }
    }
}