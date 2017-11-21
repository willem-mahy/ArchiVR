using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.WM.Settings;
using Assets.Scripts.WM.UI;
using Assets.Scripts.WM.Util;

namespace Assets.Scripts.WM.ArchiVR.Menu
{
    public class MenuGraphicsSettings : MonoBehaviour
    {
        //! The button to close this menu.
        public Button m_exitButton = null;

        //! The 'Enable Dynamic Grass' button.
        public Button m_enableDynamicGrassButton = null;

        //! The 'Show FPS' button.
        public Button m_showFPSButton = null;

        //! The 'Quality Level' button.
        public ToggleButton m_qualityButton = null;

        // Use this for initialization
        void Start()
        {
            m_exitButton.onClick.AddListener(ExitButton_OnClick);
            m_enableDynamicGrassButton.onClick.AddListener(EnableDynamicGrassButton_OnClick);
            m_showFPSButton.onClick.AddListener(ShowFPSButton_OnClick);
            m_qualityButton.gameObject.GetComponent<Button>().onClick.AddListener(QualityButton_OnClick);

            InitQualityButton();
        }

        // Use this for initialization
        void Update()
        {
            var s = ApplicationSettings.GetInstance().m_data.m_graphicSettings;

            m_enableDynamicGrassButton.GetComponent<CheckBox>().SetCheckedState(s.m_enableDynamicGrass);
            m_showFPSButton.GetComponent<CheckBox>().SetCheckedState(s.m_showFPS);
            m_qualityButton.SelectOptionByText(s.m_qualityLevelName);        
        }

        private void InitQualityButton()
        {
            Debug.Log("MenuGraphicsSettings.InitQualityButton()");

            DebugUtil.LogQualitySettings();

            // Get the supported Quality setting names.
            string[] names = QualitySettings.names;

            // Get the currently active Quality setting name.
            var activeName = names[QualitySettings.GetQualityLevel()];


            // Initialize the options on the toggle button.
            List<string> qualityOptions = new List<string>();
            foreach (var name in names)
            {
                qualityOptions.Add(name);
            }

            m_qualityButton.LoadOptions(qualityOptions, null);

            // Set the currently active Quality setting as selected on the toggle button.
            m_qualityButton.SelectOptionByText(activeName);
        }

        void QualityButton_OnClick()
        {
            Debug.Log("MenuGraphicsSettings.QualityButton_OnClick()");
            var qualityLevel = m_qualityButton.SetNextOption();

            ApplicationSettings.GetInstance().SetGraphicSettingsQualityLevel(qualityLevel);                
        }

        void ExitButton_OnClick()
        {
            Debug.Log("MenuGraphicsSettings.ExitButton_OnClick()");
            UIManager.GetInstance().CloseMenu();
        }

        void EnableDynamicGrassButton_OnClick()
        {
            Debug.Log("MenuGraphicsSettings.EnableDynamicGrassButton_OnClick()");
            ApplicationSettings.GetInstance().m_data.m_graphicSettings.m_enableDynamicGrass = !ApplicationSettings.GetInstance().m_data.m_graphicSettings.m_enableDynamicGrass;
        }

        void ShowFPSButton_OnClick()
        {
            Debug.Log("MenuGraphicsSettings.ShowFPSButton_OnClick()");
            var s = ApplicationSettings.GetInstance().m_data.m_graphicSettings;

            s.m_showFPS = !s.m_showFPS;

            UIManager.GetInstance().GetWidgetByName("WidgetFPS").SetVisible(s.m_showFPS);
        }
    }
}

