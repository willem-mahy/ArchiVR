using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.WM.CameraNavigation
{
    abstract public class CameraNavigationModeBase : MonoBehaviour
    {
        public string m_spritePath;

        public FirstPersonController m_firstPersonController;

        abstract public void OnEnable();

        abstract public void OnDisable();
    }
}
