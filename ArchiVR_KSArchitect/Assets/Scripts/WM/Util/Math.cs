using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WM.Util
{
    public class Math
    {
        // Reformat given angle (expressed in degrees) to be expressed in the range [-180, 180]
        public static float FormatAngle180(float angle)
        {
            while (angle < 180)
                angle += 360;

            while (angle > 180)
                angle -= 360;

            return angle;
        }
    }
}
