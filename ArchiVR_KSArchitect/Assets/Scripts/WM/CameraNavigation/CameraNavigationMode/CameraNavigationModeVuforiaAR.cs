using Assets.Scripts.WM.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts.WM.CameraNavigation
{
    public class CameraNavigationModeVuforiaAR : CameraNavigationModeBase
    {
        //
        public string m_worldName = "World";

        //
        public float m_rescaleFactor = 0.01f;

        // The offset distance along the Y axis, that the world model is attached to the marker by.
        public float m_offsetFromAnchor = 5.0f;

        //
        public GameObject m_vuforia = null;

        //
        public GameObject m_ARCamera = null;

        //
        public GameObject m_imageTarget = null;

        //! The parent transform for the World GO, when not in Vuforia AR state.
        private Transform m_oldWorldParentTransform = null;

        public void Awake()
        {
            Debug.Log("CameraNavigationModeVuforiaAR.Awake()");
        }

        static public void enableAll(GameObject root)
        {
            for (int i = 0; i < root.transform.childCount; ++i)
            {
                var go = root.transform.GetChild(i).gameObject;

                var meshComponent = go.GetComponent<MeshRenderer>();

                if (meshComponent)
                    meshComponent.enabled = true;

                var colliderComponent = go.GetComponent<Collider>();

                if (colliderComponent)
                    colliderComponent.enabled = true;

                enableAll(go);
            }
        }

        private void SetVuforiaActive(bool state)
        {
            if (state)
            {
                if (!VuforiaRuntime.Instance.HasInitialized)
                {
                    VuforiaRuntime.Instance.InitVuforia();
                }
            }

            if (null != m_vuforia)
            {
                m_vuforia.SetActive(state);
            }

            var vuforia = VuforiaBehaviour.Instance;

            if (vuforia)
            {
                vuforia.enabled = state;
            }
            else
            {
                if (state)
                {
                    Debug.LogWarning("Trying to enable Vuforia failed! (VuforiaBehaviour.Instance == null)");
                }
            }

            //ImageTracker.enabled = false;

            VuforiaConfiguration.Instance.VideoBackground.VideoBackgroundEnabled = state;
        }

        private GameObject m_canvasReticle = null;
        private GameObject m_world = null;

        override public void OnEnable()
        {
            Debug.Log("CameraNavigationModeVuforiaAR.OnEnable()");

            // Disable Recticle
            m_canvasReticle = GameObject.Find("FPSController/FirstPersonCharacter/CanvasReticle");

            if (null != m_canvasReticle)
                m_canvasReticle.SetActive(false);

            // Relocate world to AR anchor position.
            m_world = GameObject.Find(m_worldName);

            if (null != m_world)
            {
                // Store the parent transform for the World GO, when not in Vuforia AR state.
                m_oldWorldParentTransform = m_world.transform.parent;

                UpdateOffsetFromAnchor();

                m_world.transform.parent = m_imageTarget.transform;
                m_world.transform.localScale = m_rescaleFactor * m_world.transform.localScale;
            }

            SetVuforiaActive(true);

            // Until world-scale UI is supported properly in AR mode,
            // we forcibly and automatically switch UI mode to Screen space.
            UIManager.GetInstance().SetUIMode(UIMode.ScreenSpace);
        }

        public void UpdateOffsetFromAnchor()
        {
            var anchor = GameObject.Find(m_worldName + "/AR_Anchor");

            if (null == anchor)
            {
                return;
            }
            
            // Attach to the anchor...
            m_world.transform.localPosition = -(m_rescaleFactor * anchor.transform.localPosition);
            
            // ... at a fixed offset along world +Y.
            m_world.transform.localPosition += m_rescaleFactor * m_offsetFromAnchor * Vector3.up;
        }

        override public void OnDisable()
        {
            Debug.Log("CameraNavigationModeVuforiaAR.OnDisable()");

            // Enable Recticle
            if (null != m_canvasReticle)
                m_canvasReticle.SetActive(true);
                        
            if (null != m_world)
            {
                // Restore the parent transform for the World GO,
                // when not in Vuforia AR state.
                m_world.transform.SetParent(m_oldWorldParentTransform, false);
                m_oldWorldParentTransform = null;

                // Then restore the World GO local position, rotation,and scale to their defaults.
                m_world.transform.localScale = Vector3.one;
                m_world.transform.localPosition = Vector3.zero;
                m_world.transform.localRotation = Quaternion.identity;
            }           

            SetVuforiaActive(false);

            // Vuforia disables mesh renderers and colliders so re-enable.
            enableAll(m_world);
        }

        // Use this for initialization
        void Start()
        {            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void PositionCamera(Vector3 translation, Quaternion rotation)
        {
            // NOOP: ALready taken care of by Vuforia, by moving detected markers.
        }

        public override bool SupportsDPadInput()
        {
            return false;
        }

        public override bool SupportsNavigationViaPOI()
        {
            return false;
        }
    }
}