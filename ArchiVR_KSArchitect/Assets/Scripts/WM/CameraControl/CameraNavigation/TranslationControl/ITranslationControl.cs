using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WM.CameraControl.CameraNavigation.TranslationControl
{
    public interface ITranslationControl
    {
        void UpdateTranslation(GameObject gameObject);
    }
}
