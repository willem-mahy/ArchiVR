using System;
using UnityEngine;
using UnityEngine.XR;

namespace Assets.Scripts.WM.Util
{
    class DebugUtil
    {
        // Log all supported XR devices that are supported on the system.
        static public void LogJoystickNames()
        {
            String text = "Input.GetJoystickNames():";

            foreach (String joystickName in Input.GetJoystickNames())
            {
                text += "\n- " + joystickName;
            }

            Debug.Log(text);
        }


        // Log all supported XR devices that are supported on the system.
        static public void LogSupportedXRDevices()
        {
            var text = "XRSettings.supportedDevices:";

            foreach (var deviceName in XRSettings.supportedDevices)
            {
                text += "\n- " + deviceName;

                if (deviceName == XRSettings.loadedDeviceName)
                {
                    text += " (loaded)";
                }
            }

            Debug.Log(text);
        }

        //! Log when a keyboard key is pressed.
        static public void LogKeyPress(string keyName)
        {
            if (Input.GetKeyDown(keyName))
            {
                UnityEngine.Debug.Log(keyName + " key pressed");
            }
        }

        //! Log when a physical joystick (gamepad) key is pressed.
        static public void LogJoystickButtonPress()
        {
            for (int i = 0; i < 10; ++i)
            {
                String name = "joystick button " + i.ToString();

                LogKeyPress(name);
            }
        }

        static public void LogQualitySettings()
        {
            // Get the supported Quality setting names.
            string[] names = QualitySettings.names;

            // Get the currently active Quality setting name.
            var activeName = names[QualitySettings.GetQualityLevel()];

            // Debug Log the QualitySettings
            var text = "QualitySettings:";
            foreach (var name in names)
            {
                text += "\n- " + name;

                if (name == activeName)
                {
                    text += " (active)";
                }
            }
            Debug.Log(text);
        }
    }
}
