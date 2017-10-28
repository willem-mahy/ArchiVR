using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Toggles active state of a list of target GameObjects, upon following triggers:
// - clicking a designated button
// - tapping on the screen where there are no UI controls.
namespace Assets.Scripts.WM
{
    public class ToggleActiveBehavior : MonoBehaviour
    {
        // Flags whether a touch is in progress that can be further considered as a valid trigger tap.
        private bool m_considerTouch = false;
        private float m_touchStartTime;

        public bool m_active = true;
        public List<GameObject> m_targetGameObjectArray = new List<GameObject>();

        public string m_toggleKeyCode = "m";

        // Use this for initialization
        void Start()
        {
        }

        /// <summary>
        /// Cast a ray to test if Input.mousePosition is over any UI object in EventSystem.current.
        /// This is a replacement for IsPointerOverGameObject(),
        /// which does not work on Android in 4.6.0f3
        /// </summary>
        private bool IsPointerOverUIObject(Touch t)
        {
            // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
            // the ray cast appears to require only eventData.position.
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(t.position.x, t.position.y);

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        // Update is called once per frame
        void Update()
        {
            HandleTouches();

            if (null != m_toggleKeyCode)
            {
                // Show/hide menu
                if (Input.GetKeyUp(m_toggleKeyCode))
                {
                    TogglActiveState();
                }
            }
        }

        void HandleTouches()
        {
            switch (Input.touchCount)
            {
                case 1:
                    {
                        var touch = Input.GetTouch(0);

                        switch (touch.phase)
                        {
                            case TouchPhase.Began:
                                // Only consider touches that are not on UI elements.
                                m_considerTouch = !IsPointerOverUIObject(touch);
                                m_touchStartTime = Time.time;
                                break;
                            case TouchPhase.Ended:
                                {
                                    if (m_considerTouch)
                                    {
                                        var touchEndTime = Time.time;
                                        var touchDuration = touchEndTime - m_touchStartTime;
                                        if (touchDuration < 0.5)
                                        {
                                            // Upon each timely ended single-touch as a 'tap' where there are no UI elements,
                                            // each tap toggles visibility of the targeted UI elements.
                                            TogglActiveState();
                                        }
                                        m_considerTouch = false;
                                        m_touchStartTime = 0;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    m_considerTouch = false;
                    m_touchStartTime = 0;
                    break;
            }
        }

        public void TogglActiveState()
        {
            SetActiveState(!m_active);
        }

        public void SetActiveState(bool active)
        {
            Debug.Log("ToggleActiveBehavior.SetActive(" + active + ")");

            m_active = active;

            foreach (var target in m_targetGameObjectArray)
            {
                target.SetActive(m_active);
            }
        }
    }
}