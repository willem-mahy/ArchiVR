﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Assets.Scripts.WM.UI;
using Assets.Scripts.WM.UI.VR;
using UnityEngine.EventSystems;

namespace Assets.Scripts.WM
{
    public class ViewProject : MonoBehaviour
    {
        UIManager m_uiManager = null;

        //! The root UI conrol for the VR Overlay menu
        GameObject m_uiControlVROverlay = null;

        // The parent UI control that contains all debug logging UI controls for all types of debugging information.
        GameObject m_canvasDebug = null;

        // List of UI control that one UI control for each debug logging information type.
        List<GameObject> m_uiControlDebug = new List<GameObject>();

        public List<ToggleButton> m_toggleButtonsViewMode = new List<ToggleButton>();

        public Text m_textControlDebugViewMode = null;

        public List<string> m_devices = null;

        public List<GameObject> m_menuTimeArray = null;        

        // Use this for initialization
        void Start()
        {
            // Firs get references to all UI controls, before hiding them by setting them inactive.
            GetReferencesToUiControls();

            // Hide time menus
            foreach (var gameObject in m_menuTimeArray)
            {
                gameObject.SetActive(false);
            }

            // Hide debug logging altogether
            if (m_canvasDebug != null)
            {
                m_canvasDebug.SetActive(false);
            }

            m_uiManager = gameObject.GetComponent<UIManager>();

            // Set the initial active debug logging type to the first one.
            SetActiveDebuggingType(0);

            AddSupportedDevices();

            //SetViewMode(0);
        }

        // Update is called once per frame
        void Update()
        {
            // 'l' key: Show/hide debugging info
            if (Input.GetKeyUp("l"))
            {
                if (m_canvasDebug != null)
                {
                    m_canvasDebug.SetActive(!m_canvasDebug.activeSelf);
                }
            }

            // 'f1' to 'f9' keys: Select active debugging type.
            for (int debugType = 0; debugType < m_uiControlDebug.Count; ++debugType)
            {
                if (Input.GetKeyUp("f" + (debugType + 1)))
                {
                    SetActiveDebuggingType(debugType);
                    break;
                }
            }

            // 'v' key: Toggle View Mode.
            if (Input.GetKeyUp("v"))
            {
                if (m_toggleButtonsViewMode.Count > 0)
                {
                    var b = m_toggleButtonsViewMode[0];

                    b.SetNextOption();

                    SetViewMode(b.GetOption());
                }
            }
        }
        
        //! Add a device to the list of selectable devices if supported by the system.
        void AddDeviceIfSupported(string deviceName)
        {
            foreach (var supportedDeviceName in UnityEngine.XR.XRSettings.supportedDevices)
            {
                if (supportedDeviceName.ToLower().Equals(deviceName.ToLower()))
                {
                    m_devices.Add(deviceName);
                    return;
                }
            }
        }
        
        public void toggleButtonViewMode_OnClick()
        {
            var toggleButtonViewMode = m_toggleButtonsViewMode[0];
            
            toggleButtonViewMode.SetNextOption();
            SetViewMode(toggleButtonViewMode.GetOption());
        }

        public void buttonTime_OnButtonClick(BaseEventData obj)
        {
            if (m_menuTimeArray.Count == 0)
            {
                return; // sanity
            }

            bool show = !m_menuTimeArray[0].activeSelf;

            foreach (var gameObject in m_menuTimeArray)
            {
                gameObject.SetActive(show);
            }
        }

        void GetReferencesToUiControls()
        {
            m_canvasDebug = GameObject.Find("CanvasDebug");

            m_uiControlDebug.Add(GameObject.Find("TextDebugSystemInfo"));
            m_uiControlDebug.Add(GameObject.Find("TextDebugCamera"));
            m_uiControlDebug.Add(GameObject.Find("TextDebugSkyLight1"));
            m_uiControlDebug.Add(GameObject.Find("TextDebugSkyLight2"));
            m_uiControlDebug.Add(GameObject.Find("TextDebugWMTracker"));

            m_uiControlVROverlay = GameObject.Find("VROverlayUI");
        }

        void AddSupportedDevices()
        {
            // Log supported devices
            if (m_textControlDebugViewMode != null)
            {
                m_textControlDebugViewMode.text += "\nSupported VR devices:";

                foreach (var supportedDeviceName in UnityEngine.XR.XRSettings.supportedDevices)
                {
                    m_textControlDebugViewMode.text += "\n -" + supportedDeviceName;
                }
            }

            // No VR
            AddDeviceIfSupported("none");

            // VR
            // Regular split screen H
            AddDeviceIfSupported("stereo");

            // X Eye Split screen H
            AddDeviceIfSupported("split");

            // Oculus And GearVR
            AddDeviceIfSupported("Oculus");

            // Open VR
            //AddDeviceIfSupported("OpenVR");

            // Vive
            //AddDeviceIfSupported("vive");

            // Compose the list of option sprites to initialize the 'View Mode' toggle buttons with.
            List<string> optionSpritePaths = new List<string>();

            foreach (var deviceName in m_devices)
            {
                optionSpritePaths.Add("Menu/ViewMode/" + deviceName);
            }

            // Initialize 'View Mode' toggle buttons.
            foreach (var toggleButtonViewMode in  m_toggleButtonsViewMode)
            {
                toggleButtonViewMode.LoadOptions(optionSpritePaths);

                toggleButtonViewMode.GetComponent<Button>().onClick.AddListener(toggleButtonViewMode_OnClick);
            }
        }

        void SetActiveDebuggingType(int toActivate)
        {
            for (int i = 0; i < m_uiControlDebug.Count; ++i)
            {
                if (m_uiControlDebug[i] == null)
                {
                    continue;
                }

                m_uiControlDebug[i].SetActive(i == toActivate);
            }
        }

        public static bool IsActiveViewModeVR()
        {
            var loadedDeviceName = XRSettings.loadedDeviceName;

            return IsViewModeVR(loadedDeviceName);
        }

        public static bool IsViewModeVR(string deviceName)
        {
            if (deviceName.CompareTo("") == 0)
            {
                return false;
            }

            if (deviceName.ToLower().CompareTo("none") == 0)
            {
                return false;
            }

            return true;
        }

        public void SetViewMode(int viewMode)
        {
            for (int i = 0; i < m_toggleButtonsViewMode.Count; ++i)
            {
                var m_toggleButtonViewMode = m_toggleButtonsViewMode[i];
                m_toggleButtonViewMode.SetOption(viewMode);
            }

            bool invalidIndex = (viewMode < 0 || viewMode >= m_devices.Count);
            string deviceName = invalidIndex ? "" : m_devices[viewMode];

            // update UI visibility
            bool isViewModeVR = IsViewModeVR(deviceName);

            m_uiManager.SetUIMode(isViewModeVR ? UIManager.UIMode.VR : UIManager.UIMode.NonVR);

            if (null != m_textControlDebugViewMode)
            {
                m_textControlDebugViewMode.text += "\nSet ViewMode device:" + deviceName;
            }

            if (deviceName.CompareTo("") == 0)
            {
                DisableVR();
            }
            else
            {
                EnableVR(deviceName);
            }
        }

        IEnumerator LoadDevice(string deviceName, bool enable)
        {
            XRSettings.LoadDeviceByName(deviceName);
            yield return null;
            XRSettings.enabled = enable;
        }

        IEnumerator LoadDevice2(string deviceName, bool enable)
        {
            XRSettings.enabled = enable;
            yield return new WaitForEndOfFrame();
            XRSettings.LoadDeviceByName(deviceName);
            yield return new WaitForEndOfFrame();
            XRSettings.enabled = enable;
        }

        void EnableVR(string deviceName)
        {
            StartCoroutine(LoadDevice2(deviceName, true));
        }

        void DisableVR()
        {
            StartCoroutine(LoadDevice2("", false));
        }
    }
}
