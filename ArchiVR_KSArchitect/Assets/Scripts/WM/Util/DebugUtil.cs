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
    }
}
