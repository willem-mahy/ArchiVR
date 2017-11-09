using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using Assets.Scripts.WM.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.WM.Util;

namespace Assets.Scripts.WM.ArchiVR.Application
{
    public class ApplicationState : MonoBehaviour
    {
        UIManager m_uiManager = null;

        // The parent UI control that contains all debug logging UI controls for all types of debugging information.
        GameObject m_canvasDebug = null;

        // List of UI control that one UI control for each debug logging information type.
        List<GameObject> m_uiControlDebug = new List<GameObject>();

        public List<ToggleButton> m_toggleButtonsViewMode = new List<ToggleButton>();

        public Text m_textControlDebugViewMode = null;

        public List<string> m_devices = null;

        public List<GameObject> m_menuTimeArray = null;

        // Use this for initialization
        protected virtual void Awake()
        {
            Debug.Log("ApplicationState.Awake()");

            // Get references to all UI controls.
            GetReferencesToUiControls();

            // Get a reference to the UI Manager.
            m_uiManager = gameObject.GetComponent<UIManager>();
        }

        // Use this for initialization
        protected virtual void Start()
        {
            Debug.Log("ApplicationState.Start()");

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

            // Set the initial active debug logging type to the first one.
            SetActiveDebuggingType(0);

            AddSupportedXRDevices();
        }

        // Update is called once per frame
        protected virtual void Update()
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

                    SetViewMode(b.GetSelectedOptionIndex());
                }
            }

            if (Input.GetKeyUp("q"))
            {
                ApplicationSettings.GetInstance().SetGraphicSettingsQualityLevel((QualitySettings.GetQualityLevel() + 1) % QualitySettings.names.Length);
            }
        }
        
        //! Add devices from the given list, to the list of selectable devices, if they are supported by the system.
        void AddSupportedXRDevices(List<string> deviceNames)
        {
            var text = "XRSettings selectable devices:\n";

            foreach (var deviceName in deviceNames)
            {
                foreach (var supportedDeviceName in UnityEngine.XR.XRSettings.supportedDevices)
                {
                    if (supportedDeviceName.ToLower().Equals(deviceName.ToLower()))
                    {
                        text += deviceName;

                        if (deviceName == XRSettings.loadedDeviceName)
                        {
                            text += " (loaded)";
                        }

                        text += "\n";

                        m_devices.Add(deviceName);
                        break;
                    }
                }
            }

            Debug.Log(text);
        }
        
        public void toggleButtonViewMode_OnClick()
        {
            var toggleButtonViewMode = m_toggleButtonsViewMode[0];
            
            toggleButtonViewMode.SetNextOption();
            SetViewMode(toggleButtonViewMode.GetSelectedOptionIndex());
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
        }

        void AddSupportedXRDevices()
        {
            DebugUtil.LogSupportedXRDevices();

            // List supported XR devices in debug window
            if (m_textControlDebugViewMode != null)
            {
                m_textControlDebugViewMode.text += "\nSupported VR devices:";

                foreach (var supportedDeviceName in UnityEngine.XR.XRSettings.supportedDevices)
                {
                    m_textControlDebugViewMode.text += "\n -" + supportedDeviceName;
                }
            }

            List<string> deviceNames = new List<string>();
            deviceNames.Add("none");        // No VR: Regular full-screen mono rendering.
            deviceNames.Add("stereo");      // Regular split screen H
            deviceNames.Add("split");       // X Eye Split screen H
            deviceNames.Add("oculus");      // Oculus And GearVR
            deviceNames.Add("cardboard");   // Google cardboard

            AddSupportedXRDevices(deviceNames); 

            // Compose the list of option sprites to initialize the 'View Mode' toggle buttons with.
            List<string> optionSpritePathList = new List<string>();

            foreach (var deviceName in m_devices)
            {
                optionSpritePathList.Add("Menu/ViewMode/" + deviceName);
            }

            // Initialize 'View Mode' toggle buttons.
            foreach (var toggleButtonViewMode in  m_toggleButtonsViewMode)
            {
                toggleButtonViewMode.LoadOptions(null, optionSpritePathList);

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
                m_toggleButtonViewMode.SelectOptionByIndex(viewMode);
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
            XRSettings.enabled = enable;
            yield return new WaitForEndOfFrame();
            XRSettings.LoadDeviceByName(deviceName);
            yield return new WaitForEndOfFrame();
            XRSettings.enabled = enable;
        }

        void EnableVR(string deviceName)
        {
            Debug.Log("ApplicationState.EnableVR(" + deviceName + ")");
            StartCoroutine(LoadDevice(deviceName, true));
        }

        void DisableVR()
        {
            Debug.Log("ApplicationState.DisableVR()");
            StartCoroutine(LoadDevice("", false));
        }
    }
}
