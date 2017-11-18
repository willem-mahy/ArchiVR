using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WM.CameraControl.CameraNavigation
{
    public abstract class CameraNavigationBase
    {
        public Camera m_camera = null;

        // Use this for initialization
        public virtual void Awake()
        {
            if (null == m_camera)
            {
                Debug.LogWarning("m_camera == null!");
            }
        }

        // Update is called once per frame
        public virtual void Update()
        {

        }
    }
}
