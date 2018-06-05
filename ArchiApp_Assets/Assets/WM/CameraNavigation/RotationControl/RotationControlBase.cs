using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.WM.CameraNavigation.RotationControl
{
    public abstract class RotationControlBase : MonoBehaviour
    {
        public abstract void UpdateRotation(GameObject gameObject);
    }
}
