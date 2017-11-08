using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WM.Util
{
    class DebugUtil
    {
        //! Log when a keyboard key is pressed.
        static public void LogKeyPress(string keyName)
        {
            if (Input.GetKeyDown(keyName))
            {
                UnityEngine.Debug.Log(keyName + " key pressed");
            }
        }

        static public void LogQualitySettings()
        {
            // Get the supported Quality setting names.
            string[] names = QualitySettings.names;

            // Get the currently active Quality setting name.
            var activeName = names[QualitySettings.GetQualityLevel()];

            // Debug Log the QualitySettings
            var text = "QualitySettings:\n";
            foreach (var name in names)
            {
                text += name;

                if (name == activeName)
                {
                    text += " (active)";
                }

                text += "\n";
            }
            Debug.Log(text);
        }
    }
}
