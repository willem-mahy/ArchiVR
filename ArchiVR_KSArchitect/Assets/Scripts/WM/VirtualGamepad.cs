using Assets.Scripts.WM.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class VirtualGamepad : MonoBehaviour {

    static public string LeftStickVertical = "VirtualGamePad_LeftStick_Vertical";
    static public string RightStickVertical = "VirtualGamePad_RightStick_Vertical";
    static public string RightStickHorizontal = "VirtualGamePad_RightStick_Horizontal";
    static public string Jump = "VirtualGamePad_Jump";
    static public string FastMove = "VirtualGamePad_FastMove";

    public DPadBehavior m_virtualDPadLeft = null;
    public DPadBehavior m_virtualDPadRight = null;
    public Button m_jumpButton = null;
    public Button m_runButton = null;

    //CrossPlatformInputManager.VirtualAxis m_oldHorizontalVirtualAxis;  // Reference to the joystick in the cross platform input
    //CrossPlatformInputManager.VirtualAxis m_oldVerticalVirtualAxis;    // Reference to the joystick in the cross platform input
    //CrossPlatformInputManager.VirtualButton m_oldJumpVirtualButton;    // Reference to the jump button in the cross platform input
    //CrossPlatformInputManager.VirtualButton m_oldRunVirtualButton;     // Reference to the run button in the cross platform input

    CrossPlatformInputManager.VirtualAxis m_rightStickHorizontalVirtualAxis;  // Reference to the joystick in the cross platform input
    CrossPlatformInputManager.VirtualAxis m_rightStickVerticalVirtualAxis;    // Reference to the joystick in the cross platform input
    CrossPlatformInputManager.VirtualAxis m_leftStickVerticalVirtualAxis;    // Reference to the joystick in the cross platform input
    CrossPlatformInputManager.VirtualButton m_jumpVirtualButton;    // Reference to the jump button in the cross platform input
    CrossPlatformInputManager.VirtualButton m_runVirtualButton;     // Reference to the run button in the cross platform input

    void Awake() {
        Debug.Log("VirtualGamepad.Awake()");
        
        // Left stick
        m_leftStickVerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VirtualGamepad.LeftStickVertical);

        // Right stick
        m_rightStickHorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VirtualGamepad.RightStickHorizontal);
        m_rightStickHorizontalVirtualAxis.Update(0);
        m_rightStickVerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(VirtualGamepad.RightStickVertical);
        m_rightStickVerticalVirtualAxis.Update(0);

        // Jump button
        m_jumpVirtualButton = new CrossPlatformInputManager.VirtualButton(VirtualGamepad.Jump);

        // Run button
        m_runVirtualButton = new CrossPlatformInputManager.VirtualButton(VirtualGamepad.FastMove);
    }

    void Start()
    {
        Debug.Log("VirtualGamepad.Start()");
    }

    private void OnEnable()
    {
        Debug.Log("VirtualGamepad.OnEnable()");



        //CrossPlatformInputManager.SwitchActiveInputMethod(
        //    CrossPlatformInputManager.ActiveInputMethod.Touch);


        if (CrossPlatformInputManager.AxisExists(m_leftStickVerticalVirtualAxis.name))
        {
            CrossPlatformInputManager.UnRegisterVirtualAxis(m_leftStickVerticalVirtualAxis.name);
        }

        CrossPlatformInputManager.RegisterVirtualAxis(m_leftStickVerticalVirtualAxis);
        m_leftStickVerticalVirtualAxis.Update(0);


        if (CrossPlatformInputManager.AxisExists(m_rightStickHorizontalVirtualAxis.name))
        {
            //m_oldHorizontalVirtualAxis = CrossPlatformInputManager.VirtualAxisReference(
            //    m_horizontalVirtualAxis.name);
            CrossPlatformInputManager.UnRegisterVirtualAxis(m_rightStickHorizontalVirtualAxis.name);
        }
        
        CrossPlatformInputManager.RegisterVirtualAxis(m_rightStickHorizontalVirtualAxis);
        m_rightStickHorizontalVirtualAxis.Update(0);
        
        if (CrossPlatformInputManager.AxisExists(m_rightStickVerticalVirtualAxis.name))
        {
            //m_oldVerticalVirtualAxis = CrossPlatformInputManager.VirtualAxisReference(
            //    m_verticalVirtualAxis.name);
            CrossPlatformInputManager.UnRegisterVirtualAxis(m_rightStickVerticalVirtualAxis.name);
        }
        
        CrossPlatformInputManager.RegisterVirtualAxis(m_rightStickVerticalVirtualAxis);
        m_rightStickVerticalVirtualAxis.Update(0);

        if (CrossPlatformInputManager.ButtonExists(m_jumpVirtualButton.name))
        {
            CrossPlatformInputManager.UnRegisterVirtualButton(m_jumpVirtualButton.name);
        }
        CrossPlatformInputManager.RegisterVirtualButton(m_jumpVirtualButton);

        if (CrossPlatformInputManager.ButtonExists(m_runVirtualButton.name))
        {
            CrossPlatformInputManager.UnRegisterVirtualButton(m_runVirtualButton.name);
        }        
        CrossPlatformInputManager.RegisterVirtualButton(m_runVirtualButton);
        
        /*
        if (m_jumpButton)
        {
            m_jumpButton.OnPointerDown+= JumpButton_OnPointerDown;
            m_jumpButton.OnPointerUp += JumpButton_OnPointerUp;
        }

        if (m_runButton)
        {
            m_runButton.OnPointerDown += RunButton_OnPointerDown;
            m_runButton.OnPointerUp += RunButton_OnPointerUp;
        }
        */
    }

    private void OnDisable()
    {
        Debug.Log("VirtualGamepad.OnDisable()");
        
        CrossPlatformInputManager.UnRegisterVirtualAxis(m_rightStickHorizontalVirtualAxis.name);
        CrossPlatformInputManager.UnRegisterVirtualAxis(m_rightStickVerticalVirtualAxis.name);

        if (null != m_jumpButton)
        {
            CrossPlatformInputManager.UnRegisterVirtualButton(m_jumpButton.name);
        }

        if (null != m_runButton)
        {
            CrossPlatformInputManager.UnRegisterVirtualButton(m_runButton.name);
        }

        // Re-register old input axes.
        //if (null != m_oldHorizontalVirtualAxis)
        //{
        //    CrossPlatformInputManager.RegisterVirtualAxis(m_oldHorizontalVirtualAxis);
        //}

        //if (null != m_oldVerticalVirtualAxis)
        //{
        //    CrossPlatformInputManager.RegisterVirtualAxis(m_oldVerticalVirtualAxis);
        //}


        //if (null != m_oldJumpVirtualButton)
        //{
        //    CrossPlatformInputManager.RegisterVirtualButton(m_oldJumpVirtualButton);
        //}


        //if (null != m_oldRunVirtualButton)
        //{
        //    CrossPlatformInputManager.RegisterVirtualButton(m_oldRunVirtualButton);
        //}


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
    void Update () {

        var stickOffsetLeft = m_virtualDPadLeft.GetStickOffset();

        {
            var horizontal = stickOffsetLeft.x;

            //if (null != m_horizontalVirtualAxis)
            //{
            //    m_horizontalVirtualAxis.Update(horizontal);
            //}
        }

        {
            var vertical = stickOffsetLeft.y / 100;

            if (null != m_leftStickVerticalVirtualAxis)
            {
                m_leftStickVerticalVirtualAxis.Update(vertical);
            }
        }

        var stickOffsetRight = m_virtualDPadRight.GetStickOffset();

        {
            var horizontal = stickOffsetRight.x;

            if (null != m_rightStickHorizontalVirtualAxis)
            {
                m_rightStickHorizontalVirtualAxis.Update(horizontal);
            }
        }

        {
            var vertical = stickOffsetRight.y;

            if (null != m_rightStickVerticalVirtualAxis)
            {
                m_rightStickVerticalVirtualAxis.Update(vertical);
            }
        }
    }
}
