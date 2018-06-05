using Assets.WM.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.WM.Script.UI.VirtualGamepad
{
    public class VirtualGamepad_Fly : MonoBehaviour
    {
        // Virtual Axis/Button names.
        static public string UpDown = "VirtualGamePadFly_UpDown";
        static public string ForwardBackward = "VirtualGamePadFly_ForwardBackward";
        static public string LeftRight = "VirtualGamePadFly_LeftRight";
        static public string FastMove = "VirtualGamePadFly_FastMove";

        public DPadBehavior m_FBLRVirtualDPad = null;
        public DPadBehavior m_UpDownVirtualDPad = null;
        public Button m_fastMoveButton = null;

        CrossPlatformInputManager.VirtualAxis m_leftRightVirtualAxis;  // Reference to the joystick in the cross platform input
        CrossPlatformInputManager.VirtualAxis m_forwardBackwardVirtualAxis;    // Reference to the joystick in the cross platform input
        CrossPlatformInputManager.VirtualAxis m_upDownVirtualAxis;    // Reference to the joystick in the cross platform input
        CrossPlatformInputManager.VirtualButton m_fastMoveVirtualButton;     // Reference to the run button in the cross platform input

        void Awake()
        {
            Debug.Log("VirtualGamepad_Fly.Awake()");

            // UpDown stick
            m_upDownVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VirtualGamepad_Fly.UpDown);

            // ForwardBackwardLeftRight stick
            m_leftRightVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VirtualGamepad_Fly.LeftRight);
            m_leftRightVirtualAxis.Update(0);
            m_forwardBackwardVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VirtualGamepad_Fly.ForwardBackward);
            m_forwardBackwardVirtualAxis.Update(0);

            // FastMove button
            m_fastMoveVirtualButton = new CrossPlatformInputManager.VirtualButton(VirtualGamepad_Fly.FastMove);
        }

        void Start()
        {
            Debug.Log("VirtualGamepad_Fly.Start()");
        }

        private void OnEnable()
        {
            Debug.Log("VirtualGamepad_Fly.OnEnable()");

            // UpDown
            if (CrossPlatformInputManager.AxisExists(m_upDownVirtualAxis.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualAxis(m_upDownVirtualAxis.name);
            }

            CrossPlatformInputManager.RegisterVirtualAxis(m_upDownVirtualAxis);
            m_upDownVirtualAxis.Update(0);

            // LeftRight
            if (CrossPlatformInputManager.AxisExists(m_leftRightVirtualAxis.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualAxis(m_leftRightVirtualAxis.name);
            }

            CrossPlatformInputManager.RegisterVirtualAxis(m_leftRightVirtualAxis);
            m_leftRightVirtualAxis.Update(0);

            // ForwardBackward
            if (CrossPlatformInputManager.AxisExists(m_forwardBackwardVirtualAxis.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualAxis(m_forwardBackwardVirtualAxis.name);
            }

            CrossPlatformInputManager.RegisterVirtualAxis(m_forwardBackwardVirtualAxis);
            m_forwardBackwardVirtualAxis.Update(0);

            // FastMove
            if (m_fastMoveButton)
            {
                if (CrossPlatformInputManager.ButtonExists(m_fastMoveVirtualButton.name))
                {
                    CrossPlatformInputManager.UnRegisterVirtualButton(m_fastMoveVirtualButton.name);
                }
                CrossPlatformInputManager.RegisterVirtualButton(m_fastMoveVirtualButton);

                //m_fastMoveButton.OnPointerDown += FastMoveButton_OnPointerDown;
                //m_fastMoveButton.OnPointerUp += FastMoveButton_OnPointerUp;
            }
        }

        private void OnDisable()
        {
            Debug.Log("VirtualGamepad.OnDisable()");

            CrossPlatformInputManager.UnRegisterVirtualAxis(m_leftRightVirtualAxis.name);
            CrossPlatformInputManager.UnRegisterVirtualAxis(m_forwardBackwardVirtualAxis.name);

            if (m_fastMoveButton)
            {
                CrossPlatformInputManager.UnRegisterVirtualButton(m_fastMoveButton.name);

                /*
                m_fastMoveButton.OnPointerDown -= FastMoveButton_OnPointerDown;
                m_fastMoveButton.OnPointerUp -= FastMoveButton_OnPointerUp;
                */
            }
        }

        void FastMoveButton_OnPointerDown(PointerEventData ped)
        {
            m_fastMoveVirtualButton.Pressed();
        }

        void FastMoveButton_OnPointerUp(PointerEventData ped)
        {
            m_fastMoveVirtualButton.Released();
        }

        // Update is called once per frame
        void Update()
        {
            var stickOffsetFBLR = m_FBLRVirtualDPad.GetStickOffset();

            // LeftRight
            {
                var leftRight = stickOffsetFBLR.x;

                if (null != m_leftRightVirtualAxis)
                {
                    m_leftRightVirtualAxis.Update(leftRight);
                }
            }

            // ForwardBackward
            {
                var forwardBackward = stickOffsetFBLR.y;

                if (null != m_forwardBackwardVirtualAxis)
                {
                    m_forwardBackwardVirtualAxis.Update(forwardBackward);
                }
            }

            // UpDown
            var stickOffsetUpDown = m_UpDownVirtualDPad.GetStickOffset();

            {
                var upDown = stickOffsetUpDown.y;

                if (null != m_upDownVirtualAxis)
                {
                    m_upDownVirtualAxis.Update(upDown);
                }
            }
        }
    }
}