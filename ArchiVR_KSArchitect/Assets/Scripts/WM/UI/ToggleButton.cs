using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.WM.UI
{
    public class ToggleButton : MonoBehaviour
    {
        // The target button to behave like a toggle button.
        // Can, optionally be specified via the unity inspector.
        // If not specified via the unity inspector, it is assumed
        // that this behavior is added as a direct component of the target button.
        public Button m_button = null;        

        // If enabled, the behavior automatically selects the next option upon clicking the target button.
        public bool m_autoToggleOnClick = false;

        private int m_option = -1;

        private List<Sprite> m_optionSprites = new List<Sprite>();

        private EventTrigger.Entry m_eventTrigerEntry_OnPointerClick = null;

        void Start()
        {
            if (null == m_button)
            {
                m_button = gameObject.GetComponent<Button>();
            }

            if (null != m_button)
            {
                if (m_autoToggleOnClick)
                {
                    var eventTrigger = gameObject.GetComponent<EventTrigger>();

                    // Create a new TriggerEvent and add a listener.
                    EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
                    trigger.AddListener((eventData) => OnPointerClick()); // ignore event data

                    // Create and initialise EventTrigger.Entry using the created TriggerEvent
                    m_eventTrigerEntry_OnPointerClick =
                        new EventTrigger.Entry()
                        {
                            callback = trigger,
                            eventID = EventTriggerType.PointerClick
                        };

                    // Add the EventTrigger.Entry to delegates list on the EventTrigger
                    eventTrigger.triggers.Add(m_eventTrigerEntry_OnPointerClick);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        // Use this for initialization.
        //eg: "Menu/Light/Lightbulb 01 ON transp 256p"
        public void LoadOptions(List<string> optionSpritePaths)
        {
            for (int i = 0; i < optionSpritePaths.Count; ++i)
            {
                var sprite = Resources.Load<Sprite>(optionSpritePaths[i]);
                m_optionSprites.Add(sprite);
            }

            if (m_optionSprites.Count > 0)
            {
                SetOption(0);
            }
        }

        // Get the currently selected option.
        public int GetOption()
        {
            return m_option;
        }
        
        // Set the currently selected option.
        public void SetOption(int option)
        {
            m_option = option;
            
            var buttonImage = (m_button ? m_button.transform.Find("Image").GetComponentInChildren<Image>() : null);

            if (buttonImage)
            {
                var sprite = (m_option == -1) ? null : m_optionSprites[option];
                buttonImage.sprite = sprite;
            }
        }

        public int SetNextOption()
        {
            var newOption = ++m_option % m_optionSprites.Count;
            SetOption(newOption);
            return newOption;
        }

        public void DoSetNextOption()
        {
            var newOption = ++m_option % m_optionSprites.Count;
            SetOption(newOption);
        }

        public void SetPreviousOption()
        {
            var newOption = --m_option % m_optionSprites.Count;
            SetOption(newOption);
        }

        public void SetFirstOption()
        {
            SetOption(0);
        }

        public void SetLastOption()
        {
            SetOption(m_optionSprites.Count - 1);
        }

        public void OnPointerClick()
        {
            DoSetNextOption();
        }        
    }
}
