using Assets.WM.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.WM.Script.UI.VirtualGamepad
{
    public class VirtualGamepad_AR : MonoBehaviour
    {
        // Virtual Axis/Button names.
        static public string TranslateModel = "VirtualGamepadAR_TranslateModel";
        static public string RotateModel = "VirtualGamepadAR_RotateModel";

        public DPadBehavior m_virtualDPadTranslate = null;
        public DPadBehavior m_virtualDPadRotate = null;
        
        CrossPlatformInputManager.VirtualAxis m_translateModelVirtualAxis;  // Reference to the joystick in the cross platform input
        CrossPlatformInputManager.VirtualAxis m_rotateModelVirtualAxis;    // Reference to the joystick in the cross platform input

        void Awake()
        {
            Debug.Log("VirtualGamepad_AR.Awake()");

            // TranslateModel
            m_translateModelVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VirtualGamepad_AR.TranslateModel);
            m_translateModelVirtualAxis.Update(0);

            // RotateModel
            m_rotateModelVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VirtualGamepad_AR.RotateModel);
            m_rotateModelVirtualAxis.Update(0);
        }

        void Start()
        {
            Debug.Log("VirtualGamepad_AR.Start()");
        }

        private void OnEnable()
        {
            Debug.Log("VirtualGamepad_AR.OnEnable()");

            if (CrossPlatformInputManager.AxisExists(m_rotateModelVirtualAxis.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualAxis(m_rotateModelVirtualAxis.name);
            }

            CrossPlatformInputManager.RegisterVirtualAxis(m_rotateModelVirtualAxis);
            m_rotateModelVirtualAxis.Update(0);


            if (CrossPlatformInputManager.AxisExists(m_translateModelVirtualAxis.name))
            {
               CrossPlatformInputManager.UnRegisterVirtualAxis(m_translateModelVirtualAxis.name);
            }

            CrossPlatformInputManager.RegisterVirtualAxis(m_translateModelVirtualAxis);
            m_translateModelVirtualAxis.Update(0);            
        }

        private void OnDisable()
        {
            Debug.Log("VirtualGamepad.OnDisable()");

            CrossPlatformInputManager.UnRegisterVirtualAxis(m_translateModelVirtualAxis.name);
            CrossPlatformInputManager.UnRegisterVirtualAxis(m_rotateModelVirtualAxis.name);
        }

        //TODO? Resets the model at default translation and rotation.
        //void ResetButton_OnPointerDown(PointerEventData ped)
        //{
        //    m_resetVirtualButton.Pressed();
        //}

        // Update is called once per frame
        void Update()
        {
            // TranslateModel
            var translation = m_virtualDPadTranslate.GetStickOffset().y;

            m_translateModelVirtualAxis.Update(translation);

            // RotateModel
            var rotation = m_virtualDPadRotate.GetStickOffset().x;

            m_rotateModelVirtualAxis.Update(rotation);
        }
    }
}
