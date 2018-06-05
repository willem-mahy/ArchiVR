using UnityEngine;
using UnityEngine.UI;

namespace Assets.WM.UI
{
    public class ButtonCameraNavigationMode : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            var image = transform.Find("Image");

            if (image)
            {
                var imageComponent = image.GetComponent<Image>();

                if (imageComponent)
                {
                    var cameraNavigation = CameraNavigation.CameraNavigation.GetInstance();

                    if (cameraNavigation)
                    {
                        var activeNavigationMode = cameraNavigation.GetActiveNavigationMode();

                        var sprite = Resources.Load<Sprite>(activeNavigationMode.m_spritePath);

                        imageComponent.sprite = sprite;
                    }
                    else
                    {
                        imageComponent.sprite = null; 
                    }
                }
            }
        }
    }
}

