using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace Assets.Scripts.WM.CameraNavigation
{
    public class CameraNavigationModeVuforiaAR : CameraNavigationModeBase
    {
        public string m_worldName = "World_Test";
        public float m_rescaleFactor = 0.01f;
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

            VuforiaBehaviour.Instance.enabled = state;
            //ImageTracker.enabled = false;

            VuforiaConfiguration.Instance.VideoBackground.VideoBackgroundEnabled = state;
        }

        override public void OnEnable()
        {
            Debug.Log("CameraNavigationModeVuforiaAR.OnEnable()");

            // Disable Recticle
            var canvasReticle = GameObject.Find("FPSController/FirstPersonCharacter/CanvasReticle");
            canvasReticle.SetActive(false);

            // Relocate world to AR anchor position.
            var world = GameObject.Find(m_worldName);

            if (null != world)
            {
                m_oldWorldParentTransform = world.transform.parent;

                var anchor = GameObject.Find("World/Anchor_AR");

                if (null != anchor)
                {
                    world.transform.position = -anchor.transform.localPosition;
                }
                world.transform.parent = m_imageTarget.transform;
                world.transform.localScale = m_rescaleFactor * world.transform.localScale;
            }

            SetVuforiaActive(true);
        }

        override public void OnDisable()
        {
            Debug.Log("CameraNavigationModeVuforiaAR.OnDisable()");

            // Enable Recticle
            var canvasReticle = GameObject.Find("FPSController/FirstPersonCharacter/CanvasReticle");
            canvasReticle.SetActive(true);

            // Relocate world to origin as root GO.
            var world = GameObject.Find("world");

            if (null != world)
            {
                world.transform.position = Vector3.zero;

                world.transform.parent = m_oldWorldParentTransform;
                world.transform.localScale = 1/ m_rescaleFactor * world.transform.localScale;
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
            throw new NotImplementedException();
        }

        public override bool SupportsDPadInput()
        {
            return false;
        }
    }
}