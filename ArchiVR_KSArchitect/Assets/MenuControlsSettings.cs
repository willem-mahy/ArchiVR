using Assets.Scripts.WM.Settings;
using Assets.Scripts.WM.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControlsSettings : MonoBehaviour {
    //! The 'Enable Onscreen Gamepad' button.
    public Button m_enableOnscreenGamepadButton = null;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var s = ApplicationSettings.GetInstance().m_data.m_controlSettings;

        m_enableOnscreenGamepadButton.GetComponent<CheckBox>().SetCheckedState(s.m_enableVirtualGamepad);

    }
}
