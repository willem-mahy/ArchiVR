using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.WM.CameraNavigation;

public class Play_MenuMain : MonoBehaviour {

    public CameraNavigation m_cameraNavigation = null;

    public Button m_buttonCameraNavigationMode = null;

    public Button m_buttonConstructionLightingMode = null;
        
	// Update is called once per frame
	void Update () {
        // Update 'Camera Navigation Mode' button.
        {
            var sprite = Resources.Load<Sprite>(m_cameraNavigation.GetActiveNavigationMode().m_spritePath);
            m_buttonCameraNavigationMode.transform.Find("Image").GetComponent<Image>().sprite = sprite;
        }

        // Update 'Construction Lighting Mode' button.
        {
            var constructionLighting = GameObject.Find("Application").GetComponent<Assets.Scripts.WM.ArchiVR.ConstructionLighting>();
            var constructionLightingMode = constructionLighting.GetActiveMode();
            var sprite = (null == constructionLightingMode) ? null : constructionLightingMode.m_sprite;
            m_buttonConstructionLightingMode.transform.Find("Image").GetComponent<Image>().sprite = sprite;
        }
    }
}
