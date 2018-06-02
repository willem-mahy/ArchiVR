using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.WM.CameraNavigation.TranslationControl
{
    public class TranslationControlFPS : TranslationControlBase
    {
        public FirstPersonController m_firstPersonController;

        public abstract class ICameraNavigationFPSTranslationInput
        {
            abstract public Vector3 GetTranslationVector();
            abstract public bool GetCrouch();
            abstract public bool GetJump();
            abstract public bool GetFastMovement();
        }

        // Use this for initialization
        void Start()
        {
            m_firstPersonController.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnDisable()
        {
            Debug.Log("TranslationControlFPS.OnDisable()");

            m_firstPersonController.m_MouseLook.lockCursor = false;
            
            //m_firstPersonController.gameObject.SetActive(false);
            m_firstPersonController.enabled = false;
        }

        void OnEnable()
        {
            Debug.Log("TranslationControlFPS.OnEnable()");

            //m_firstPersonController.gameObject.SetActive(true);
            m_firstPersonController.enabled = true;

            m_firstPersonController.m_MouseLook.lockCursor = true;
        }

        public override void UpdateTranslation(GameObject gameObject)
        {
            // NOOP: m_firstPersonController takes care of this.
        }
    }
}
