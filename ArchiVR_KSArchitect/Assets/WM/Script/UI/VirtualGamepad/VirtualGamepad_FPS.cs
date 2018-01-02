using Assets.Scripts.WM.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.WM.Script.UI.VirtualGamepad
{
    public class VirtualGamepad_FPS : MonoBehaviour
    {
        // Virtual Axis/Button names.
        static public string RightStickVertical = "VirtualGamePadFPS_ForwardBackward";
        static public string RightStickHorizontal = "VirtualGamePadFPS_LeftRight";
        static public string Jump = "VirtualGamePadFPS_Jump";
        static public string FastMove = "VirtualGamePadFPS_FastMove";

        public DPadBehavior m_FBLRVirtualDPad = null;
        public Button m_jumpButton = null;
        public Button m_runButton = null;

        CrossPlatformInputManager.VirtualAxis m_leftRightVirtualAxis;  // Reference to the joystick in the cross platform input
        CrossPlatformInputManager.VirtualAxis m_forwardBackwardVirtualAxis;    // Reference to the joystick in the cross platform input
        CrossPlatformInputManager.VirtualButton m_jumpVirtualButton;    // Reference to the jump button in the cross platform input
        CrossPlatformInputManager.VirtualButton m_runVirtualButton;     // Reference to the run button in the cross platform input

        void Awake()
        {
            Debug.Log("VirtualGamepad_FPS.Awake()");
            
            // Right stick
            m_leftRightVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VirtualGamepad_FPS.RightStickHorizontal);
            m_leftRightVirtualAxis.Update(0);
            m_forwardBackwardVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VirtualGamepad_FPS.RightStickVertical);
            m_forwardBackwardVirtualAxis.Update(0);

            // Jump button
            m_jumpVirtualButton = new CrossPlatformInputManager.VirtualButton(VirtualGamepad_FPS.Jump);

            // Run button
            m_runVirtualButton = new CrossPlatformInputManager.VirtualButton(VirtualGamepad_FPS.FastMove);
        }

        void Start()
        {
            Debug.Log("VirtualGamepad_FPS.Start()");
        }

        private void OnEnable()
        {
            Debug.Log("VirtualGamepad_FPS.OnEnable()");
            
            // LeftRight
            if (CrossPlatformInputManager.AxisExists(m_leftRightVirtualAxis.name))
            {
                //m_oldHorizontalVirtualAxis = CrossPlatformInputManager.VirtualAxisReference(
                //    m_horizontalVirtualAxis.name);
                CrossPlatformInputManager.UnRegisterVirtualAxis(m_leftRightVirtualAxis.name);
            }

            // ForwardBackward
            CrossPlatformInputManager.RegisterVirtualAxis(m_leftRightVirtualAxis);
            m_leftRightVirtualAxis.Update(0);

            if (CrossPlatformInputManager.AxisExists(m_forwardBackwardVirtualAxis.name))
            {
                //m_oldVerticalVirtualAxis = CrossPlatformInputManager.VirtualAxisReference(
                //    m_verticalVirtualAxis.name);
                CrossPlatformInputManager.UnRegisterVirtualAxis(m_forwardBackwardVirtualAxis.name);
            }

            CrossPlatformInputManager.RegisterVirtualAxis(m_forwardBackwardVirtualAxis);
            m_forwardBackwardVirtualAxis.Update(0);

            // Jump
            if (CrossPlatformInputManager.ButtonExists(m_jumpVirtualButton.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualButton(m_jumpVirtualButton.name);
            }
            CrossPlatformInputManager.RegisterVirtualButton(m_jumpVirtualButton);

            if (m_jumpButton)
            {
                //m_jumpButton.OnPointerDown+= JumpButton_OnPointerDown;
                //m_jumpButton.OnPointerUp += JumpButton_OnPointerUp;
            }

            // Run
            if (CrossPlatformInputManager.ButtonExists(m_runVirtualButton.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualButton(m_runVirtualButton.name);
            }
            CrossPlatformInputManager.RegisterVirtualButton(m_runVirtualButton);
                       
            if (m_runButton)
            {
                //m_runButton.OnPointerDown += RunButton_OnPointerDown;
                //m_runButton.OnPointerUp += RunButton_OnPointerUp;
            }
        }

        private void OnDisable()
        {
            Debug.Log("VirtualGamepad_FPS.OnDisable()");

            CrossPlatformInputManager.UnRegisterVirtualAxis(m_leftRightVirtualAxis.name);
            CrossPlatformInputManager.UnRegisterVirtualAxis(m_forwardBackwardVirtualAxis.name);

            if (m_jumpButton)
            {
                CrossPlatformInputManager.UnRegisterVirtualButton(m_jumpButton.name);

                /*
                m_jumpButton.OnPointerDown -= JumpButton_OnPointerDown;
                m_jumpButton.OnPointerUp -= JumpButton_OnPointerUp;
                */
            }

            if (m_runButton)
            {
                CrossPlatformInputManager.UnRegisterVirtualButton(m_runButton.name);
            
                /*
                m_runButton.OnPointerDown -= RunButton_OnPointerDown;
                m_runButton.OnPointerUp -= RunButton_OnPointerUp;
                */
            }
        }

        void JumpButton_OnPointerDown(PointerEventData ped)
        {
            m_jumpVirtualButton.Pressed();
        }

        void JumpButton_OnPointerUp(PointerEventData ped)
        {
            m_jumpVirtualButton.Released();
        }

        void RunButton_OnPointerDown(PointerEventData ped)
        {
            m_runVirtualButton.Pressed();
        }

        void RunButton_OnPointerUp(PointerEventData ped)
        {
            m_runVirtualButton.Released();
        }

        // Update is called once per frame
        void Update()
        {
            var stickOffsetFBLR = m_FBLRVirtualDPad.GetStickOffset();

            {
                var leftRight = stickOffsetFBLR.x;

                if (null != m_leftRightVirtualAxis)
                {
                    m_leftRightVirtualAxis.Update(leftRight);
                }
            }

            {
                var forwardBackward = stickOffsetFBLR.y;

                if (null != m_forwardBackwardVirtualAxis)
                {
                    m_forwardBackwardVirtualAxis.Update(forwardBackward);
                }
            }
        }
    }
}