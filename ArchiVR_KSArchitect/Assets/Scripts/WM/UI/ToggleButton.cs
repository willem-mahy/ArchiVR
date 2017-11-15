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
        private Button m_buttonComponent = null;

        private GameObject m_image = null;
        private Image m_imageComponent = null;

        private GameObject m_text = null;
        private Text m_textComponent = null;

        // If enabled, the behavior automatically selects the next option upon clicking the target button.
        public bool m_autoToggleOnClick = false;

        private int m_selectedOptionIndex = -1;

        private List<ToggleButtonOption> m_optionList = new List<ToggleButtonOption>();

        void Awake()
        {
            Debug.Log("ToggleButton('" + gameObject.name + "').Awake()");

            InitializeReferences();
        }

        void Start()
        {
            Debug.Log("ToggleButton('" + gameObject.name + "').Start()");

            if (m_autoToggleOnClick)
            {
                GetButtonComponent().onClick.AddListener(DoSetNextOption);
            }
        }

        private Button GetButtonComponent()
        {
            if (null == m_buttonComponent)
            {
                InitializeReferences();
            }

            return m_buttonComponent;
        }

        private Image GetImageComponent()
        {
            if (null == m_imageComponent)
            {
                InitializeReferences();
            }

            return m_imageComponent;
        }

        private Text GetTextComponent()
        {
            if (null == m_textComponent)
            {
                InitializeReferences();
            }

            return m_textComponent;
        }

        private GameObject GetText()
        {
            if (null == m_text)
            {
                InitializeReferences();
            }

            return m_text;
        }

        private GameObject GetImage()
        {
            if (null == m_image)
            {
                InitializeReferences();
            }

            return m_image;
        }

        private void InitializeReferences()
        {
            Debug.Log("ToggleButton('" + gameObject.name + "').InitializeReferences()");

            // Get reference to button component.
            m_buttonComponent = gameObject.GetComponent<Button>();

            if (null == m_buttonComponent) // Sanity: We must have a reference to the Button component
            {
                var msg = "ToggleButton '" + gameObject.name + "': Failed to get Button component!";
                Debug.LogWarning(msg);
                //throw new System.Exception(msg);
                return;
            }
            
            var childImageTransformComponent = m_buttonComponent.transform.Find("Image");

            // Get reference to child Image.
            m_image = childImageTransformComponent.gameObject;

            if (null == m_image) // Sanity: We must have a reference to the Button's child Image
            {
                var msg = "ToggleButton '" + gameObject.name + "': Failed to get child Image!";
                Debug.LogWarning(msg);
                //throw new System.Exception(msg);
                return;
            }

            // Get reference to child Image component.
            m_imageComponent = childImageTransformComponent.GetComponentInChildren<Image>();

            if (null == m_imageComponent) // Sanity: We must have a reference to the Button's child Image's Image Component
            {
                var msg = "ToggleButton '" + gameObject.name + "': Failed to get child Image's Image component!";
                Debug.LogWarning(msg);
                //throw new System.Exception(msg);
                return;
            }

            var childTextTransformComponent = m_buttonComponent.transform.Find("Text");

            // Get reference to child Text.
            m_text = childTextTransformComponent.gameObject;

            if (null == m_text) // Sanity: We must have a reference to the Button's child Text
            {
                var msg = "ToggleButton '" + gameObject.name + "': Failed to get child Text!";
                Debug.LogWarning(msg);
                //throw new System.Exception(msg);
                return;
            }

            // Get reference to child Text component.
            m_textComponent = childTextTransformComponent.GetComponentInChildren<Text>();

            if (null == m_textComponent) // Sanity: We must have a reference to the Button's child Text's Text componente
            {
                var msg = "ToggleButton '" + gameObject.name + "': Failed to get Text component!";
                Debug.LogWarning(msg);
                //throw new System.Exception(msg);
                return;
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
            Debug.Log("ToggleButton('" + gameObject.name + "').LoadOptions()");

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
            GetImage().SetActive(enableImage);

            // If no texts supplied for the options, hide the button text.
            bool enableText = HasOptions() && (null != optionTextList);
            GetText().SetActive(enableText);

            if (HasOptions())
            {
                SelectOptionByIndex(0);
            }
        }

        public bool HasOptions()
        {
            return (m_optionList.Count > 0);
        }

        /*! Linearily searches the list of options for the first option that has the given text.
         *
         * \return The index into the option list of the first matching option, or -1 if no matching option found.
         */
        public int GetOptionIndexByText(string text)
        {
            for (int optionIndex = 0; optionIndex < m_optionList.Count; ++optionIndex)
            {
                if (m_optionList[optionIndex].m_text == text)
                {
                    return optionIndex;
                }
            }

            return -1;
        }

        /*! Selects the first option that has the given text.  If no matching option found, sets selection to 'none'.
         * 
         * \return The index of the newly selected option, or -1 if no matching option found.
         */
        public int SelectOptionByText(string text)
        {
            int optionIndex = GetOptionIndexByText(text);
            SelectOptionByIndex(optionIndex);
            return m_selectedOptionIndex;
        }

        //! Query whether the list of options contains an option that has the given text.
        public bool HasOptionWithText(string text)
        {
            foreach (var option in m_optionList)
            {
                if (option.m_text == text)
                {
                    return true;
                }
            }

            return false;
        }

        //! Get the currently selected option index.
        public int GetSelectedOptionIndex()
        {
            return m_selectedOptionIndex;
        }

        //! Get the currently selected option, or null if no option selected.
        private ToggleButtonOption GetSelectedOption()
        {
            return (m_selectedOptionIndex == -1) ? null : m_optionList[m_selectedOptionIndex];
        }

        // Set the currently selected option.
        public void SelectOptionByIndex(int optionIndex)
        {
            m_selectedOptionIndex = optionIndex;

            UpdateToSelectedOption();
        }

        private void UpdateToSelectedOption()
        {
            var selectedOption = (m_selectedOptionIndex == -1) ? null : m_optionList[m_selectedOptionIndex];

            GetImageComponent().sprite = (null == selectedOption) ? null : selectedOption.m_sprite;
            GetTextComponent().text = (null == selectedOption) ? "" : selectedOption.m_text;
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
            var nextOptionIndex = (HasOptions() ? ++m_selectedOptionIndex % NumOptions() : -1);
            SelectOptionByIndex(nextOptionIndex);
        }

        public int SetPreviousOption()
        {
            DoSetPreviousOption();
            return m_selectedOptionIndex;
        }

        public void DoSetPreviousOption()
        {
            var previousOptionIndex = (HasOptions() ? --m_selectedOptionIndex % NumOptions() : -1);
            SelectOptionByIndex(previousOptionIndex);
        }

        public void SetFirstOption()
        {
            SelectOptionByIndex(0);
        }

        public void SetLastOption()
        {
            SelectOptionByIndex(NumOptions() - 1);
        }

        public void OnPointerClick()
        {
            DoSetNextOption();
        }        
    }
}
