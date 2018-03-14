using Assets.Scripts.WM.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

namespace Assets.Scripts.WM.ArchiVR.Application
{
    public class ApplicationStateHome : ApplicationState
    {
        override protected string GetName()
        {
            return "Home";
        }

        // Use this for initialization
        override protected void Start()
        {
            Debug.Log("ApplicationStateHome.Start()");
            base.Start();

            var mapping = new Dictionary<String, String>();

            mapping["DPadLeft"] = "";
            mapping["DPadRight"] = "";
            mapping["DPadUp"] = "";
            mapping["DPadDown"] = "";

            mapping[GamepadXBox.A] = "OK";
            mapping[GamepadXBox.B] = "Cancel";
            mapping[GamepadXBox.X] = "";
            mapping[GamepadXBox.Y] = "Show/Hide controls";

            mapping[GamepadXBox.L1] = "Show/hide debug";
            mapping[GamepadXBox.R1] = "";
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

            // Hack to make sure the camera ends up in the correct location in Application Home state.
            Camera.main.transform.position = new Vector3(0.0f, 1.6f, 0.0f);

            // If user presses 'Esc', quit the application.
            if (Input.GetKey("escape"))
            {
                QuitApplication();
            }

            if (Input.GetKeyUp(KeyCode.LeftAlt))
            {
                var uiManager = UIManager.GetInstance();

                var currentMenuName = uiManager.GetCurrentMenu().name;
                if (currentMenuName.CompareTo("MenuMain") == 0)
                {
                    uiManager.OpenMenu("MenuSettings");
                }
            }
        }

        // OnGUI is called once per frame
        void UpdateDebugText()
        {
            var mainCamera = Camera.main;

            if (!mainCamera)
                return;

            var textDebugVR = GameObject.Find("Text_Debug_VR");

            if (!textDebugVR)
                return;

            var textDebugVRComponent = textDebugVR.GetComponent<Text>();

            if (!textDebugVRComponent)
                return;

            var cameraText =
                "Camera\n" +
                "Pos:" + mainCamera.transform.position.ToString() + "\n" +
                "Fwd:" + mainCamera.transform.forward.ToString() + "\n" +
                "Up:" + mainCamera.transform.up.ToString() + "\n";

            var text =
                "XRDevice.isPresent= " + (XRDevice.isPresent ? "true" : "false") + "\n" +
                "XRDevice.model= " + XRDevice.model + "\n" +
                "XRSettings.loadedDeviceName= " + XRSettings.loadedDeviceName + "\n" +
                cameraText;

            textDebugVRComponent.text = text;
        }

        public void QuitButton_OnClick()
        {
            Debug.Log("ApplicationStateHome.QuitButton_OnClick()");

            QuitApplication();
        }

        public void GoButton_OnClick()
        {
            ApplicationState.OpenProject();
        }
    }
}
