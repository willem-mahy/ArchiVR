using GoogleARCore;
using KS.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WM.UI;

namespace ArchiAR_ARCore_AR
{
#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class ApplicationStateDefault : MonoBehaviour
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;        

        public List<float> m_modelScales = new List<float>()
        {
            (1.0f /50.0f),
            (1.0f /25.0f),
            (1.0f /5.0f),
            (1.0f /2.0f),
            1.0f
        };

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        public MenuLayer m_menuLayer = null;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;

        public Text m_textProjectName = null;

        public Text m_textLastInputEvent = null;

        public Text m_textModelScale = null;

        public Text m_textModelPosition = null;

        public Text m_textModelRotation = null;

        public GameObject m_menu = null;

        public GameObject m_panelModelEditControls = null;

        public GameObject m_panelDebugInfo = null;

        private int m_modelScaleIndex = 0;

        public ProjectManager m_projectManager = null;         

        private GameObject m_model = null;

        private bool m_loadingProject = false;

        /// <summary>
        /// The rotation in degrees need to apply to model when it is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame.
        /// This object is used across the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        private Anchor m_modelAnchor = null;

        private float m_defaultNearClipPlane = 0.1f;

        public void Start()
        {
            m_defaultNearClipPlane = Camera.main.nearClipPlane;
        }

        public void ToggleMenuVisible()
        {
            if (!m_menu)
                return;

            m_menu.SetActive(!m_menu.activeSelf);
        }

        public void TogglePanelModelEditControlsVisible()
        {
            if (!m_panelModelEditControls)
                return;

            m_panelModelEditControls.SetActive(!m_panelModelEditControls.activeSelf);
        }

        public void ToggleShowDebugInfo_ValueChanged(Toggle toggle)
        {
            if (!m_panelDebugInfo)
                return;

            m_panelDebugInfo.SetActive(toggle.isOn);
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            _UpdateApplicationLifecycle();

            // Hide snackbar when currently tracking at least one plane.
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            bool showSearchingUI = true;
            for (int i = 0; i < m_AllPlanes.Count; i++)
            {
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                {
                    showSearchingUI = false;
                    break;
                }
            }

            SearchingForPlaneUI.SetActive(showSearchingUI);

            // If the player has touched the screen, we 'try to add'/'remove' the model.
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (m_textLastInputEvent)
                        m_textLastInputEvent.text = "Touch";

                    if (m_model == null)
                    {
                        AddModel(touch.position);
                    }
                }
            }            
           
            if (m_textModelPosition)
            {
                var ss = m_model ? m_model.transform.position.ToString() : "-";
                m_textModelPosition.text = ss;
            }

            if (m_textModelRotation)
            {
                var rs = m_model ? m_model.transform.rotation.eulerAngles.ToString() : "-";
                m_textModelRotation.text = rs;
            }

            if (m_textModelScale)
            {
                var ss = m_model ? m_model.transform.localScale.x.ToString() : "-";
                m_textModelScale.text = ss;
            }

            if (!m_loadingProject)
                if (m_textProjectName)
                {
                    var activeProject = m_projectManager.GetActiveProject();
                    m_textProjectName.text = activeProject ==null ? "" : activeProject.m_name;
                }
        }

        private void AddModel(Vector2 position)
        {
            if (m_loadingProject)
                return;

#if UNITY_EDITOR
            // Instantiate model at the hit pose.
            SetModel(Vector3.zero, Quaternion.identity, k_ModelRotation);
            return;
#endif

            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter =
                TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (GoogleARCore.Frame.Raycast(position.x, position.y, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    this._ShowAndroidToastMessage("Hit at back of the current DetectedPlane");
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    // Create an anchor to allow ARCore to track the hitpoint as understanding 
                    // of the physical world evolves.
                    m_modelAnchor = hit.Trackable.CreateAnchor(hit.Pose);

                    // Instantiate model at the hit pose.
                    SetModel(hit.Pose.position, hit.Pose.rotation, k_ModelRotation);
                }
            }
        }

        public void NextProject()
        {
            if (m_loadingProject)
                return;

            m_projectManager.NextProject();

            UpdateModel();
        }

        public void PreviousProject()
        {
            if (m_loadingProject)
                return;

            m_projectManager.PreviousProject();

            UpdateModel();
        }

        private void UpdateModel()
        {
            if (!m_model)
            {
                return;
            }

            if (!m_modelAnchor)
            {
                return;
            }

            // Get location from current model.
            var position = m_model.transform.position;
            var rotation = m_model.transform.rotation;

            // Remove current model
            Destroy(m_model);
            m_model = null;

            SetModel(position, rotation, 0);
        }

        private void SetModel(Vector3 position, Quaternion rotation, float rotationY)
        {
            var activeProject = m_projectManager.GetActiveProject();

            if (activeProject == null)
                return;

            StartCoroutine(LoadModelAsync(activeProject.m_scenePath, position, rotation, rotationY));
        }

        IEnumerator LoadModelAsync(String sceneName, Vector3 position, Quaternion rotation, float rotationY)
        {
            m_loadingProject = true;

            if (m_textProjectName)
                m_textProjectName.text = "Loading...";

            // The Application loads the Scene in the background as the current Scene runs.
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            m_model = GameObject.Find("World");            

            if (m_model)
            {
                // Disable all lights in the model.
                var transformLighting = m_model.transform.Find("Construction/Phases/Final/Lighting");
                if (transformLighting)
                {
                    var gameObjectLighting = transformLighting.gameObject;

                    if (gameObjectLighting)
                    {
                        gameObjectLighting.SetActive(false);
                    }
                }

                // Instantiate model at the stored location.
                m_model.transform.position = position;

                m_model.transform.rotation = rotation;

                // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                m_model.transform.Rotate(0, rotationY, 0, Space.Self);

#if UNITY_EDITOR
#else
                // Make model a child of the model anchor.
                m_model.transform.parent = m_modelAnchor.transform;
#endif
            }

            var layerManager = gameObject.GetComponent<LayerManager>();

            layerManager.DynamicallyCreateLayers();

            m_menuLayer.Init(layerManager);

            m_loadingProject = false;
        }

        public void ModelScaleDown()
        {
            SetModelScale(Mathf.Max(m_modelScaleIndex - 1, 0));
        }

        public void ModelScaleUp()
        {
            SetModelScale(Mathf.Min(++m_modelScaleIndex, (m_modelScales.Count - 1)));
        }

        private void SetModelScale(int modelScaleIndex)
        {
            if (m_loadingProject)
                return;

            if (!m_model)
                return;

            m_modelScaleIndex = modelScaleIndex;

            float s = m_modelScales[m_modelScaleIndex];
            m_model.transform.localScale = new Vector3(s, s, s);

            UpdateCameraClipPlanes();
        }

        // Adjust near clipping plane.
        // This prevents the model from becoming clipped (partially or entirely)
        // when viewing the model at smaller scales from close up.
        private void UpdateCameraClipPlanes()
        {
            float s = m_modelScales[m_modelScaleIndex];

            Camera.main.nearClipPlane = 0.1f;// m_defaultNearClipPlane * s;
        }

        public void ModelRotationDown()
        {
            if (m_loadingProject)
                return;

            if (!m_model)
                return;

            m_model.transform.Rotate(0, -30, 0);
        }

        public void ModelRotationUp()
        {
            if (m_loadingProject)
                return;

            if (!m_model)
                return;

            m_model.transform.Rotate(0, 30, 0);
        }


        public void ModelPositionDown()
        {
            if (m_loadingProject)
                return;

            if (!m_model)
                return;

            var pos = m_model.transform.localPosition - 0.1f * Vector3.up;
            m_model.transform.localPosition = pos;
        }

        public void ModelPositionUp()
        {
            if (m_loadingProject)
                return;

            if (!m_model)
                return;

            var pos = m_model.transform.localPosition + 0.1f * Vector3.up;
            m_model.transform.localPosition = pos;
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        public void RemoveModel()
        {
            if (m_loadingProject)
                return;

            if (m_model)
            {
                Destroy(m_model);
                m_model = null;
            }

            if (m_modelAnchor)
            {
                Destroy(m_modelAnchor);
                m_modelAnchor = null;
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
