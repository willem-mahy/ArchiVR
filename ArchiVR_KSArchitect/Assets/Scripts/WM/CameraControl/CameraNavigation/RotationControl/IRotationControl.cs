using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WM.CameraControl.CameraNavigation.RotationControl
{
    public interface IRotationControl
    {
        void UpdateRotation(GameObject gameObject);
    }
}
