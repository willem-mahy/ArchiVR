using UnityEngine;
using UnityEngine.SceneManagement;

using Assets.Scripts.WM.UI;
using Assets.Scripts.WM.Settings;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.WM.ArchiVR.Application
{
    public class ApplicationStatePlay : ApplicationState
    {
        

        // For debugging purposes: allows to start up in 'Play' state,
        // with the project designated in 'm_initialProjectSceneName'.
        public string m_initialProjectSceneName = "";

        public Widget m_widgetVirtualGamepad = null;

        public Widget m_widgetMenuPOI = null;

        override protected string GetName()
        {
            return "Play";
        }

        // Use this for initialization
        override protected void Start()
        {
            // For debugging purposes:
            // enables to open an initial project (defined by 'm_initialProjectSceneName')
            // when starting the application directly in 'Play' mode from the Unity editor.
            if (ApplicationState.s_isFirstTime)
            {
                ApplicationState.s_isFirstTime = false;

                if (m_initialProjectSceneName.Length > 0)
                {
                    OpenProject(m_initialProjectSceneName);
                }
                return;
            }

            base.Start();

            GameObject.Find("Time").GetComponent<TimeBehavior>().SetTime(12, 0, 0);

            var mapping = new Dictionary<String, String>();

            mapping["DPadLeft"]      = "Time Menu";
            mapping["DPadRight"]     = "Toggle ViewMode";
            mapping["DPadUp"]        = "Settings Menu";
            mapping["DPadDown"]      = "Graphics Menu";

            mapping[GamepadXBox.A] = "OK";
            mapping[GamepadXBox.B] = "Cancel";
            mapping[GamepadXBox.X] = "";
            mapping[GamepadXBox.Y] = "Show/Hide controls";

            mapping[GamepadXBox.L1] = "Prev POI";
            mapping[GamepadXBox.R1] = "Next POI";
            mapping[GamepadXBox.L2R2] = "";

            //mapping[GamepadXBox.Windows] = "";
            mapping[GamepadXBox.Select] = "Show/Hide menu";
            mapping[GamepadXBox.Start] = "Go to home";

            mapping["LeftJoystick"] = "Navigate menu";
            mapping["RightJoystick"] = "Look around";

            if (m_gamepadPreview)
            {
                m_gamepadPreview.SetFunctionTexts(mapping);
            }
        }

        // Update is called once per frame
        override protected void Update()
        {
            base.Update();            

            // If user presses 'p', Write the current camera location as POI.
            if (Input.GetKeyDown("p"))
            {
                var poiManager = GetComponent<POIManager>();

                if (null != poiManager)
                {
                    poiManager.WriteCurrentCameraLocationToFilePOI();
                }
            }

            // If user presses 'c', toggle Construction Lighting Mode.
            if (Input.GetKeyDown("c"))
            {
                GetComponent<ConstructionLighting>().ActivateNextLightingMode();
            }

            // If user presses 'Q', toggle Graphics Quality Mode.
            if (Input.GetKeyDown("q"))
            {
                ApplicationSettings.GetInstance().SetNextGraphicSettingsQualityLevel();
            }           

            // If user presses 'F12', next poi.
            if (Input.GetKeyDown("f12"))
            {
                GetComponent<POIManager>().ActivateNextPOI();
            }

            // If user presses 'F11', previous poi
            if (Input.GetKeyDown("f11"))
            {
                GetComponent<POIManager>().ActivatePrevPOI();
            }

            if (Input.GetKeyDown(GamepadXBox.X))
            {
                GetComponent<POIManager>().ActivateNextPOI();
            }

            if (Input.GetKeyDown(GamepadXBox.B))
            {
                GetComponent<POIManager>().ActivateNextPOI();
            }

            if (Input.GetKeyDown(GamepadXBox.L1))
            {
                GetComponent<POIManager>().ActivatePrevPOI();
            }

            if (Input.GetKeyDown(GamepadXBox.R1))
            {
                GetComponent<POIManager>().ActivateNextPOI();
            }

            if (Input.GetKeyDown(GamepadXBox.Start))
            {
                OpenHomeMenu();
            }

            if (Input.GetKeyDown(GamepadXBox.Select))
            {
                UIManager.GetInstance().OpenMenu("MenuMain");
            }

            UpdatePOIMenuVisibility();

            UpdateVirtualGamepadWidgetVisibility();
        }

        private void UpdatePOIMenuVisibility()
        {
            if (!m_widgetMenuPOI)
            {
                return;
            }

            var enablePOI = false;

            var cn = CameraNavigation.CameraNavigation.GetInstance();

            var cnm = (cn ? cn.GetActiveNavigationMode() : null);

            if (cnm)
            {
                var cnmSupportsPOI = cnm ? cnm.SupportsNavigationViaPOI() : false;

                enablePOI = cnmSupportsPOI;
            }

            var uiManager = UIManager.GetInstance();

            if (uiManager)
            {
                var menu = uiManager.GetCurrentMenu();

                if (menu)
                {
                    if (menu.name != "MenuPlay")
                    {
                        enablePOI = false;
                    }
                }
            }

            m_widgetMenuPOI.SetVisible(enablePOI);
        }

        private void UpdateVirtualGamepadWidgetVisibility()
        {
            if (!m_widgetVirtualGamepad)
            {
                return;
            }

            // Update Virtual Gamepad visibility.
            var asd = ApplicationSettings.GetInstance().m_data;
            var cameraNavigation = GameObject.Find("CameraNavigation").GetComponent<CameraNavigation.CameraNavigation>();
            var uiManager = UIManager.GetInstance();

            bool userEnabledVirtualGamepad = asd.m_controlSettings.m_enableVirtualGamepad;
            bool isUIModeScreenSpace = uiManager.GetUIMode() == UIMode.ScreenSpace;
            bool currentCameraNavigationSupportsGamepad = (null == cameraNavigation) ? true : cameraNavigation.GetActiveNavigationMode().SupportsDPadInput();
            bool isPhysicalGamePadConnected = (Input.GetJoystickNames().Length > 0);

            var menu = uiManager.GetCurrentMenu();

            bool isShowingPlayMenu = menu && menu.name == "MenuPlay";
            bool isShowingMainMenu = menu && menu.name == "MenuMain";

            bool enableVirtualGamepad =
                isShowingPlayMenu
                //isShowingMainMenu
                && isUIModeScreenSpace
                //&& !isPhysicalGamePadConnected
                && currentCameraNavigationSupportsGamepad
                && userEnabledVirtualGamepad;

            m_widgetVirtualGamepad.SetVisible(enableVirtualGamepad);

        }

        public void HomeButton_OnClick()
        {
            Debug.Log("ApplicationStatePlay.HomeButton_OnClick()");
            OpenHomeMenu();
        }

        public void MenuTimeButton_OnClick()
        {
            Debug.Log("ApplicationStatePlay.MenuTimeButton_OnClick()");

            //UIManager.GetInstance().OpenMenu("MenuTime");
            UIManager.GetInstance().OpenMenu(GameObject.Find("MenuTime").GetComponent<Assets.Scripts.WM.UI.Menu>());
        }

        public void MenuLayerButton_OnClick()
        {
            Debug.Log("ApplicationStatePlay.MenuLayerButton_OnClick()");

            UIManager.GetInstance().OpenMenu(GameObject.Find("MenuLayer").GetComponent<Assets.Scripts.WM.UI.Menu>());
        }

        public void OpenHomeMenu()
        {
            SceneManager.LoadScene("Home");
        }        
    }
}
