using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.WM.UI;

public class Home_MainMenu : MonoBehaviour {

    //! The toggle button to set the view mode.
    public Button m_viewModeButton = null;

    // Use this for initialization
    void Start() {
        //InitViewModeButton();
    }

    void InitViewModeButton()
    { 
        var toggleButtonComponent = m_viewModeButton.GetComponent<ToggleButton>();

        //toggleButtonComponent.LoadOptions(viewModeOptionSpritePaths);

        m_viewModeButton.onClick.AddListener(ViewModeButton_OnClick);
    }
	
	void ViewModeButton_OnClick()
    {
        var toggleButtonComponent = m_viewModeButton.GetComponent<ToggleButton>();

        toggleButtonComponent.SetNextOption();


    }
}
