using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.Scripts.WM
{
    // XBox 36 / GameSir G4s gamepad mappings:
    //
    // 0 = A
    // 1 = B
    // 2 = X
    // 3 = Y
    // 4 = L1
    // 5 = R1
    // 6 = Select
    // 7 = Start
    // 8 = Left Analog pressed joystick button
    // 9 = Right Analog pressed joystick button
    // X Axis = Left Analog X Axis Joystick Axis
    // Y Axis = Left Analog Y Axis Joystick Axis
    // 3th Axis = Left Trigger and Right Trigger  (L2/R2)
    // 4th Axis = Right Analog X Axis Joystick Axis
    // 5th Axis = Right Analog Y Axis Joystick Axis
    // 6th Axis = Left/Right on D-Pad Joystick Axis
    // 7th Axis = Up/Down on D-Pad Joystick Axis


    class PhysicalGamepad : MonoBehaviour
    {
        CrossPlatformInputManager.VirtualAxis m_horizontalVirtualAxis;
        CrossPlatformInputManager.VirtualAxis m_verticalVirtualAxis;

        CrossPlatformInputManager.VirtualAxis m_horizontalRotationVirtualAxis;
        CrossPlatformInputManager.VirtualAxis m_verticalRotationVirtualAxis;

        CrossPlatformInputManager.VirtualAxis m_upDownVirtualAxis;
        CrossPlatformInputManager.VirtualButton m_jumpVirtualButton;
        CrossPlatformInputManager.VirtualButton m_runVirtualButton;

        void Awake()
        {
            Debug.Log("PhysicalGamepad.Awake()");

            m_horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis("Horizontal");
            m_verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis("Vertical");

            m_horizontalRotationVirtualAxis = new CrossPlatformInputManager.VirtualAxis("HorizontalRotation");
            m_verticalRotationVirtualAxis = new CrossPlatformInputManager.VirtualAxis("VerticalRotation");

            m_upDownVirtualAxis = new CrossPlatformInputManager.VirtualAxis("UpDown");
            m_jumpVirtualButton = new CrossPlatformInputManager.VirtualButton("Jump");
            m_runVirtualButton = new CrossPlatformInputManager.VirtualButton("Run");
        }

        void Start()
        {
            Debug.Log("PhysicalGamepad.Start()");
        }

        private void OnEnable()
        {
            Debug.Log("PhysicalGamepad.OnEnable()");

            /*
            // Horizontal Axis
            if (CrossPlatformInputManager.AxisExists(m_horizontalVirtualAxis.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualAxis(m_horizontalVirtualAxis.name);
            }
            CrossPlatformInputManager.RegisterVirtualAxis(m_horizontalVirtualAxis);

            // Vertical Axis
            if (CrossPlatformInputManager.AxisExists(m_verticalVirtualAxis.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualAxis(m_verticalVirtualAxis.name);
            }
            CrossPlatformInputManager.RegisterVirtualAxis(m_verticalVirtualAxis);
            */

            // Horizontal Rotation Axis
            if (CrossPlatformInputManager.AxisExists(m_horizontalRotationVirtualAxis.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualAxis(m_horizontalRotationVirtualAxis.name);
            }
            CrossPlatformInputManager.RegisterVirtualAxis(m_horizontalRotationVirtualAxis);

            // Vertical Rotation Axis
            if (CrossPlatformInputManager.AxisExists(m_verticalRotationVirtualAxis.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualAxis(m_verticalRotationVirtualAxis.name);
            }
            CrossPlatformInputManager.RegisterVirtualAxis(m_verticalRotationVirtualAxis);
            

            // UpoDown Axis
            if (CrossPlatformInputManager.AxisExists(m_upDownVirtualAxis.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualAxis(m_upDownVirtualAxis.name);
            }
            CrossPlatformInputManager.RegisterVirtualAxis(m_upDownVirtualAxis);

            // Jump Button
            if (CrossPlatformInputManager.ButtonExists(m_jumpVirtualButton.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualButton(m_jumpVirtualButton.name);
            }
            CrossPlatformInputManager.RegisterVirtualButton(m_jumpVirtualButton);

            // Run Button
            if (CrossPlatformInputManager.ButtonExists(m_runVirtualButton.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualButton(m_runVirtualButton.name);
            }
            CrossPlatformInputManager.RegisterVirtualButton(m_runVirtualButton);

            CrossPlatformInputManager.SwitchActiveInputMethod(CrossPlatformInputManager.ActiveInputMethod.Hardware);
        }

        private void OnDisable()
        {
            Debug.Log("PhysicalGamepad.OnDisable()");

            /*
            CrossPlatformInputManager.UnRegisterVirtualAxis("Horizontal");
            CrossPlatformInputManager.UnRegisterVirtualAxis("Vertical");
            CrossPlatformInputManager.UnRegisterVirtualButton("Jump");
            CrossPlatformInputManager.UnRegisterVirtualButton("Run");
            */

            // TODO: re-register old inputs???
            /*
            CrossPlatformInputManager.RegisterVirtualAxis(m_oldHorizontalVirtualAxis);
            CrossPlatformInputManager.RegisterVirtualAxis(m_oldVerticalVirtualAxis);
            CrossPlatformInputManager.RegisterVirtualButton(m_oldJumpVirtualButton);
            CrossPlatformInputManager.RegisterVirtualButton(m_oldRunVirtualButton);
            */

            /*
            if (m_jumpButton)
            {
                m_jumpButton.OnPointerDown -= JumpButton_OnPointerDown;
                m_jumpButton.OnPointerUp -= JumpButton_OnPointerUp;
            }

            if (m_runButton)
            {
                m_runButton.OnPointerDown -= RunButton_OnPointerDown;
                m_runButton.OnPointerUp -= RunButton_OnPointerUp;
            }
            */
        }

        private void LogGamepadState()
        {
            // Log pressed buttons.
            {
                var text = "";

                for (int i = 0; i < 10; ++i)
                {
                    var keyName = "joystick button " + i;

                    if (Input.GetKey(keyName))
                    {
                        text += "\n- " + keyName;
                    }
                }

                if (text != "")
                {
                    text = "Pressed joystick buttons" + text;
                    Debug.Log(text);
                }
            }
        }
    
        void Update()
        {

            if (Input.GetKey("joystick button 0")) // A
            {
            }
            if (Input.GetKey("joystick button 1")) // B
            {
            }
            if (Input.GetKey("joystick button 2")) // X
            {
            }
            if (Input.GetKey("joystick button 4")) // Y
            {
            }
            if (Input.GetKey("joystick button 5")) // L1
            {
            }
            if (Input.GetKey("joystick button 6")) // R1
            {
            }
            if (Input.GetKey("joystick button 7")) // L1
            {
            }
            if (Input.GetKey("joystick button 8")) // R1
            {
            }

            {
                float horizontal = Input.GetAxis("Horizontal");

                if (horizontal != 0)
                {
                    Debug.Log("Joystick Horizontal: " + horizontal);
                }

                m_horizontalVirtualAxis.Update(horizontal);
            }

            {
                float vertical = Input.GetAxis("Vertical");

                if (vertical != 0)
                {
                    Debug.Log("Joystick Vertical: " + vertical);
                }

                m_verticalVirtualAxis.Update(vertical);
            }

            {
                float horizontal = Input.GetAxis("HorizontalRotation");

                if (horizontal != 0)
                {
                    Debug.Log("Joystick HorizontalRotation: " + horizontal);
                }

                m_horizontalRotationVirtualAxis.Update(horizontal);
            }

            {
                float verticalRotation = Input.GetAxis("VerticalRotation");

                if (verticalRotation != 0)
                {
                    Debug.Log("Joystick VerticalRotation: " + verticalRotation);
                }

                //VerticalRotation -= 1;

                m_verticalRotationVirtualAxis.Update(verticalRotation);
            }

            {
                float upDown = Input.GetAxis("UpDown");

                if (upDown != 0)
                {
                    Debug.Log("Joystick UpDown: " + upDown);
                }

                //Debug.Log("upDown=" + upDown);
                ////GUI.Label(new Rect(100, 100, 50, 50), "upDown=" + upDown);
                //upDown -= 1;

                m_upDownVirtualAxis.Update(upDown);
            }
        }
    }
}

