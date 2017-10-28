using System.Collections;
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
        //! The root UI conrol for the overlay menu
        GameObject m_uiControlMenuOverlay = null;

        //! The root UI conrol for the FPS control in the non-VR Overlay menu
        GameObject m_uiControlFPS = null;

        //! The root UI conrol for the gaze menu
        GameObject m_uiControlMenuGaze = null;

        //! The root UI conrol for the VR Overlay menu
        GameObject m_uiControlVROverlay = null;

        // The parent UI control that contains all debug logging UI controls for all types of debugging information.
        GameObject m_canvasDebug = null;

        // List of UI control that one UI control for each debug logging information type.
        List<GameObject> m_uiControlDebug = new List<GameObject>();

        public List<ToggleButton> m_toggleButtonsViewMode = new List<ToggleButton>();

        public Text m_textControlDebugViewMode = null;

        public string[] m_supportedDevices = null;
        public List<string> m_devices = new List<string>();

        private GameObject m_canvasTime = null;

        public List<Button> m_buttonToggleUIArray = new List<Button>();

        // Use this for initialization
        void Start()
        {
            // Firs get references to all UI controls, before hiding them by setting them inactive.
            GetReferencesToUiControls();

            // Hide m_canvasTime
            if (m_canvasTime != null)
            {
                m_canvasTime.SetActive(false);
            }

            // Hide debug logging altogether
            if (m_canvasDebug != null)
            {
                m_canvasDebug.SetActive(false);
            }

            for (int i = 0; i < m_buttonToggleUIArray.Count; ++i)
            {
                var buttonGameObject = m_buttonToggleUIArray[i];
                var buttonComponent = buttonGameObject.GetComponent<Button>();
                buttonComponent.onClick.AddListener(ButtonToggleActiveUI_OnClick);
            }

            // Set the initial active debug logging type to the first one.
            SetActiveDebuggingType(0);
        }

        // Update is called once per frame
        void Update()
        {
            AddSupportedDevices();

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


        public void ButtonToggleActiveUI_OnClick()
        {
            var toggleActiveBehaviorComponent = gameObject.GetComponent<ToggleActiveBehavior>();
            toggleActiveBehaviorComponent.TogglActiveState();
        }

        //! Add a device to he list of selectable devices if supported by the system.
        void AddDeviceIfSupported(string deviceName)
        {
            for (int i = 0; i < m_supportedDevices.Length; ++i)
            {
                if (m_supportedDevices[i].ToLower().Equals(deviceName.ToLower()))
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
            m_canvasTime.SetActive(!m_canvasTime.activeSelf);

            if (m_canvasTime.activeSelf)
            {
                //TODO: GamerObject.find(WMCameraControlDPad).SetActive(false);
            }
        }

        void GetReferencesToUiControls()
        {
            m_canvasDebug = GameObject.Find("CanvasDebug");

            m_uiControlMenuOverlay = GameObject.Find("CanvasOverlayMenu");

            m_uiControlFPS = GameObject.Find("CanvasFPS");

            m_uiControlMenuGaze = GameObject.Find("CanvasGazeMenu");

            m_canvasTime = GameObject.Find("CanvasTime");

            m_uiControlDebug.Add(GameObject.Find("TextDebugSystemInfo"));
            m_uiControlDebug.Add(GameObject.Find("TextDebugCamera"));
            m_uiControlDebug.Add(GameObject.Find("TextDebugSkyLight1"));
            m_uiControlDebug.Add(GameObject.Find("TextDebugSkyLight2"));
            m_uiControlDebug.Add(GameObject.Find("TextDebugWMTracker"));

            m_uiControlVROverlay = GameObject.Find("VROverlayUI");
        }

        void AddSupportedDevices()
        {
            if (null == m_supportedDevices || 0 == m_supportedDevices.Length)
            {
                m_supportedDevices = UnityEngine.XR.XRSettings.supportedDevices;

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

                List<string> optionSpritePaths = new List<string>();

                if (m_textControlDebugViewMode != null)
                {
                    m_textControlDebugViewMode.text += "\nSupported VR devices:";
                }

                for (int i = 0; i < m_devices.Count; ++i)
                {
                    optionSpritePaths.Add("Menu/ViewMode/" + m_devices[i]);

                    if (m_textControlDebugViewMode != null)
                    {

                        m_textControlDebugViewMode.text += "\n -" + m_supportedDevices[i];
                    }
                }

                // Initialize 'View Mode' toggle buttons.
                for (int i = 0; i < m_toggleButtonsViewMode.Count; ++i)
                {
                    var m_toggleButtonViewMode = m_toggleButtonsViewMode[i];
                    m_toggleButtonViewMode.LoadOptions(optionSpritePaths);

                    m_toggleButtonViewMode.GetComponent<Button>().onClick.AddListener(toggleButtonViewMode_OnClick);
                }

                SetViewMode(0);
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

        public void SetViewMode(int viewMode)
        {
            for (int i = 0; i < m_toggleButtonsViewMode.Count; ++i)
            {
                var m_toggleButtonViewMode = m_toggleButtonsViewMode[i];
                m_toggleButtonViewMode.SetOption(viewMode);
            }

            bool invalidIndex = (viewMode < 0 || viewMode >= m_devices.Count);
            string deviceName = invalidIndex ? "" : m_devices[viewMode];

            // update Menu visibility
            if (
                deviceName.CompareTo("") == 0
                || deviceName.ToLower().CompareTo("none") == 0
                )
            {
                m_uiControlMenuOverlay.SetActive(true);
                m_uiControlFPS.SetActive(true);
                m_uiControlMenuGaze.SetActive(false);
                m_uiControlVROverlay.SetActive(false);
            }
            else
            {
                m_uiControlMenuOverlay.SetActive(false);
                m_uiControlFPS.SetActive(false);
                m_uiControlMenuGaze.SetActive(true);
                m_uiControlVROverlay.SetActive(true);

                // While it was disabled, the menu did not update its position in order to be in front of the player.
                // So update it once now just before setting it visible.
                var c = m_uiControlMenuGaze.GetComponent<PlayerGazeMenuBehavior>();

                if (null != c)
                {
                    c.UpdateLocationFromCamera();
                }


            }

            if (null != m_textControlDebugViewMode)
            {
                m_textControlDebugViewMode.text += "\nSet ViewMode device:" + deviceName;
            }

            if (deviceName.CompareTo("") == 0)
            {
                DisableVR();

                //XRSettings.LoadDeviceByName("");
                //XRSettings.enabled = false;
            }
            else
            {
                EnableVR(deviceName);

                //XRSettings.LoadDeviceByName(deviceName);
                //XRSettings.enabled = true;
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
