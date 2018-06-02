using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WM.CameraNavigation.TranslationControl
{
    public abstract class TranslationControlBase : MonoBehaviour
    {
        public abstract void UpdateTranslation(GameObject gameObject);
    }
}
