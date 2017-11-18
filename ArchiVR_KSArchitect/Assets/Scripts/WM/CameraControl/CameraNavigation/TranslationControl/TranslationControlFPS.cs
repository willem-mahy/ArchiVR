using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WM.CameraControl.CameraNavigation.TranslationControl
{
    public class TranslationControlFPS : ITranslationControl
    {
        public abstract class ICameraNavigationFPSTranslationInput : MonoBehaviour
        {
            abstract public Vector3 GetTranslationVector();
            abstract public bool GetCrouch();
            abstract public bool GetJump();
            abstract public bool GetFastMovement();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateTranslation(GameObject gameObject)
        {
            throw new NotImplementedException();
        }
    }
}
