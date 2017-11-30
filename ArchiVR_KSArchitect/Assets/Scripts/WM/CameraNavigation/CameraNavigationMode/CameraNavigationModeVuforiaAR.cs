using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts.WM.CameraNavigation
{
    public class CameraNavigationModeVuforiaAR : CameraNavigationModeBase
    {
        public string m_worldName = "World";
        public float m_rescaleFactor = 0.01f;
        public float m_offsetFromAnchor = 5.0f;

        public GameObject m_vuforia = null;
        public GameObject m_ARCamera = null;
        public GameObject m_imageTarget = null;

        private Transform m_oldWorldParentTransform = null;

        private Camera m_oldMainCamera = null;

        public void Awake()
        {
            Debug.Log("CameraNavigationModeVuforiaAR.Awake()");
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

            if (VuforiaBehaviour.Instance)
            {
                VuforiaBehaviour.Instance.enabled = state;
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
                m_oldWorldParentTransform = m_world.transform.parent;

                UpdateOffsetFromAnchor();

                m_world.transform.parent = m_imageTarget.transform;
                m_world.transform.localScale = m_rescaleFactor * m_world.transform.localScale;
            }

            SetVuforiaActive(true);
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

            // Relocate world to origin as root GO.
            if (null != m_world)
            {
                m_world.transform.position = Vector3.zero;

                m_world.transform.parent = m_oldWorldParentTransform;
                m_world.transform.localScale = 1/ m_rescaleFactor * m_world.transform.localScale;
            }

            m_oldWorldParentTransform = null;

            SetVuforiaActive(false);
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
    }
}