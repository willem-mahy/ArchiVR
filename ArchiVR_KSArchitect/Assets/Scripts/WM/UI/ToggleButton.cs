using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.WM.UI
{
    public class ToggleButtonOption
    {
        public string m_text = null;
        public Sprite m_sprite = null;
    }

    /*! A behavior to add to a button the functionality to toggle between a specified list of options.
     *  A list of texts and/or sprites must be supplied to represent the possible options.
     * 
     *  \usage  Add this behavior as a direct Script component of a Button.
     *          The button must have a Image child (named 'image') and a Text child (named 'text').
     */
    public class ToggleButton : MonoBehaviour
    {
        // The target button to behave like a toggle button.
        private Button m_button = null;

        private GameObject m_image = null;
        private Image m_imageComponent = null;

        private GameObject m_text = null;
        private Text m_textComponent = null;

        // If enabled, the behavior automatically selects the next option upon clicking the target button.
        public bool m_autoToggleOnClick = false;

        private int m_selectedOptionIndex = -1;

        private List<ToggleButtonOption> m_optionList = new List<ToggleButtonOption>();

        private EventTrigger.Entry m_eventTrigerEntry_OnPointerClick = null;

        void Start()
        {
            m_button = gameObject.GetComponent<Button>();

            if (null == m_button) // Sanity: We must have a reference to the Button
            {
                var msg = "ToggleButton '" + gameObject.name + "': Failed to get Button component!";
                Debug.Log(msg);
                //throw new System.Exception(msg);
                return;
            }

            m_image = m_button.transform.Find("Image").gameObject;
            m_imageComponent = m_button.transform.Find("Image").GetComponentInChildren<Image>();

            if (null == m_imageComponent) // Sanity: We must have a reference to the Button's Image child Image Component
            {
                var msg = "ToggleButton '" + gameObject.name + "': Failed to get child Image's Image component!";
                Debug.Log(msg);
                //throw new System.Exception(msg);
                return;
            }

            m_text = m_button.transform.Find("Text").gameObject;
            m_textComponent = m_button.transform.Find("Text").GetComponentInChildren<Text>();

            if (null == m_textComponent) // Sanity: We must have a reference to the Button's Text child
            {
                var msg = "ToggleButton '" + gameObject.name + "': Failed to get Text component!";
                Debug.Log(msg);
                //throw new System.Exception(msg);
                return;
            }

            if (m_autoToggleOnClick)
            {
                m_button.onClick.AddListener(DoSetNextOption);
                //var eventTrigger = gameObject.GetComponent<EventTrigger>();

                //// Create a new TriggerEvent and add a listener.
                //EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
                //trigger.AddListener((eventData) => OnPointerClick()); // ignore event data

                //// Create and initialise EventTrigger.Entry using the created TriggerEvent
                //m_eventTrigerEntry_OnPointerClick =
                //    new EventTrigger.Entry()
                //    {
                //        callback = trigger,
                //        eventID = EventTriggerType.PointerClick
                //    };

                //// Add the EventTrigger.Entry to delegates list on the EventTrigger
                //eventTrigger.triggers.Add(m_eventTrigerEntry_OnPointerClick);
            }
        }

        /*! Initialize the list of possible options.
         *
         *  \param[in] optionTextList           Optional.  A list with one string per option.  These strings are used to represent the selected option in the Button's Text child.
         *  \param[in] optionSpritePathList     Optional.  A list with one string per option.  These strings are relative paths to Sprites in the project resources (IE relative to 'Assets/Resources').
         *                                      If supplied, the referenced Sprites are used to represent the selected option in the Button's Image child.
         *                                      Example of a sprite path: "Menu/Light/Lightbulb 01 ON transp 256p"
         */
        public void LoadOptions(
            List<string> optionTextList,
            List<string> optionSpritePathList)
        {
            if ((null == optionSpritePathList) && (null == optionTextList))
            {
                var msg = "Neither option texts or sprites supplied!";
                Debug.Log(msg);
                //throw new System.Exception(msg);
            }

            if ((null != optionSpritePathList) && (null != optionTextList))
            {
                if (optionSpritePathList.Count != optionTextList.Count)
                {
                    if (optionSpritePathList.Count > optionTextList.Count)
                    {
                        optionTextList = null;
                    }
                    else
                    {
                        optionSpritePathList = null;
                    }

                    var msg = "Mismatch in number of option names versus sprite paths!";
                    Debug.Log(msg);
                    //throw new System.Exception(msg);
                }
            }

            int numOptions = 0;

            if (null != optionSpritePathList)
            {
                numOptions = optionSpritePathList.Count;
            }
            else if (null != optionTextList)
            {
                numOptions = optionTextList.Count;
            }

            // Initialize list of options.
            m_optionList.Clear();

            for (int i = 0; i < numOptions; ++i)
            {
                var option = new ToggleButtonOption();
            
                // Initialize option sprite
                if (null != optionSpritePathList)
                {
                    var optionSpritePath = optionSpritePathList[i];

                    // Load the Sprite from project Resources ('Assets/Resources')
                    var sprite = Resources.Load<Sprite>(optionSpritePath);

                    if (null == sprite)
                    {
                        var msg = "Option Sprite '" + optionSpritePath + "' not found in resources!";
                        Debug.Log(msg);
                        //throw new System.Exception(msg);
                    }

                    option.m_sprite = sprite;
                }

                if (null != optionTextList)
                {
                    option.m_text = optionTextList[i];
                }

                m_optionList.Add(option);
            }

            // If no sprites supplied for the options, hide the button image.
            bool enableImage = HasOptions() && (null != optionSpritePathList);
            m_image.SetActive(enableImage);

            // If no texts supplied for the options, hide the button text.
            bool enableText = HasOptions() && (null != optionTextList);
            m_text.SetActive(enableText);

            if (HasOptions())
            {
                SetOption(0);
            }
        }

        public bool HasOptions()
        {
            return (m_optionList.Count > 0);
        }

        // Get the currently selected option.
        public int GetOption()
        {
            return m_selectedOptionIndex;
        }

        // Set the currently selected option.
        public void SetOption(int optionIndex)
        {
            m_selectedOptionIndex = optionIndex;

            UpdateToSelectedOption();
        }

        private void UpdateToSelectedOption()
        {
            var selectedOption = (m_selectedOptionIndex == -1) ? null : m_optionList[m_selectedOptionIndex];

            m_imageComponent.sprite = selectedOption.m_sprite;
            m_textComponent.text = selectedOption.m_text;
        }

        int NumOptions()
        {
            return m_optionList.Count;
        }

        public int SetNextOption()
        {
            DoSetNextOption();
            return m_selectedOptionIndex;
        }

        public void DoSetNextOption()
        {
            var newOption = (HasOptions() ? ++m_selectedOptionIndex % NumOptions() : -1);
            SetOption(newOption);
        }

        public int SetPreviousOption()
        {
            DoSetPreviousOption();
            return m_selectedOptionIndex;
        }

        public void DoSetPreviousOption()
        {
            var newOption = (HasOptions() ? --m_selectedOptionIndex % NumOptions() : -1);
            SetOption(newOption);
        }

        public void SetFirstOption()
        {
            SetOption(0);
        }

        public void SetLastOption()
        {
            SetOption(NumOptions() - 1);
        }

        public void OnPointerClick()
        {
            DoSetNextOption();
        }        
    }
}
