using Assets.ArchiApp.Application;
using Assets.WM.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.WM.Script.UI.Menu
{
    public class Play_MenuMain : WMMenu
    {
        public CameraNavigation.CameraNavigation m_cameraNavigation = null;

        public Button m_buttonHome = null;

        public Button m_buttonCameraNavigationMode = null;

        public Button m_buttonConstructionLightingMode = null;

        public Button m_buttonXRDevice = null;

        public ApplicationState m_applicationState = null;

        // Update is called once per frame
        public new void Update()
        {
            base.Update();

            // Update 'Construction Lighting Mode' button.
            {
                var constructionLighting = GameObject.Find("Application").GetComponent<Assets.ArchiApp.Application.Entities.ConstructionLighting>();
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
        }
    }
}
