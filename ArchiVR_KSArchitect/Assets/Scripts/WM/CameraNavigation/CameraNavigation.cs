using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.WM.CameraNavigation.RotationControl;
using Assets.Scripts.WM.CameraNavigation.TranslationControl;
using Assets.Scripts.WM.Settings;

namespace Assets.Scripts.WM.CameraNavigation
{
    public class CameraNavigation : MonoBehaviour
    {
        // Name of the initial active navigation mode.
        public string m_initialActiveNavigationMode = null;

        // Index of the current active navigation mode.
        private int m_activeNavigationModeIndex = -1;

        // List of supported navigation modes.
        public List<CameraNavigationModeBase> m_navigationModeList = new List<CameraNavigationModeBase>();

        // Rotation mode
        public string m_initialActiveRotationMode = null;

        private int m_activeRotationControlIndex = -1;
        
        public List<RotationControlBase> m_rotationControlModeList = new List<RotationControlBase>();

        // Rotation mode
        public string m_initialActiveTranslationMode = null;

        private int m_activeTranslationControlIndex = -1;

        public List<TranslationControlBase> m_translationControlModeList = new List<TranslationControlBase>();

        // Use this for initialization
        void Awake()
        {
            // Remove 'Vuforia AR' navigation mode on non-supported platforms.
            if (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                for (int i = 0; i < m_navigationModeList.Count; ++i)
                {
                    var nm = m_navigationModeList[i];

                    if (nm.name == "CameraNavigationModeVuforiaAR")
                    {
                        m_navigationModeList.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void Start()
        {
            // Disable all modes.
            foreach (var mode in m_navigationModeList)
            {
                mode.enabled = false;
            }

            foreach (var mode in m_translationControlModeList)
            {
                mode.enabled = false;
            }

            foreach (var mode in m_rotationControlModeList)
            {
                mode.enabled = false;
            }

            // Enable navigation mode.
            if (null != m_initialActiveNavigationMode)
            {
                SetActiveNavigationModeByName(m_initialActiveNavigationMode);
            }
            else
            {
                var m = ApplicationSettings.GetInstance().m_data.m_controlSettings.m_navigationMode;

                if (null != m)
                {
                    SetActiveNavigationModeByName(m);
                }
                else
                {
                    SetActiveNavigationMode(0);
                }
            }
            
            // Enable translation mode
            if (null != m_initialActiveTranslationMode)
            {
                SetActiveTranslationControlModeByName(m_initialActiveTranslationMode);
            }
            else
            {
                var m = ApplicationSettings.GetInstance().m_data.m_controlSettings.m_translationInputMode;

                if (null != m)
                {
                    SetActiveTranslationControlModeByName(m);
                }
                else
                {
                    SetActiveTranslationControlMode(0);
                }
            }

            // Enable rotation mode            
            if (null != m_initialActiveRotationMode)
            {
                SetActiveRotationControlModeByName(m_initialActiveRotationMode);
            }
            else
            {
                var m = ApplicationSettings.GetInstance().m_data.m_controlSettings.m_rotationInputMode;
                
                if (null != m)
                {
                    SetActiveRotationControlModeByName(m);
                }
                else
                {
                    SetActiveRotationControlMode(0);
                }
            }
        }

        private void SetActiveTranslationControlMode(int translationModeIndex)
        {
            Debug.Log("SetActiveTranslationControlMode(" + translationModeIndex + ")");

            if (m_activeTranslationControlIndex >= 0)
            {
                m_translationControlModeList[m_activeTranslationControlIndex].enabled = false;
            }

            m_activeTranslationControlIndex = translationModeIndex;

            if (m_activeTranslationControlIndex >= 0)
            {
                m_translationControlModeList[m_activeTranslationControlIndex].enabled = true;
            }
        }

        public void SetActiveTranslationControlModeByName(string name)
        {
            Debug.Log("SetActiveTranslationControlModeByName(" + name + ")");

            int modeIndex = 0;
            foreach (var mode in m_translationControlModeList)
            {
                if (mode.name == name)
                {
                    SetActiveNavigationMode(modeIndex);
                    return;
                }
                ++modeIndex;
            }

            Debug.LogWarning("Unsupported camera translation control mode! (" + name + ")");
        }
        
        private void SetActiveNavigationMode(int modeIndex)
        {
            Debug.Log("SetActiveNavigationMode(" + modeIndex + ")");

            if (m_activeNavigationModeIndex >= 0)
            {
                m_navigationModeList[m_activeNavigationModeIndex].enabled = false;
            }

            m_activeNavigationModeIndex = modeIndex;

            var activeNavigationMode = GetActiveNavigationMode();

            if (activeNavigationMode)
            {
                activeNavigationMode.enabled = true;
            }
        }

        public CameraNavigationModeBase GetActiveNavigationMode()
        {
            return m_activeNavigationModeIndex < 0 ? null : m_navigationModeList[m_activeNavigationModeIndex];
        }

        public void SetActiveNavigationModeByName(string name)
        {
            Debug.Log("SetActiveNavigationModeByName(" + name + ")");

            int modeIndex = 0;
            foreach (var mode in m_navigationModeList)
            {
                if (mode.name == name)
                {
                    SetActiveNavigationMode(modeIndex);
                    return;
                }
                ++modeIndex;
            }

            Debug.LogWarning("Unsupported camera navigation mode! (" + name + ")");
        }

        private void SetActiveRotationControlMode(int modeIndex)
        {
            Debug.Log("SetActiveRotationControlMode(" + modeIndex + ")");

            if (m_activeRotationControlIndex >= 0)
            {
                m_rotationControlModeList[m_activeRotationControlIndex].enabled = false;
            }

            m_activeRotationControlIndex = modeIndex;

            if (m_activeRotationControlIndex >= 0)
            {
                m_rotationControlModeList[m_activeRotationControlIndex].enabled = true;
            }
        }

        public void SetActiveRotationControlModeByName(string name)
        {
            Debug.Log("SetActiveRotationControlModeByName(" + name + ")");
            int modeIndex = 0;
            foreach (var mode in m_rotationControlModeList)
            {
                if (mode.name == name)
                {
                    SetActiveRotationControlMode(modeIndex);
                    return;
                }
                ++modeIndex;
            }

            Debug.LogWarning("Unsupported camera rotation control mode! (" + name + ")");
        }

        public void ActivateNextNavigationMode()
        {
            var nextMode = (m_activeNavigationModeIndex + 1) % m_navigationModeList.Count;

            SetActiveNavigationMode(nextMode);
        }

        public void Update()
        {
            // 'n' key: Toggle navigation mode
            if (Input.GetKeyUp("n"))
            {
                var nm = (m_activeNavigationModeIndex + 1) % m_navigationModeList.Count;

                SetActiveNavigationMode(nm);
            }

            // 'r' key: Toggle rotation mode
            if (Input.GetKeyUp("r"))
            {
                var rcm = (m_activeRotationControlIndex + 1) % m_rotationControlModeList.Count;

                SetActiveRotationControlMode(rcm);
            }

            // 't' key: Toggle translation mode
            if (Input.GetKeyUp("t"))
            {
                var tcm = (m_activeTranslationControlIndex + 1) % m_translationControlModeList.Count;

                SetActiveTranslationControlMode(tcm);
            }

            if (m_activeRotationControlIndex >= 0)
            {
                m_rotationControlModeList[m_activeRotationControlIndex].UpdateRotation(null);
            }

            if (m_activeTranslationControlIndex >= 0)
            {
                m_translationControlModeList[m_activeTranslationControlIndex].UpdateTranslation(null);
            }
        }
    }
}
