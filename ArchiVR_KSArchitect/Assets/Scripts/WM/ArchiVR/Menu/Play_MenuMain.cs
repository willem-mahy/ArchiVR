using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.WM.CameraNavigation;
using Assets.Scripts.WM.ArchiVR.Application;
using Assets.Scripts.WM.UI;
using Assets.Scripts.WM.Settings;

public class Play_MenuMain : MonoBehaviour {

    public CameraNavigation m_cameraNavigation = null;

    public Button m_buttonCameraNavigationMode = null;

    public Button m_buttonConstructionLightingMode = null;

    public Button m_buttonXRDevice = null;

    public ApplicationState m_applicationState = null;

    public Widget m_widgetVirtualGamepad = null;

    public Widget m_widgetMenuPOI = null;

    public bool ActiveXRDevice_IsOnScreenUISupported()
    {
        return (!UnityEngine.XR.XRDevice.isPresent);
    }

    // Update is called once per frame
    void Update () {

        // Update 'Construction Lighting Mode' button.
        {
            var constructionLighting = GameObject.Find("Application").GetComponent<Assets.Scripts.WM.ArchiVR.ConstructionLighting>();
            var constructionLightingMode = constructionLighting.GetActiveMode();
            var sprite = (null == constructionLightingMode) ? null : constructionLightingMode.m_sprite;
            m_buttonConstructionLightingMode.transform.Find("Image").GetComponent<Image>().sprite = sprite;
        }

        // Update 'XRDevice' button.
        int numAvailableXRDevices = m_applicationState.GetAvailableXRDeviceNameList().Count;
        m_buttonXRDevice.interactable = 
            //false;<
            (numAvailableXRDevices > 1);

        {
            var loadedXRDeviceName = UnityEngine.XR.XRSettings.loadedDeviceName;

            if ("" == loadedXRDeviceName)
                loadedXRDeviceName = "none";

            var spritePath = "Menu/ViewMode/" + loadedXRDeviceName;
            var sprite = Resources.Load<Sprite>(spritePath);
            m_buttonXRDevice.transform.Find("Image").GetComponent<Image>().sprite = sprite;
        }

        // Update POI Menu visibility.
        var enablePOI = false;

        var cn = CameraNavigation.GetInstance();

        var cnm = (cn ? cn.GetActiveNavigationMode() : null);

        if (cnm)
        {
            var cnmSupportsPOI = cnm ? cnm.SupportsNavigationViaPOI() : false;

            enablePOI = cnmSupportsPOI;
        }

        //TODO:
        m_widgetMenuPOI.SetVisible(enablePOI);

        var asd = ApplicationSettings.GetInstance().m_data;

        var enableVirtualGamepad =
            ActiveXRDevice_IsOnScreenUISupported()
            && asd.m_stateSettings.m_enableVirtualGamepad;

        m_widgetVirtualGamepad.SetVisible(enableVirtualGamepad);
    }
}
