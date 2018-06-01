//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.UI;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;    
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class HelloARController : MonoBehaviour
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        public GameObject m_model = null;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject ModelPrefab;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;

        public Text m_textCameraPosition = null;

        public Text m_textCameraRotation = null;

        public Text m_textNumTaps = null;

        public Text m_textLastInputEvent = null;

        public Text m_textModelScale = null;

        public Text m_textModelPosition = null;

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

        private int m_numTaps = 0;

        public Anchor m_modelAnchor = null;

       public void Start()
        {
            OVRTouchpad.Create();
            OVRTouchpad.TouchHandler += HandleTouchHandler;
        }

        bool m_modelPlaceRemove = false;

        void HandleTouchHandler(object sender, System.EventArgs e)
        {
            OVRTouchpad.TouchArgs touchArgs = (OVRTouchpad.TouchArgs)e;
            OVRTouchpad.TouchEvent touchEvent = touchArgs.TouchType;
            /*if(touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap)
            {
                //TODO: Insert code here to handle a single tap.  Note that there are other TouchTypes you can check for like directional swipes, but double tap is not currently implemented I believe.
            }*/

            switch (touchEvent)
            {
                case OVRTouchpad.TouchEvent.SingleTap:
                    Screen.SetResolution(2560, 1440, true);

                    if (m_textNumTaps)
                        m_textNumTaps.text = (++m_numTaps).ToString();

                    if (m_textLastInputEvent)
                        m_textLastInputEvent.text = "SingleTap";
                    m_modelPlaceRemove = true;
                    break;

                case OVRTouchpad.TouchEvent.Left:
                    if (m_textLastInputEvent)
                        m_textLastInputEvent.text = "Left";
                    if (m_model)
                    {
                        m_model.SetActive(!m_model.activeSelf);
                    }
                    break;

                case OVRTouchpad.TouchEvent.Right:
                    if (m_textLastInputEvent)
                        m_textLastInputEvent.text = "Right";
                    if (m_model)
                    {
                        m_model.SetActive(!m_model.activeSelf);
                        //TODO: instead: NextModel();
                    }

                    break;

                case OVRTouchpad.TouchEvent.Up:
                    if (m_textLastInputEvent)
                        m_textLastInputEvent.text = "Up";
                    ToggleModelScaleUp();
                    break;

                case OVRTouchpad.TouchEvent.Down:
                    if (m_textLastInputEvent)
                        m_textLastInputEvent.text = "Down";
                    ToggleModelScaleDown();
                    break;
            }
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
            
            if (m_modelPlaceRemove)
            {
                m_modelPlaceRemove = false;
                                
                if (m_model == null)
                {
                    AddModel();
                }
                else
                {
                    RemoveModel();
                }
            }

            if (false)
            {
                var rayColor = (m_modelPlaceRemove ? Color.green : Color.red);
                DrawDebugRayCamera(rayColor);
            }

            if (m_textCameraPosition)
            {
                m_textCameraPosition.text = Camera.current ? Camera.current.transform.position.ToString() : "-";
            }

            if (m_textCameraRotation)
            {
                m_textCameraRotation.text = Camera.current ? Camera.current.transform.rotation.eulerAngles.ToString() : "-";
            }

            if (m_textModelPosition)
            {
                var ss = m_model ? m_model.transform.position.ToString() : "-";
                m_textModelPosition.text = ss;
            }

            if (m_textModelScale)
            {
                var ss = m_model ? m_model.transform.localScale.x.ToString() : "-";
                m_textModelScale.text = ss;
            }
        }

        private void AddModel()
        {
            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter =
                TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            Vector2 GearVRSizeOfView = new Vector2(1280, 720);
            Vector2 GearVRCenterOfView = GearVRSizeOfView * 0.5f;

            if (Frame.Raycast(GearVRCenterOfView.x, GearVRCenterOfView.y, raycastFilter, out hit))
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    // Instantiate Andy model at the hit pose.
                    m_model = Instantiate(ModelPrefab, hit.Pose.position, hit.Pose.rotation);

                    // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                    m_model.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    m_modelAnchor = hit.Trackable.CreateAnchor(hit.Pose);

                    // Make model a child of the anchor.
                    m_model.transform.parent = m_modelAnchor.transform;
                }                
            }
            else
            {
                // Instantiate Andy model at world origin.
                m_model = Instantiate(ModelPrefab, Vector3.zero, Quaternion.identity);
            }
        }

        private void DrawDebugRayCamera(Color rayColor)
        {
            if (Camera.current == null)
                return;

            var transform = Camera.current.transform;
            Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
            Debug.DrawRay(transform.position, forward, rayColor, 2, true);
        }

        private int m_modelScaleIndex = 0;

        private List<float> m_modelScales = new List<float>()
        {
            (1.0f /50.0f),
            (1.0f /25.0f),
            (1.0f /5.0f),
            (1.0f /2.0f),
            1.0f
        };

        private void ToggleModelScaleDown()
        {
            m_modelScaleIndex = Mathf.Max(--m_modelScaleIndex, 0);
            float s = m_modelScales[m_modelScaleIndex];
            m_model.transform.localScale = new Vector3(s, s, s);            
        }

        private void ToggleModelScaleUp()
        {            
            m_modelScaleIndex = Mathf.Min(++m_modelScaleIndex, (m_modelScales.Count-1));
            float s = m_modelScales[m_modelScaleIndex];
            m_model.transform.localScale = new Vector3(s, s, s);
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

        private void RemoveModel()
        {
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
