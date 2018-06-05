using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.WM.Util
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

        //! Makes a Vector3 from the given Vector2 (z is set to 0)
        public static Vector3 ToVector3(Vector2 v)
        {
            return new Vector3(v.x, v.y);
        }

        //! Makes a Vector2 from the given Vector2 (z is omitted)
        public static Vector2 ToVector2(Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }
    }
}
