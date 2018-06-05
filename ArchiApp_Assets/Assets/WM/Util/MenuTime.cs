using UnityEngine;
using UnityEngine.UI;

using Assets.WM.Util;
using Assets.WM.Script.UI.Menu;

namespace Assets.WM.UI
{
    public class MenuTime : WMMenu
    {
        //! The time component to be edited in this menu.
        public TimeBehavior m_time = null;

        public int m_maxAnimationSpeed = 3600;

        //! The text control showing the current time, formatted as '..h:..m:..s'.
        public Text m_textTime = null;

        //! The text control showing the current time, formatted as '..x'.
        public Text m_textAnimationSpeed = null;

        //! The slider to show/adjust the current time.
        public DPadBehavior m_dpadAnimation = null;

        //! The slider to show/adjust the current time animation speed.
        public DPadBehavior m_dpadTime = null;

        public Button m_buttonSunset = null;
        public Button m_buttonNoon = null;
        public Button m_buttonSundawn = null;
        public Button m_buttonMidnight = null;

        // Update is called once per frame
        public new void Start()
        {
            base.Start();

            if (null != m_buttonSunset)
                m_buttonSunset.onClick.AddListener(ButtonTimeSunset_OnButtonClick);

            if (null != m_buttonNoon)
                m_buttonNoon.onClick.AddListener(ButtonTimeNoon_OnButtonClick);

            if (null != m_buttonSundawn)
                m_buttonSundawn.onClick.AddListener(ButtonTimeSunDawn_OnButtonClick);

            if (null != m_buttonMidnight)
                m_buttonMidnight.onClick.AddListener(ButtonTimeMidnight_OnButtonClick);
        }

        // Update is called once per frame
        public new void Update()
        {
            base.Update();
            
            UpdateTimeSlider();

            UpdateAnimationSpeedSlider();

            m_textAnimationSpeed.text = m_time.m_animationSpeed + "x";

            var mins = ((int)(m_time.m_fractionOfHour * 60)).ToString();
            if (mins.Length == 1)
            {
                mins = "0" + mins;
            }

            var secs = ((int)((m_time.m_fractionOfHour * 3600) % 60)).ToString();
            if (secs.Length == 1)
            {
                secs = "0" + secs;
            }

            m_textTime.text =   m_time.m_hour + "h" +
                                ":" + mins + "m" +
                                ":" + secs + "s";
        }

        public void ButtonTimeMidnight_OnButtonClick()
        {
            m_time.SetTime(0, 0, 0);
        }

        public void ButtonTimeSunset_OnButtonClick()
        {
            m_time.SetTime(6, 0, 0);
        }

        public void ButtonTimeNoon_OnButtonClick()
        {
            m_time.SetTime(12, 0, 0);
        }

        public void ButtonTimeSunDawn_OnButtonClick()
        {
            m_time.SetTime(18, 0, 0);
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

            var v = new Vector2(timeInDayNormalized, 0);

            m_dpadTime.SetStickOffsetNormalized(v, s);           
        }

        private void UpdateAnimationSpeedSlider()
        {
            if (!m_dpadAnimation)
            {
                return;
            }

            float s = gameObject.GetComponentInParent<Canvas>().scaleFactor;

            float a = 0;

            if (m_time)
            {
                if (m_time.m_animationSpeed > m_maxAnimationSpeed)
                    a = 1;
                else if (m_time.m_animationSpeed < -m_maxAnimationSpeed)
                    a = 0;
                else
                {
                    a = ((float)m_time.m_animationSpeed) / m_maxAnimationSpeed;

                    a = (a + 1) / 2;
                }
            }

            m_dpadAnimation.SetStickOffsetNormalized(new Vector2(a, 0), s);
        }
    }
}
