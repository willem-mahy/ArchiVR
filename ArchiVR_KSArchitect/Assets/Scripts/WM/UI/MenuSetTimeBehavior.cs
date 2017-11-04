using UnityEngine;
using Assets.Scripts.WM.ArchiVR;

namespace Assets.Scripts.WM.UI
{
    public class MenuSetTimeBehavior : MonoBehaviour
    {

        public TimeBehavior m_time = null;

        public DPadBehavior m_dpadAnimation = null;

        public DPadBehavior m_dpadTime = null;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            float s = gameObject.GetComponent<Canvas>().scaleFactor;

            if (m_dpadAnimation)
            {
                Vector2 offset = m_dpadAnimation.GetStickOffset();

                // Compute normalized value (range [-1, 1])
                float w = s * m_dpadAnimation.GetComponent<RectTransform>().rect.width;
                float valueNormalized = Mathf.Clamp(offset.x / (0.5f * w), -1, 1); ;

                float maxAnimationSpeed = 2.0f * 60.0f * 24.0f; // max 1 day / 30 sec
                float animationSpeed = valueNormalized * maxAnimationSpeed;

                if (m_time)
                {
                    m_time.m_animationSpeed = (int)animationSpeed;
                }
            }

            if (m_dpadTime)
            {
                Vector2 offset = m_dpadTime.GetStickOffset();

                // Compute normalized value (range [0, 1])
                float w = s * m_dpadTime.GetComponent<RectTransform>().rect.width;
                float valueNormalized = Mathf.Clamp((offset.x / w) + 0.5f, 0, 1);

                float time = valueNormalized * 60.0f * 60.0f * 24.0f;

                if (m_time)
                {
                    m_time.m_time = time;
                }
            }
        }
    }
}
