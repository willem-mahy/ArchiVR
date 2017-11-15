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
            //! The 'Settings' parent menu.
            public GameObject m_menuSettings = null;

            //! The button to close this menu.
            public Button m_exitButton = null;

            //! The 'Rotate Mode' button.
            public Button m_enableDynamicGrassButton = null;

            //! The 'Translate Mode' button.
            public Button m_showFPSButton = null;

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
                gameObject.SetActive(false);
                m_menuSettings.SetActive(true);
            }

            void EnableDynamicGrassButton_OnClick()
            {
                Debug.Log("MenuGraphicsSettings.EnableDynamicGrassButton_OnClick()");

                // TODO:
                //m_enableDynamicGrassButton.SetSelected(!m_enableDynamicGrassButton.IsSelected());
                //m_fps.SetActive(m_enableDynamicGrassButton.IsSelected());
            }

            void ShowFPSButton_OnClick()
            {
                Debug.Log("MenuGraphicsSettings.ShowFPSButton_OnClick()");

                // TODO:
                //m_showFPSButton.SetSelected(!m_showFPSButton.IsSelected());
                //m_fps.SetActive(m_showFPSButton.IsSelected());
            }
        }
    }

