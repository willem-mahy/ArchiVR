using System.IO;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WM.Settings
{
    [System.Serializable()]
    public class ControlSettings
    {
        public enum RotateMode
        {
            RotateByGyro = 0,
            RotateByTouch,
            RotateByMouseKB,
            RotateByTrakingWM,
            RotateByTrakingMicrosoftXR,
        }

        public enum TranslateMode
        {
            TranslateByMouseKB = 0,
            TranslateByTeleport,
            TranslateByGamepad,
            TranslateByTrakingWM,
            TranslateByTrakingMicrosoftXR,
        }

        public RotateMode m_rotateMode;

        public TranslateMode m_translateMode;
    }
}
