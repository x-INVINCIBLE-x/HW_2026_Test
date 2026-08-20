using Doofus.Gameplay;
using TMPro;
using UnityEngine;

namespace Doofus.UI
{
    // Displays the remaining lifetime of the platform it is attached to.
    public class PlatformTimerUI : MonoBehaviour
    {
        [SerializeField] private Platform platform;
        [SerializeField] private TMP_Text timerText;

        private void Awake()
        {
            if (platform == null)
                platform = GetComponentInParent<Platform>();
        }

        // Updates the displayed platform lifetime.
        private void Update()
        {
            if (platform == null || timerText == null)
                return;

            float remainingTime = Mathf.Max(0f, platform.Lifetime - platform.ElapsedTime);

            timerText.text = remainingTime.ToString("0.0");
        }
    }
}