using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

using Assets.Scripts.WM.Settings;
using Assets.Scripts.WM.UI;
using Assets.Scripts.WM.Util;
using UnityStandardAssets.CrossPlatformInput;
using System;

namespace Assets.Scripts.WM.ArchiVR.Application
{
    public abstract class ApplicationState : MonoBehaviour
    {
        // For debugging purposes.
        static private bool s_debugInitialSetup = true;
        
        // For debugging purposes.
        static public bool s_initialModeForce = true;

        // For debugging purposes.
        static public UIMode s_initialUIMode = UIMode.WorldSpace;

        // For debugging purposes.
        static public string s_initialXRDevice = "split";

        //! Indicates whether or not the application has already performed initial setup during startup.
        static private bool s_initialized = false;

        // puclic: must be settable from Unity Editor
        public Assets.Scripts.WM.UI.TabView m_debugView_SS = null;

        // puclic: must be settable from Unity Editor
        public Assets.Scripts.WM.UI.TabView m_debugView_WS = null;

        // List of names of supported XR devices.
        // This is the list of all possible XR devices that are supported by the current application build, on supporting systems.
        // Names in this list are all lower-case.
        // Shared by all application states.
        private static List<string> s_supportedDeviceNameList = new List<string>();

        // List of names of available XR devices.
        // This is the list of supported XR devices that are supported by the current systems.
        // Names in this list are all lower-case.
        // Shared by all application states.
        private List<string> s_availableDeviceSpritePathList = new List<string>();
        
        // List of paths to the sprites to represent each available XR device.
        // Shared by all application states.
        private static List<string> s_availableDeviceNameList = new List<string>();

        // Use this for initialization
        protected virtual void Awake()
        {
            Debug.Log("ApplicationState.Awake()");
        }

        // Use this for initialization
        protected virtual void Start()
        {
            Debug.Log("ApplicationState.Start()");

            // 1) Perform (if not yet done) initial setup after application startup.
            PerformInitialSetup();

            // 2) Perform initial setup necessary upon each application state entry.

            // 2a) Add the Controls info to the first tab of the debug view.
            var textControlsInfo = GetTextControlsInfo();
            m_debugView_SS.m_tabPanes[0].GetComponent<Text>().text = textControlsInfo;
            m_debugView_WS.m_tabPanes[0].GetComponent<Text>().text = textControlsInfo;

            // 2b) Add the System info to the first tab of the debug view.

            //TODO: put in : AddSystemInfoToDebugView();
            var textSystemInfo = DebugUtil.GetSystemInfoString();
            m_debugView_SS.m_tabPanes[1].GetComponent<Text>().text = textSystemInfo;
            m_debugView_WS.m_tabPanes[1].GetComponent<Text>().text = textSystemInfo;

            // 2c) Make sure that UI Mode is in accordance to the active XR device.
            OnSetActiveXRDevice(XRSettings.loadedDeviceName);

            var widgetDebug = UIManager.GetInstance().GetWidgetByName("WidgetDebug");

            if (null != widgetDebug)
            {
                widgetDebug.SetVisible(false);
            }
        }

        public static string GetTextControlsInfo()
        {
            var textControlsInfo =
            "[Controls]" +
            "\nGeneral" +
            "\n     X: Toggle XR Device" +
            "\n     N: Toggle Navigation mode" +
            "\n     M: Toggle Menu visible" +
            "\n     Q: Toggle graphic quality level" +
            "\nMovement" +
            "\n     Arrows: Move F/B/L/R" +
            "\n     Space: Jump" +
            "\n     PgUp,PgDn: Move Up/Down" +
            "\n     Space: Jump" +
            "\n     Hold Shift: Fast Move Mode" +
            "\n     F11,F12: Previous/Next POI" +
            "\nScene" +
            "\n     C: Toggle construction lights" +
            "\n     L: Show layer menu" +
            "\n     B,S,F: ++Backward/Stop/++Forward animation speed" +
            "\nAdvanced" +
            "\n     L: Toggle Debug window visible" +
            "\n     S: Screen Capture" +
            "\n     P: Export POI" +
            "\n     F1-F10: Show Debug Pane";

            return textControlsInfo;
        }

        private void PerformInitialSetup()
        {
            if (s_initialized)
            {   return; // Already performed initial setup during startup of application.
            }

            // Set the flag to indicate that we already performed initial setup.
            s_initialized = true;

            //DebugUtil::LogJoystickNames();

            // Set the initial active debug logging type to the first one.
            //SetActiveDebuggingType(0);

            // Setup lists of supported and available XR devices.
            SetupXRDevices();

            // Setup initial XR device, UI Mode, Navigation mode, etc...
            var cameraNavigation = GameObject.Find("CameraNavigation");

            if (null == cameraNavigation)
            {
                Debug.LogWarning("GameObject 'CameraNavigation' not found in scene!");
            }

            var cameraNavigationComponent = cameraNavigation.GetComponent<CameraNavigation.CameraNavigation>();

            if (null == cameraNavigationComponent)
            {
                Debug.LogWarning("GameObject 'CameraNavigation' has no 'CameraNavigation' component!");
            }

            if (s_initialModeForce)
            {
                // For debugging purposes, we are forcing an initial UI Mode and XR Device.
                
                // First set the requested UI Mode
                UIManager.GetInstance().SetUIMode(s_initialUIMode);

                // Then activate the requested XR device.
                // This might change the UI mode if incompatible with the XR device.
                SetActiveXRDevice(s_initialXRDevice);
            }
            else
            {
                // Make sure that UI Mode is in accordance to the active XR device.
                OnSetActiveXRDevice(XRSettings.loadedDeviceName);

                bool gamepadIsConnected = (Input.GetJoystickNames().Length > 0);

                // 1) Figure out initial UI mode

                if (XRDevice.isPresent)
                {
                    // When running on a head-mounted XR device, UI mode is always World-space.
                    UIManager.GetInstance().SetUIMode(UIMode.WorldSpace);
                }
                else
                {
                    // When running on a non-head-mounted XR device, UI mode is World-space if gyroscope is present, else Screen-space.
                    UIManager.GetInstance().SetUIMode(SystemInfo.supportsGyroscope ? UIMode.WorldSpace : UIMode.ScreenSpace);
                }               

                // 2) Figure out initial rotation mode

                // If gyroscope is present, use it for rotational navigation.
                //string initialRotationMode = (SystemInfo.supportsGyroscope ? "RotationControlMouse" : "RotationControlGyro");
                
                //cameraNavigationComponent.SetActiveRotationControlModeByName(initialRotationMode);

                // 2) Figure out initial rotation mode
                
            }

#if UNITY_EDITOR

            // When running in the Unity Editor (Debugging), enable mouse and KB input.
            //CrossPlatformInputManager.SwitchActiveInputMethod(CrossPlatformInputManager.ActiveInputMethod.Hardware);

#endif
        }

        static bool ActiveXRDevice_SupportsUIMode(UIMode newMode)
        {
            if ((UIMode.ScreenSpace == newMode) && XRDevice.isPresent)
            {
                // Head-mounted dedicated devices do not support Screen-space UI.
                return false;
            }

            return true;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            var camera = Camera.main;

            var cameraText =
                "[Camera]\n" +
                "Pos:" + camera.transform.position.ToString() + "\n" +
                "Fwd:" + camera.transform.forward.ToString() + "\n" +
                "Up:" + camera.transform.up.ToString() + "\n";
            m_debugView_SS.m_tabPanes[2].GetComponent<Text>().text = cameraText;
            m_debugView_WS.m_tabPanes[2].GetComponent<Text>().text = cameraText;

            // 'l' key: Show/hide debugging info
            if (Input.GetKeyUp("l"))
            {
                var wd = UIManager.GetInstance().GetWidgetByName("WidgetDebug");

                if (null != wd)
                {
                    wd.ToggleVisible();
                }
            }

            // 'c' key: Generate ScreenCapture.
            if (Input.GetKeyUp("c"))
            {
                MakeScreenCapture();
            }

            // 'f1' to 'f9' keys: Select active debugging type.
            for (int debugType = 0; debugType < m_debugView_SS.GetNumTabs(); ++debugType)
            {
                if (Input.GetKeyUp("f" + (debugType + 1)))
                {
                    m_debugView_SS.SetActiveTab(debugType);
                    m_debugView_WS.SetActiveTab(debugType);
                    break;
                }
            }

            if (Input.GetKeyDown("x"))
            {
                var prev = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

                if (prev)
                {
                    ActivatePreviousXRDevice();
                }
                else
                {
                    ActivateNextXRDevice();
                }
            }

            // 'q' key: Toggle Quality Level.
            if (Input.GetKeyUp("q"))
            {
                ApplicationSettings.GetInstance().SetGraphicSettingsQualityLevel((QualitySettings.GetQualityLevel() + 1) % QualitySettings.names.Length);
            }

            // 'u' key: Toggle world/screen space UI.
            if (Input.GetKeyUp("u"))
            {
                var s = ApplicationSettings.GetInstance().m_data.m_stateSettings;

                var newMode = (UIMode.ScreenSpace == s.m_uiMode)
                                ? UIMode.WorldSpace
                                : UIMode.ScreenSpace;

                if (ActiveXRDevice_SupportsUIMode(newMode))
                {
                    s.m_uiMode = newMode;
                    UIManager.GetInstance().SetUIMode(newMode);
                }
            }

            UpdateVirtualGamepadActiveState();
        }        

        protected abstract string GetName();        

        public void MenuSettingsButton_OnClick()
        {
            Debug.Log("ApplicationState.MenuSettingsButton_OnClick()");

            //UIManager.GetInstance().OpenMenu("MenuSettings");
            UIManager.GetInstance().OpenMenu(GameObject.Find("MenuSettings").GetComponent<Assets.Scripts.WM.UI.Menu>());
        }

        public void MenuGraphicsSettingsButton_OnClick()
        {
            Debug.Log("ApplicationState.MenuGraphicsSettingsButton_OnClick()");

            UIManager.GetInstance().OpenMenu(GameObject.Find("MenuGraphicsSettings").GetComponent<Assets.Scripts.WM.UI.Menu>());
        }

        public void MenuControlsSettingsButton_OnClick()
        {
            Debug.Log("ApplicationState.MenuControlsSettingsButton_OnClick()");

            UIManager.GetInstance().OpenMenu(GameObject.Find("MenuControlsSettings").GetComponent<Assets.Scripts.WM.UI.Menu>());
        }

        public void ToggleUIButton_OnClick()
        {
            Debug.Log("ApplicationState.ToggleUIButton_OnClick()");

            UIManager.GetInstance().ToggleUIVisible();
        }

        public void MenuClose_OnClick()
        {
            Debug.Log("ApplicationState.MenuClose_OnClick()");

            UIManager.GetInstance().CloseMenu();
        }

        //! Quit the application.
        protected void QuitApplication()
        {
            Debug.Log("ApplicationState.QuitApplication");

            ApplicationSettings.GetInstance().Save();

            UnityEngine.Application.Quit();
        }
        
        public void ButtonXRDevice_OnClick()
        {
            ActivateNextXRDevice();
        }

        public int GetActiveXRDeviceIndex()
        {
            var activeDeviceName = XRSettings.loadedDeviceName;

            if ("" == activeDeviceName)
            {
                activeDeviceName = "none";
            }

            for (int i = 0; i < s_availableDeviceNameList.Count; ++i)
            {
                var availableDeviceName = s_availableDeviceNameList[i];                

                if (availableDeviceName == activeDeviceName)
                {
                    return i;
                }
            }

            return -1;
        }

        public List<string> GetAvailableXRDeviceNameList()
        {
            return s_availableDeviceNameList;
        }

        public void ActivatePreviousXRDevice()
        {
            if (s_availableDeviceNameList.Count == 1)
            {
                return;
            }

            int current = GetActiveXRDeviceIndex();
            int next = (current - 1);

            if (next < 0)
                next = s_availableDeviceNameList.Count - 1;

            SetActiveXRDevice(s_availableDeviceNameList[next]);
        }

        public void ActivateNextXRDevice()
        {
            if (s_availableDeviceNameList.Count == 1)
            {
                return;
            }

            int current = GetActiveXRDeviceIndex();
            int next = (current + 1) % s_availableDeviceNameList.Count;

            SetActiveXRDevice(s_availableDeviceNameList[next]);
        }

        /*! Setup the list of supported devices.
         *  A supported device is a XR device that is supported by the application on compatible systems,
         *  but not necessarily on the current system.
         */
        private void SetupSupportedXRDeviceList()
        {
            bool loadDynamic = true;

            if (loadDynamic)
            {
                foreach (var deviceName in UnityEngine.XR.XRSettings.supportedDevices)
                {
                    var dn = deviceName.ToLower();

                    // Detect duplicates which are for some reason present in XRSettings.supportedDevices.
                    bool isDuplicate = false;
                    foreach (var alreadyAddedSupportedDeviceName in s_supportedDeviceNameList)
                    {
                        if (alreadyAddedSupportedDeviceName.CompareTo(dn) == 0)
                        {
                            isDuplicate = true;
                            break;
                        }
                    }

                    if (!isDuplicate)
                    {
                        s_supportedDeviceNameList.Add(dn);
                    }
                }
            }
            else
            {
                s_supportedDeviceNameList.Add("none");        // No VR: Regular full-screen mono rendering.
                s_supportedDeviceNameList.Add("stereo");      // Regular split screen H
                s_supportedDeviceNameList.Add("split");       // X Eye Split screen H
                s_supportedDeviceNameList.Add("oculus");      // Oculus And GearVR
                s_supportedDeviceNameList.Add("cardboard");   // Google cardboard
            }

            if (s_debugInitialSetup)
            {
                var text = "Supported XR devices:";

                foreach (var deviceName in s_supportedDeviceNameList)
                {
                    text += "\n- " + deviceName;

                    if (deviceName == XRSettings.loadedDeviceName.ToLower())
                    {
                        text += " (loaded)";
                    }
                }

                Debug.Log(text);
            }
        }

        /*! Setup the list of available devices.
         *  Available devices are supported (by the application) devices that are also supported by the current system.
         *
         * \pre 'SetupSupportedXRDeviceList' has been executed priorly to setup the list of supported XR devices.
         */
        void SetupAvailableXRDeviceList()
        {
            if (UnityEngine.XR.XRDevice.isPresent)
            {
                var loadedXRDeviceName = XRSettings.loadedDeviceName.ToLower();

                if (!AllowSetActiveXRDevice(loadedXRDeviceName))
                {
                    s_availableDeviceNameList.Add(loadedXRDeviceName);
                    return;
                }
            }

            foreach (var deviceName in s_supportedDeviceNameList)
            {
                if (!AllowSetActiveXRDevice(deviceName))
                {
                    continue;  // Switching to/from this supported XR device at runtime is not supported.
                }

                s_availableDeviceNameList.Add(deviceName);
            }
            
            // Compose the list of option sprites to initialize the 'View Mode' toggle buttons with.
            foreach (var deviceName in s_availableDeviceNameList)
            {
                s_availableDeviceSpritePathList.Add("Menu/ViewMode/" + deviceName);
            }

            if (s_debugInitialSetup)
            {
                var text = "Available XR devices:";

                foreach (var deviceName in s_availableDeviceNameList)
                {
                    text += "\n- " + deviceName;

                    if (deviceName == XRSettings.loadedDeviceName)
                    {
                        text += " (loaded)";
                    }
                }

                Debug.Log(text);
            }
        }

        //! Setup the supported and available XR device lists.
        void SetupXRDevices()
        {
            SetupSupportedXRDeviceList();
            SetupAvailableXRDeviceList();
        }

        void SetActiveDebuggingType(int toActivate)
        {
            m_debugView_SS.SetActiveTab(toActivate);
            m_debugView_WS.SetActiveTab(toActivate);
        }

        public static bool IsActiveXRDeviceStereoscopic()
        {
            return IsXRDeviceStereoscopic(XRSettings.loadedDeviceName.ToLower());
        }

        public static bool IsXRDeviceStereoscopic(string deviceName)
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

        public void SetActiveXRDeviceByIndex(int viewMode)
        {
            bool invalidIndex = (viewMode < 0 || viewMode >= s_availableDeviceNameList.Count);
            string deviceName = invalidIndex ? "" : s_availableDeviceNameList[viewMode];

            SetActiveXRDevice(deviceName);
        }

        /*! Query whether switching to/from the given XR device at runtime is supported.
         *
         * \param[in] deviceName    The XR device name.
         */
        static public bool AllowSetActiveXRDevice(string deviceNameIn)
        {
            var deviceName = deviceNameIn.ToLower();

            // Cannot switch to/from Oculus from/to other XR device.
            if (deviceName.CompareTo("oculus") == 0)
            {
                return false;
            }

            return true;
        }

        public void SetActiveXRDevice(string deviceName)
        {
            // Sanity check: Can only switch to available XR devices.
            //TODO

            // Sanity check: Cannot switch from Oculus to other XR device.
            if (XRSettings.loadedDeviceName.ToLower().CompareTo("oculus") == 0)
            {
                return;
            }

            DoSetActiveXRDevice(deviceName);

            OnSetActiveXRDevice(deviceName);
        }

        private void OnSetActiveXRDevice(string deviceName)
        {
            // Force UI mode to world-space UI mode, when setting a stereoscopic XRDevice.
            if (IsXRDeviceStereoscopic(deviceName))
            {
                UIManager.GetInstance().SetUIMode(UIMode.WorldSpace);
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

        void DoSetActiveXRDevice(string deviceName)
        {
            Debug.Log("ApplicationState.SetActiveXRDevice(" + deviceName + ")");

            bool enableXR = (deviceName != null) && (deviceName != "");

            StartCoroutine(LoadDevice(deviceName, enableXR));
        }

        private void UpdateVirtualGamepadActiveState()
        {
            var virtualGamepad = GameObject.Find("Panel_WidgetControlCameraFlyDPad");

            if (null == virtualGamepad)
            {
                return;
            }

            var cameraNavigation = GameObject.Find("CameraNavigation").GetComponent<CameraNavigation.CameraNavigation>();

            bool isUIModeWorldSpace = UIManager.GetInstance().GetUIMode() == UIMode.WorldSpace;
            bool currentCameraNavigationSupportsGamepad = (null == cameraNavigation) ? true : cameraNavigation.GetActiveNavigationMode().SupportsDPadInput();
            bool isPhysicalGamePadConnected = (Input.GetJoystickNames().Length > 0);

            virtualGamepad.SetActive(!isUIModeWorldSpace && !isPhysicalGamePadConnected && currentCameraNavigationSupportsGamepad);
        }

        static public void OpenProject(string sceneName)
        {
            Debug.Log("OpenProject(" + sceneName + ")");

            ApplicationSettings.GetInstance().m_data.m_stateSettings.m_activeProjectName = sceneName;

            OpenProject();
        }

        static public void OpenProject()
        {
            Debug.Log("OpenProject()");

            var projectName = ApplicationSettings.GetInstance().m_data.m_stateSettings.m_activeProjectName;
            SceneManager.LoadScene(projectName);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene("Play", LoadSceneMode.Additive);
        }

        static private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            var projectName = ApplicationSettings.GetInstance().m_data.m_stateSettings.m_activeProjectName;

            var sp = SceneManager.GetSceneByName(projectName);

            if (!sp.IsValid())
                return;

            if (!sp.isLoaded)
                return;

            var svp = SceneManager.GetSceneByName("Play");

            if (!svp.IsValid())
                return;

            if (svp.isLoaded == false)
                return;

            var gameObjects = sp.GetRootGameObjects();

            var gameObjectWorld = gameObjects[0];

            if (gameObjectWorld)
            {
                SceneManager.MoveGameObjectToScene(gameObjectWorld, svp);
                SceneManager.SetActiveScene(svp);
                SceneManager.UnloadSceneAsync(sp);
            }

            LayerManager.GetInstance().DynamicallyCreateLayers();
        }

        public void MakeScreenCapture()
        {
            StartCoroutine(DoMakeScreenCapture());
        }

        IEnumerator DoMakeScreenCapture()
        {
            string time = Time.time.ToString();
            string path =
                "ScreenCapture/"
                + GetName()
                + "_" + ApplicationSettings.GetInstance().m_data.m_stateSettings.m_activeProjectName
                + "_" + time
                + ".png";

            var reticle = GameObject.Find("CanvasReticle");

            if (null != reticle)
            {
                reticle.SetActive(false);
                yield return new WaitForEndOfFrame();
            }

            ScreenCapture.CaptureScreenshot(path);
            

            if (null != reticle)
            {
                yield return new WaitForEndOfFrame();
                reticle.SetActive(true);
            }
        }
    }
}
