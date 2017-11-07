﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

            // Use this for initialization
            void Start()
            {
                m_exitButton.onClick.AddListener(ExitButton_OnClick);
                m_enableDynamicGrassButton.onClick.AddListener(EnableDynamicGrassButton_OnClick);
                m_showFPSButton.onClick.AddListener(ShowFPSButton_OnClick);
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

