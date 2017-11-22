using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.WM.CameraNavigation;

public class Play_MenuMain : MonoBehaviour {

    public CameraNavigation m_cameraNavigation = null;

    public Button m_buttonCameraNavigationMode = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var sprite = Resources.Load<Sprite>(m_cameraNavigation.GetActiveNavigationMode().m_spritePath);
        m_buttonCameraNavigationMode.transform.Find("Image").GetComponent<Image>().sprite = sprite;
    }
}
