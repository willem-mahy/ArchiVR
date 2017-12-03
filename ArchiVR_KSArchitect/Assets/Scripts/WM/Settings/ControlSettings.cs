using System;

namespace Assets.Scripts.WM.Settings
{
    [Serializable()]
    public class ControlSettings
    {
        /*
        NavigationMode: one of
        - "CameraNavigationFPS"
        - "CameraNavigationFly"
        - "CameraNavigationVuforia"
        - "CameraNavigationTrakingWM" (TODO)
        - "CameraNavigationTrakingMicrosoftXR" (TODO)
        
        public static String s_RotationInputModes[] =
        {
            "RotateByGyro",
            "RotateByGravity",
            "RotateByTouch",
            "RotateByGamePad",
            "RotateByMouse",
            "RotateByKB"
        }

        public String TranslationInputModes[] =
        {
            "TranslateByKB",
            "TranslateByMouse",
            "TranslateByTeleport",
            "TranslateByGamepad"
        }
        */

        public string m_navigationMode = null;

        public string m_rotationInputMode = null;

        public string m_translationInputMode = null;
    }
}
