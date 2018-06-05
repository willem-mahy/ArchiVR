using Assets.WM;
using Assets.WM.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace WM.Gamepad
{
    public class GamePadPreview : MonoBehaviour
    {
        public Button m_buttonX = null;
        public Button m_buttonY = null;
        public Button m_buttonA = null;
        public Button m_buttonB = null;

        public Button m_buttonL1 = null;
        public Button m_buttonL2 = null;
        public Button m_buttonR1 = null;
        public Button m_buttonR2 = null;

        public Button m_buttonSelect = null;
        public Button m_buttonStart = null;
        public Button m_buttonWindows = null;

        public Button m_buttonDPadLeft = null;
        public Button m_buttonDPadRight = null;
        public Button m_buttonDPadUp = null;
        public Button m_buttonDPadDown = null;

        // Shows the actual value of the L2/R2 trigger button position.
        public Text m_textL2R2 = null;

        public Text m_textA_Function = null;
        public Text m_textB_Function = null;
        public Text m_textX_Function = null;
        public Text m_textY_Function = null;

        public Text m_textStart_Function = null;
        public Text m_textSelect_Function = null;
        public Text m_textWindows_Function = null;

        public Text m_textDPadLeft_Function = null;
        public Text m_textDPadRight_Function = null;
        public Text m_textDPadUp_Function = null;
        public Text m_textDPadDown_Function = null;

        public Text m_textLeftJoystick_Function = null;
        public Text m_textRightJoystick_Function = null;

        public Text m_textL1_Function = null;
        public Text m_textR1_Function = null;
        public Text m_textL2R2_Function = null;

        public DPadBehavior m_leftJoystick = null;
        public DPadBehavior m_rightJoystick = null;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePressedState();
        }

        void UpdatePressedState(DPadBehavior dp, String keyName)
        {
            if (dp)
            {
                var image = dp.m_stick.GetComponentInChildren<Image>();

                bool pressed = Input.GetKey(keyName);

                image.color = (pressed ? new Color32(0, 255, 0, 255) : new Color32(255, 255, 255, 255));
            }
        }

        void UpdatePressedState(Button button, String keyName)
        {
            if (button)
            {
                if (Input.GetKey(keyName))
                {
                    button.OnPointerEnter(null);
                }
                else
                {
                    button.OnPointerExit(null);
                }
            }
        }

        public void UpdateJoyStick(
                DPadBehavior joystick,
                String xAxis,
                String yAxis)
        {
            float s = gameObject.GetComponentInParent<Canvas>().scaleFactor;

            if (joystick)
            {
                var x = 0.5f * (1 + CrossPlatformInputManager.GetAxis(xAxis));
                var y = 0.5f * (1 + CrossPlatformInputManager.GetAxis(yAxis));

                var offset = new Vector2(y, x);

                joystick.SetStickOffsetNormalized(offset, s);
            }
        }

        void UpdatePressedState()
        {
            UpdatePressedState(m_leftJoystick, GamepadXBox.LeftAnalogPress);
            UpdatePressedState(m_rightJoystick, GamepadXBox.RightAnalogPress);

            UpdatePressedState(m_buttonA, GamepadXBox.A);
            UpdatePressedState(m_buttonB, GamepadXBox.B);
            UpdatePressedState(m_buttonX, GamepadXBox.X);
            UpdatePressedState(m_buttonY, GamepadXBox.Y);

            UpdatePressedState(m_buttonL1, GamepadXBox.L1);
            UpdatePressedState(m_buttonR1, GamepadXBox.R1);

            var valueL2R2 = CrossPlatformInputManager.GetAxis(GamepadXBox.L2R2);

            if (m_textL2R2)
            {
                m_textL2R2.text = "" + valueL2R2;
            }

            if (m_buttonL2)
            {
                if (valueL2R2 == 0)
                {
                    m_buttonL2.OnPointerEnter(null);
                }
                else
                {
                    m_buttonL2.OnPointerExit(null);
                }
            }

            if (m_buttonR2)
            {
                if (valueL2R2 == 0)
                {
                    m_buttonR2.OnPointerEnter(null);
                }
                else
                {
                    m_buttonR2.OnPointerExit(null);
                }
            }

            UpdateJoyStick(
                m_leftJoystick,
                GamepadXBox.LeftAnalogVertical,
                GamepadXBox.LeftAnalogHorizontal);

            UpdateJoyStick(
                m_rightJoystick,
                GamepadXBox.RightAnalogVertical,
                GamepadXBox.RightAnalogHorizontal);

            var valueDPadVertical = CrossPlatformInputManager.GetAxis(GamepadXBox.DPadVertical);
            var valueDPadHorizontal = CrossPlatformInputManager.GetAxis(GamepadXBox.DPadHorizontal);

            if (m_buttonDPadLeft)
            {
                if (valueDPadHorizontal < 0)
                {
                    m_buttonDPadLeft.OnPointerEnter(null);
                }
                else
                {
                    m_buttonDPadLeft.OnPointerExit(null);
                }
            }

            if (m_buttonDPadRight)
            {
                if (valueDPadHorizontal > 0)
                {
                    m_buttonDPadRight.OnPointerEnter(null);
                }
                else
                {
                    m_buttonDPadRight.OnPointerExit(null);
                }
            }

            if (m_buttonDPadDown)
            {
                if (valueDPadVertical < 0)
                {
                    m_buttonDPadDown.OnPointerEnter(null);
                }
                else
                {
                    m_buttonDPadDown.OnPointerExit(null);
                }
            }

            if (m_buttonDPadUp)
            {
                if (valueDPadVertical > 0)
                {
                    m_buttonDPadUp.OnPointerEnter(null);
                }
                else
                {
                    m_buttonDPadUp.OnPointerExit(null);
                }
            }

            UpdatePressedState(m_buttonSelect, GamepadXBox.Select);
            UpdatePressedState(m_buttonStart, GamepadXBox.Start);
            //UpdatePressedState(m_buttonWindows, GamepadXBox.Windows);

            //if (m_joystickLeft)
            //{
            //    var value = new Vector2(CrossPlatformInputManager.GetAxis(GamepadXBox.LeftAnalogHorizontal), CrossPlatformInputManager.GetAxis(GamepadXBox.LeftAnalogVertical));

            //    m_joystickLeft.SetPosition(value);
            //}
        }

        void SetFunctionText(
            Text text,
            Dictionary<String, String> mapping,
            String functionName)
        {
            if (text)
            {
                text.text = mapping.ContainsKey(functionName) ? mapping[functionName] : "";
            }
        }

        public void SetFunctionTexts(Dictionary<String, String> mapping)
        {
            SetFunctionText(m_textDPadLeft_Function, mapping, "DPadLeft");
            SetFunctionText(m_textDPadRight_Function, mapping, "DPadRight");
            SetFunctionText(m_textDPadUp_Function, mapping, "DPadUp");
            SetFunctionText(m_textDPadDown_Function, mapping, "DPadDown");

            SetFunctionText(m_textA_Function, mapping, GamepadXBox.A);
            SetFunctionText(m_textB_Function, mapping, GamepadXBox.B);
            SetFunctionText(m_textX_Function, mapping, GamepadXBox.X);
            SetFunctionText(m_textY_Function, mapping, GamepadXBox.Y);

            SetFunctionText(m_textSelect_Function, mapping, GamepadXBox.Select);
            SetFunctionText(m_textStart_Function, mapping, GamepadXBox.Start);

            SetFunctionText(m_textL1_Function, mapping, GamepadXBox.L1);
            SetFunctionText(m_textR1_Function, mapping, GamepadXBox.R1);
            SetFunctionText(m_textL2R2_Function, mapping, GamepadXBox.L2R2);

            SetFunctionText(m_textLeftJoystick_Function, mapping, "LeftJoystick");
            SetFunctionText(m_textRightJoystick_Function, mapping, "RightJoystick");
        }
    }
}