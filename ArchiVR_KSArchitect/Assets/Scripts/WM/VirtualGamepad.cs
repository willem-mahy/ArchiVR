using Assets.Scripts.WM.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class VirtualGamepad : MonoBehaviour {
    public DPadBehavior m_virtualDPadLeft = null;
    public DPadBehavior m_virtualDPadRight = null;
    public Button m_jumpButton = null;
    public Button m_runButton = null;

    CrossPlatformInputManager.VirtualAxis m_horizontalVirtualAxis;  // Reference to the joystick in the cross platform input
    CrossPlatformInputManager.VirtualAxis m_verticalVirtualAxis;    // Reference to the joystick in the cross platform input
    CrossPlatformInputManager.VirtualButton m_jumpVirtualButton;    // Reference to the jump button in the cross platform input
    CrossPlatformInputManager.VirtualButton m_runVirtualButton;     // Reference to the run button in the cross platform input

    void Awake() {
        Debug.Log("VirtualGamepad.Awake()");
        m_horizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis("Horizontal");        
        m_verticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis("Vertical");
        m_jumpVirtualButton = new CrossPlatformInputManager.VirtualButton("Jump");
        m_runVirtualButton = new CrossPlatformInputManager.VirtualButton("Run");
    }

    void Start()
    {
        Debug.Log("VirtualGamepad.Start()");
    }

    private void OnEnable()
    {
        Debug.Log("VirtualGamepad.OnEnable()");

        /*
        if (CrossPlatformInputManager.AxisExists(m_horizontalVirtualAxis.name))
        {
            CrossPlatformInputManager.UnRegisterVirtualAxis(m_horizontalVirtualAxis.name);
        }
        */
        CrossPlatformInputManager.RegisterVirtualAxis(m_horizontalVirtualAxis);

        /*
        if (CrossPlatformInputManager.AxisExists(m_verticalVirtualAxis.name))
        {
            CrossPlatformInputManager.UnRegisterVirtualAxis(m_verticalVirtualAxis.name);
        }
        */
        CrossPlatformInputManager.RegisterVirtualAxis(m_verticalVirtualAxis);

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

        //CrossPlatformInputManager.SwitchActiveInputMethod(CrossPlatformInputManager.ActiveInputMethod.Touch);
    }

    private void OnDisable()
    {
        Debug.Log("VirtualGamepad.OnDisable()");
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

        var stickOffset = m_virtualDPadLeft.GetStickOffset();

        {
            var horizontal = stickOffset.x;
            m_horizontalVirtualAxis.Update(horizontal);
        }

        {
            var vertical = stickOffset.y;
            m_verticalVirtualAxis.Update(vertical);
        }        
    }
}
