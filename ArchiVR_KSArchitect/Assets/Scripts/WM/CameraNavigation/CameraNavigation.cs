using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.WM.CameraNavigation.RotationControl;
using Assets.Scripts.WM.CameraNavigation.TranslationControl;

namespace Assets.Scripts.WM.CameraNavigation
{
    public class CameraNavigation : MonoBehaviour
    {
        public Camera m_camera = null;

        public List<RotationControlBase> m_rotationControlModeList = new List<RotationControlBase>();

        public int m_initialActiveRotationModeIndex = -1;

        private int m_activeRotationControlIndex = -1;

        public List<TranslationControlBase> m_translationControlModeList = new List<TranslationControlBase>();

        public int m_initialActiveTranslationModeIndex = -1;

        private int m_activeTranslationControlIndex = -1;

        // Use this for initialization
        void Awake()
        {
            if (null == m_camera)
            {
                Debug.LogWarning("m_camera == null!");
            }
        }

        public void Start()
        {
            foreach (var mode in m_translationControlModeList)
            {
                mode.enabled = false;
            }

            if (m_initialActiveTranslationModeIndex >= 0)
            {
                SetActiveTranslationControlMode(m_initialActiveTranslationModeIndex);
            }
            else
            {
                // TODO:
                /*
                var m = ApplicationSettings.ControlSettings.ActiveTranslationModeIndex;
                
                if (m >= 0)
                {
                    //SetActiveTranslationControlMode(m);
                }
                else
                {
                    //SetActiveTranslationControlMode(0);
                }
                */
            }

            foreach (var mode in m_rotationControlModeList)
            {
                mode.enabled = false;
            }

            if (m_initialActiveRotationModeIndex >= 0)
            {
                SetActiveRotationControlMode(m_initialActiveRotationModeIndex);
            }
            else
            {
                // TODO:
                /*
                var m = ApplicationSettings.ControlSettings.ActiveTranslationModeIndex;
                
                if (m >= 0)
                {
                    //SetActiveRotationControlMode(m);
                }
                else
                {
                    //SetActiveRotationControlMode(0);
                }
                */
            }
        }

        public void SetActiveTranslationControlMode(int translationModeIndex)
        {
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

        public void SetActiveRotationControlMode(int modeIndex)
        {
            if (m_activeRotationControlIndex >= 0)
            {
                m_rotationControlModeList[m_activeRotationControlIndex].enabled = false;
            }

            m_activeRotationControlIndex = modeIndex;

            if (m_activeTranslationControlIndex >= 0)
            {
                m_rotationControlModeList[m_activeRotationControlIndex].enabled = true;
            }
        }

        public void SetActiveRotationControlModeByName(string name)
        {
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

        public void Update()
        {
            // 'r' key: Toggle rotation mode
            if (Input.GetKeyUp("r"))
            {
                var rcm = (m_activeRotationControlIndex + 1) % m_rotationControlModeList.Count;

                SetActiveTranslationControlMode(rcm);
            }

            // 't' key: Toggle translation mode
            if (Input.GetKeyUp("t"))
            {
                var tcm = (m_activeTranslationControlIndex + 1) % m_translationControlModeList.Count;

                SetActiveTranslationControlMode(tcm);
            }

            if (m_activeRotationControlIndex >= 0)
            {
                m_rotationControlModeList[m_activeRotationControlIndex].UpdateRotation(m_camera.gameObject);
            }

            if (m_activeTranslationControlIndex >= 0)
            {
                m_translationControlModeList[m_activeTranslationControlIndex].UpdateTranslation(m_camera.gameObject);
            }
        }
    }
}
