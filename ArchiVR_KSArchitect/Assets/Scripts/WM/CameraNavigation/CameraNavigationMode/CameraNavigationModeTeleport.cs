using System;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.Scripts.WM.CameraNavigation
{
    public class CameraNavigationModeTeleport : CameraNavigationModeBase
    {        
        override public void OnEnable()
        {
            Debug.Log("CameraNavigationModeTeleport.OnEnable()");

            // We will use the FPS controller only for camera rotation.
            m_firstPersonController.enabled = true;
            
            // Disable gravity effect on the FPS controller
            m_firstPersonController.m_GravityMultiplier = 0;
            m_firstPersonController.m_MoveDir.y = 0;
            m_firstPersonController.m_enableJump = true;

            // Disable translation
            m_firstPersonController.m_WalkSpeed = 0;
            m_firstPersonController.m_RunSpeed = 0;

            // Disable jumping/crouching
            //m_firstPersonController.m_jumpSpeed = 0;
            m_firstPersonController.m_MouseLook.lockCursor = true;

            m_firstPersonController.GetComponent<CharacterController>().enabled = true;
            
            // Position it.
            m_firstPersonController.gameObject.transform.localPosition = Vector3.up;
            m_firstPersonController.gameObject.transform.localRotation = Quaternion.identity;            
        }

        override public void OnDisable()
        {
            Debug.Log("CameraNavigationModeTeleport.OnDisable()");
        }
    }
}
