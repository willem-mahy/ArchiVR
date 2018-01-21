using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.WM.ArchiVR;
using Assets.WM.Script.UI.Menu;

namespace Assets.Scripts.WM.UI
{
    public class MenuSetTimeBehavior : WMMenu
    {
        //!
        public TimeBehavior m_time = null;

        //! The button to close this menu.
        public Button m_exitButton = null;

        public Text m_textTime = null;

        public Text m_textAnimationSpeed = null;

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
        public new void Update()
        {
            base.Update();

            if (m_dpadAnimation)
            {                
                // Update time slider to the currently set time animation speed.
            }

            UpdateTimeSlider();            
        }

        //! Update time slider to the currently set time.
        private void UpdateTimeSlider()
        {
            if (null == m_dpadTime)
            {
                return;
            }

            if (null == m_time)
            {
                return;
            }

            float s = gameObject.GetComponentInParent<Canvas>().scaleFactor;

            var time = m_time.m_hour + m_time.m_fractionOfHour;

            var timeInDay = time % 24;

            if (timeInDay < 0)
                timeInDay += 24.0f;

            var timeInDayNormalized = timeInDay / 24.0f;

            //timeInDayNormalized = timeInDayNormalized * 2 - 1;

            var v = new Vector2(timeInDayNormalized, 0);

            m_dpadTime.SetStickOffsetNormalized(v, s);

            var maxas = 3600;

            float a = 0;

            if (m_time.m_animationSpeed > maxas)
                a = 1;
            else if (m_time.m_animationSpeed < -maxas)
                a = 0;
            else
            {
                a = ((float)m_time.m_animationSpeed) / maxas;

                a = (a + 1) / 2;
            }

            m_dpadAnimation.SetStickOffsetNormalized(new Vector2(a, 0), s);

            m_textAnimationSpeed.text = m_time.m_animationSpeed + "x";

            m_textTime.text = m_time.m_hour + "h" + (m_time.m_fractionOfHour * 60) + "m";
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
