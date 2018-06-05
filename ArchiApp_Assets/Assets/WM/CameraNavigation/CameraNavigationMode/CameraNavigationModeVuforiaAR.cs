using WM.Gamepad;
using Assets.WM.UI;
using Assets.WM.Script.UI.VirtualGamepad;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Vuforia;

namespace Assets.WM.CameraNavigation
{
    public class CameraNavigationModeVuforiaAR : CameraNavigationModeBase
    {
        //
        public string m_worldName = "World";

        //
        static public float s_defaultModelScaleFactor = 0.01f;

        //
        public float m_modelScaleFactor = s_defaultModelScaleFactor;
                
        //
        public GameObject m_vuforia = null;

        //
        public GameObject m_ARCamera = null;

        //
        public GameObject m_imageTarget = null;

        //
        public GameObject m_modelRescale = null;

        //
        public GameObject m_modelAnchor = null;

        //
        public GameObject m_modelTranslation = null;

        //
        public GameObject m_modelRotation = null;

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
            base.OnEnable();

            Debug.Log("CameraNavigationModeVuforiaAR.OnEnable()");

            SetModelScale(s_defaultModelScaleFactor);

            // Disable Recticle
            m_canvasReticle = GameObject.Find("FPSController/FirstPersonCharacter/CanvasReticle");

            if (null != m_canvasReticle)
                m_canvasReticle.SetActive(false);

            var defaultAnchor = GameObject.Find(m_worldName + "/AR_Anchor");

            SetAnchor(defaultAnchor);

            RelocateWorldToAnchor();

            SetVuforiaActive(true);

            // Until world-scale UI is supported properly in AR mode,
            // we forcibly and automatically switch UI mode to Screen space.
            UIManager.GetInstance().SetUIMode(UIMode.ScreenSpace);
        }

        private void SetAnchor(GameObject anchor)
        {            
            if (null == anchor)
            {
                return;
            }
            
            // Adjust the Vuforio Marker Anchor GO position,
            // in order to line up the Model Anchor position to the Image Marker. 
            m_modelAnchor.transform.localPosition = -anchor.transform.localPosition;
        }

        private void RelocateWorldToAnchor()
        {
            m_oldWorldParentTransform = null;

            m_world = GameObject.Find(m_worldName);

            if (null == m_world)
            {
                return;
            }

            // Store the parent transform for the World GO, when not in Vuforia AR state.
            m_oldWorldParentTransform = m_world.transform.parent;

            m_world.transform.SetParent(m_modelAnchor.transform, false);
        }

        override public void OnDisable()
        {
            base.OnDisable();

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
            // 
            float translate = CrossPlatformInputManager.GetAxis("Vertical");

            if (CrossPlatformInputManager.AxisExists(VirtualGamepad_AR.TranslateModel))
            {
                if (0 == translate)
                {
                    translate = CrossPlatformInputManager.VirtualAxisReference(VirtualGamepad_AR.TranslateModel).GetValue;

                    translate *= 0.001f;
                }
            }            

            m_modelTranslation.transform.localPosition = m_modelTranslation.transform.localPosition + translate * Vector3.up;

            // 
            float rotate = CrossPlatformInputManager.GetAxis("Horizontal");

            if (CrossPlatformInputManager.AxisExists(VirtualGamepad_AR.RotateModel))
            {
                if (0 == rotate)
                {
                    rotate = CrossPlatformInputManager.VirtualAxisReference(VirtualGamepad_AR.RotateModel).GetValue;

                    rotate *= 0.01f;
                }                
            }           

            m_modelRotation.transform.localRotation = m_modelRotation.transform.localRotation * Quaternion.Euler(0, rotate, 0);

            // 
            if (Input.GetKeyDown(GamepadXBox.X))
            {
                DecreaseModelScale();
            }

            // 
            if (Input.GetKeyDown(GamepadXBox.B))
            {
                IncreaseModelScale();
            }

            // 
            if (Input.GetKeyDown(GamepadXBox.Y))
            {
                Reset();
            }
        }

        public override void PositionCamera(Vector3 translation, Quaternion rotation)
        {
            // NOOP: ALready taken care of by Vuforia, by moving detected markers.
        }

        public override bool SupportsDPadInput()
        {
            return true;
        }

        public override bool SupportsNavigationViaPOI()
        {
            return false;
        }

        public void IncreaseModelScale()
        {
            SetModelScale(2.0f * m_modelScaleFactor);
        }

        public void DecreaseModelScale()
        {
            SetModelScale(0.5f * m_modelScaleFactor);
        }

        public void Reset()
        {
            // Reset scale
            SetModelScale(s_defaultModelScaleFactor);

            // Reset translation
            m_modelTranslation.transform.localPosition = Vector3.zero;

            // Reset rotation
            m_modelRotation.transform.localRotation = Quaternion.identity;
        }

        private void SetModelScale(float modelScaleFacor)
        {
            m_modelScaleFactor = modelScaleFacor;
            m_modelRescale.transform.localScale = m_modelScaleFactor * Vector3.one;
        }
    }
}