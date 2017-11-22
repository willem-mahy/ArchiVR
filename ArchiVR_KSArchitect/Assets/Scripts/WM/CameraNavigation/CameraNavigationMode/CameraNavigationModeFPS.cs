using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.WM.CameraNavigation
{
    public class CameraNavigationModeFPS : CameraNavigationModeBase
    {
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
        }

        override public void OnEnable()
        {
            Debug.Log("CameraNavigationModeFPS.OnEnable()");

            m_firstPersonController.enabled = true;

            m_firstPersonController.m_MouseLook.lockCursor = true;

            m_firstPersonController.m_WalkSpeed = 5;
            m_firstPersonController.m_RunSpeed = 10;
            m_firstPersonController.m_GravityMultiplier = 2;
            m_firstPersonController.m_enableJump = true;

            m_firstPersonController.m_MouseLook.lockCursor = true;

            m_firstPersonController.GetComponent<CharacterController>().enabled = true;
        }

        override public void OnDisable()
        {
            Debug.Log("CameraNavigationModeFPS.OnDisable()");
        }
    }
}
