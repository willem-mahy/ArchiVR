using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.WM.Util;

namespace Assets.Scripts.WM.CameraControl
{
    public class ControlCameraFlyGamePad : CameraFlyBase
    {
        // Use this for initialization
        void Start()
        {
            //m_HorizontalPhysicalAxis = new CrossPlatformInputManager.VirtualAxis("Horizontal");
            //CrossPlatformInputManager.UnRegisterVirtualAxis("Horizontal");
            //CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalPhysicalAxis);

            //m_VerticalPhysicalAxis = new CrossPlatformInputManager.VirtualAxis("Vertical");
            //CrossPlatformInputManager.UnRegisterVirtualAxis("Vertical");
            //CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalPhysicalAxis);

            //m_jumpButton = new CrossPlatformInputManager.VirtualButton("Jump");
            //CrossPlatformInputManager.UnRegisterVirtualButton("Jump");
            //CrossPlatformInputManager.RegisterVirtualButton(m_jumpButton);
        }

        // Physical gamepad buttons are mapped as follows:
        // A = 0
        // B = 1
        // X = 2
        // Y = 3
        private static string Fast  = "joystick button 4";
        private static string Down  = "joystick button 0";
        private static string Up    = "joystick button 1";

        // Update is called once per frame
        void Update()
        {
            DebugUtil.LogJoystickButtonPress();

            float speed = Input.GetKey(Fast) ? m_translateSpeedFast : m_translateSpeedNormal;

            var offset = Time.deltaTime * speed;

            float offsetUpDown = 0;

            if (Input.GetKey(Down))
            { 
                offsetUpDown += Time.deltaTime * speed;
            }
            
            if (Input.GetKey(Up))
            {
                offsetUpDown -= Time.deltaTime * speed;
            }

            TranslateY(offsetUpDown);

            var movementDirectionXZ = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"));

            if ((movementDirectionXZ.x != 0) || (movementDirectionXZ.y != 0))
            {
                Debug.Log("movementDirectionXZ: " + movementDirectionXZ.ToString());
            }

            TranslateXZ(movementDirectionXZ, movementDirectionXZ.magnitude * offset, true);

            //if (rigidBodyFPSController)
            //{
            //    m_HorizontalPhysicalAxis.Update(h);
            //    m_VerticalPhysicalAxis.Update(v);
            //}
        }
    }
}
