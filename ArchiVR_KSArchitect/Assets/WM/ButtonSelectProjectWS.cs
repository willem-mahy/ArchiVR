using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelectProjectWS : MonoBehaviour {

    public ProjectSelectButton m_button = null;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public ProjectSelectButton GetButton()
    {
        return m_button;
    }
}
