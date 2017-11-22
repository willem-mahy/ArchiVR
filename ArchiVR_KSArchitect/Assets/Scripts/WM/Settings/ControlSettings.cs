using System;

namespace Assets.Scripts.WM.Settings
{
    [Serializable()]
    public class ControlSettings
    {
        /*
        public enum NavigationMode
        {
            FPS = 0,
            Fly,
            Teleport,
            Tracked
        }

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
        */

        public string m_navigationMode = null;

        public string m_rotateMode = null;

        public string m_translateMode = null;
    }
}
