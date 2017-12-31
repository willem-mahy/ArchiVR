using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.WM.ArchiVR;

namespace Assets.Scripts.WM.UI
{
    public class MenuSetTimeBehavior : MonoBehaviour
    {
        //!
        public TimeBehavior m_time = null;

        //! The button to close this menu.
        public Button m_exitButton = null;       

        //!
        public DPadBehavior m_dpadAnimation = null;

        //!
        public DPadBehavior m_dpadTime = null;

        // Use this for initialization
        void Start()
        {
            //m_exitButton.onClick.AddListener(ExitButton_OnClick);
        }

        // Update is called once per frame
        void Update()
        {
            if (m_dpadAnimation)
            {                
                // Update time slider to the currently set time animation speed.
            }

            if (m_dpadTime)
            {
                // Update time slider to the currently set time.
            }
        }

        //void UpdateTimeAnimationSpeedToSliderValue()
        //{
        //    float s = gameObject.GetComponentInParent<Canvas>().scaleFactor;

        //    if (m_dpadAnimation)
        //    {
        //        Vector2 offset = m_dpadAnimation.GetStickOffset();

        //        // Compute normalized value (range [-1, 1])
        //        float w = s * m_dpadAnimation.GetComponent<RectTransform>().rect.width;
        //        float valueNormalized = Mathf.Clamp(offset.x / (0.5f * w), -1, 1); ;

        //        float maxAnimationSpeed = 2.0f * 60.0f * 24.0f; // max 1 day / 30 sec
        //        float animationSpeed = valueNormalized * maxAnimationSpeed;

        //        if (m_time)
        //        {
        //            m_time.m_animationSpeed = (int)animationSpeed;
        //        }
        //    }
        //}

        //void UpdateTimeToSliderValue()
        //{
        //    float s = gameObject.GetComponentInParent<Canvas>().scaleFactor;
            
        //    Vector2 offset = m_dpadTime.GetStickOffset();

        //    // Compute normalized value (range [0, 1])
        //    float w = s * m_dpadTime.GetComponent<RectTransform>().rect.width;
        //    float valueNormalized = Mathf.Clamp((offset.x / w) + 0.5f, 0, 1);

        //    float time = valueNormalized * 60.0f * 60.0f * 24.0f;

        //    if (m_time)
        //    {
        //        m_time.m_time = time;
        //    }
        //}
    }
}
