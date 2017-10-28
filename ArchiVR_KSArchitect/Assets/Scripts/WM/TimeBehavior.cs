using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.WM
{
    public class TimeBehavior : MonoBehaviour
    {
        public Text m_timeText = null;

        public int m_animationSpeed = 1;

        public float m_time = 0; // in seconds;
        public float m_delta = 0; // last delta time, in seconds
        public int m_hour = 0;
        public float m_fractionOfHour = 0;
        public int m_nextHour = 1;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimatioSpeedFromKeyboard();

            m_delta = m_animationSpeed * Time.deltaTime;

            m_time += m_delta;

            // Compute hour
            float timeInHours = m_time / 3600.0f;

            // Compute hour index
            m_hour = (int)timeInHours % 24;

            if (m_hour >= 24)
                m_hour = 0;

            // Compute portion of an hour 
            m_fractionOfHour = (m_time % 3600.0f) / 3600.0f;

            // Compute index for 'next' hour's color.
            m_nextHour = (m_hour + 1) % 24;

            if (m_nextHour >= 24)
                m_nextHour = 0;

            if (m_timeText)
            {
                float minutesOfHour = m_fractionOfHour * 60;
                var minutes2DigitString = (minutesOfHour < 10 ? "0" : "") + (int)minutesOfHour;

                var hourString2Digit = (m_hour < 10 ? " " : "") + m_hour;

                m_timeText.text = (hourString2Digit + ":" + minutes2DigitString + "\n" + "x" + m_animationSpeed);
            }
        }

        public void buttonTimeMidnight_OnButtonClick(BaseEventData obj)
        {
            m_time = 0;
        }

        public void buttonTimeSunset_OnButtonClick(BaseEventData obj)
        {
            m_time = 3600 * 6;
        }

        public void buttonTimeSunDawn_OnButtonClick(BaseEventData obj)
        {
            m_time = 3600 * 18;
        }

        public void buttonTimeNoon_OnButtonClick(BaseEventData obj)
        {
            m_time = 3600 * 12;
        }

        void UpdateAnimatioSpeedFromKeyboard()
        {
            if (Input.GetKeyUp("b")) // backward speed
            {
                switch (m_animationSpeed)
                {
                    case 0:
                    case 1:
                        --m_animationSpeed;
                        break;
                    default:
                        if (m_animationSpeed < 0)
                            m_animationSpeed = m_animationSpeed * 2;
                        else
                            m_animationSpeed = m_animationSpeed / 2;
                        break;
                }
            }

            if (Input.GetKeyUp("f")) // forward speed
            {
                switch (m_animationSpeed)
                {
                    case 0:
                    case -1:
                        ++m_animationSpeed;
                        break;
                    default:
                        if (m_animationSpeed > 0)
                            m_animationSpeed = m_animationSpeed * 2;
                        else
                            m_animationSpeed = m_animationSpeed / 2;
                        break;
                }
            }

            if (Input.GetKeyUp("s")) // stop animating
            {
                m_animationSpeed = 0;
            }
        }
    }
}