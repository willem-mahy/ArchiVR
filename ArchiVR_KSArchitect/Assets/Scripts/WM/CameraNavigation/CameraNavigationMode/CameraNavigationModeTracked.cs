using System;
using UnityEngine;

namespace Assets.Scripts.WM.CameraNavigation
{
    public class CameraNavigationModeTracked : CameraNavigationModeBase
    {
        override public void OnEnable()
        {
            Debug.Log("CameraNavigationModeTracked.OnEnable()");
            m_firstPersonController.enabled = false;
        }

        override public void OnDisable()
        {
            Debug.Log("CameraNavigationModeTracked.OnDisable()");
        }
    }
}
