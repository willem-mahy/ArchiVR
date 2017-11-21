using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.WM.CameraNavigation;
using Assets.Scripts.WM.CameraNavigation.RotationControl;


namespace Assets.Scripts.WM.UI
{
    public class ButtonRotateMode : MonoBehaviour
    {
        enum Modes
        {
            None = -1,
            Gyro = 0,
            Touch,
            MouseKB,
            NumControlModes
        };

        public Camera m_camera = null;

        public List<GameObject> m_touchControls = new List<GameObject>();

        private Button m_button = null;

        private Sprite m_spriteGyro = null;
        private Sprite m_spriteTouch = null;
        private Sprite m_spriteMouseKB = null;

        private int m_controlModeIndex = -1;

        private List<Modes> m_supportedControlModes = new List<Modes>();


        void Start()
        {
            // Compose list of control modes that are supported by the system hardware.
            // When running in the editor, we enable all control modes, even non-supported ones, in order to debug.
            if (SystemInfo.supportsGyroscope || Application.isEditor)
            {
                m_supportedControlModes.Add(Modes.Gyro);
            }

            if (Input.touchSupported || Application.isEditor)
            {
                m_supportedControlModes.Add(Modes.Touch);
            }

            if (Input.mousePresent || Application.isEditor)
            {
                m_supportedControlModes.Add(Modes.MouseKB);
            }

            m_spriteGyro = Resources.Load<Sprite>("Menu/ControlMode/Gyro");
            m_spriteTouch = Resources.Load<Sprite>("Menu/ControlMode/Touch");
            m_spriteMouseKB = Resources.Load<Sprite>("Menu/ControlMode/MouseKB");

            m_button = gameObject.GetComponent<Button>();

            if (m_button)
            {
                if (GetNumSupportedControlModes() < 2)
                {
                    m_button.GetComponent<Button>().interactable = SystemInfo.supportsGyroscope;
                }
                else
                {
                    m_button.onClick.AddListener(OnClick);
                }
            }

            Modes initialMode = SystemInfo.supportsGyroscope ? Modes.Gyro :
                                    Input.touchSupported ? Modes.Touch :
                                    Modes.MouseKB;

            SetControlMode(GetModeIndex(initialMode));
        }

        private int GetModeIndex(Modes mode)
        {
            for (int i = 0; i < m_supportedControlModes.Count; ++i)
            {
                if (m_supportedControlModes[i] == mode)
                    return i;
            }

            return -1;
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void SetControlMode(Modes mode)
        {
            SetControlMode(GetModeIndex(mode));
        }

        private void SetControlMode(int modeIndex)
        {
            m_controlModeIndex = modeIndex;

            Modes mode = (m_controlModeIndex == -1 ? Modes.None : m_supportedControlModes[m_controlModeIndex]);

            var button1 = GetComponentInChildren<Button>();
            var button = button1.transform.Find("Image").GetComponentInChildren<Image>();
            
            var cameraNavigation = GameObject.Find("CameraNavigation");
            var cameraNavigationComponent = cameraNavigation.GetComponent<CameraNavigation.CameraNavigation>();

            
            switch (mode)
            {
                case Modes.None:
                    gameObject.SetActive(false);
                    button.sprite = null;

                    SetTouchControlsActive(false);

                    cameraNavigationComponent.SetActiveRotationControlModeByName("RotationControlMouse");

                    /*
                    controlMouseKB.enabled = false;                    
                    controlCameraRotateByGyro.enabled = false;
                    WMCameraRotateBySwipe.enabled = false;
                    */
                    break;
                case Modes.Gyro:
                    gameObject.SetActive(true);
                    button.sprite = m_spriteGyro;

                    SetTouchControlsActive(true);

                    cameraNavigationComponent.SetActiveRotationControlModeByName("RotationControlMouse");

                    /*
                    controlMouseKB.enabled = false;
                    controlCameraRotateByGyro.enabled = true;
                    WMCameraRotateBySwipe.enabled = false;
                    */
                    break;
                case Modes.Touch:
                    gameObject.SetActive(true);
                    button.sprite = m_spriteTouch;

                    SetTouchControlsActive(true);

                    cameraNavigationComponent.SetActiveRotationControlModeByName("RotationControlSwipe");

                    /*
                    controlMouseKB.enabled = false;
                    controlCameraRotateByGyro.enabled = false;
                    WMCameraRotateBySwipe.enabled = true;
                    */
                    break;
                case Modes.MouseKB:
                    gameObject.SetActive(true);
                    button.sprite = m_spriteMouseKB;

                    SetTouchControlsActive(false);

                    cameraNavigationComponent.SetActiveRotationControlModeByName("RotationControlMouse");

                    /*
                    controlMouseKB.enabled = true;
                    controlCameraRotateByGyro.enabled = false;
                    WMCameraRotateBySwipe.enabled = false;
                    */
                    break;
            }
        }

        private void SetTouchControlsActive(bool state)
        {
            foreach (var touchControl in m_touchControls)
            {
                touchControl.SetActive(state);
            }
        }

        private void OnClick()
        {
            SetControlMode(GetNextSupportedControlModeIndex());
        }

        private int GetNumSupportedControlModes()
        {
            return m_supportedControlModes.Count;
        }

        private int GetNextSupportedControlModeIndex()
        {
            if (GetNumSupportedControlModes() == 0)
            {
                return -1;
            }

            return (m_controlModeIndex + 1) % GetNumSupportedControlModes();
        }
    }
}
 
 