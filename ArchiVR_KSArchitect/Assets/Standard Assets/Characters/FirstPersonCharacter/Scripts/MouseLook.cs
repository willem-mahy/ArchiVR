using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class MouseLook
    {
        public static bool IsPointerOverUIObject(Vector2 position)
        {
            Debug.Log("InputUtil.IsPointerOverUIObject(" + position + ")");

            // Referencing this code for GraphicRaycaster https://gist.github.com/stramit/ead7ca1f432f3c0f181f
            // the ray cast appears to require only eventData.position.
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = position;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            if (results.Count == 0)
            {
                Debug.Log("Not over UI Object.");
                return false;
            }

            foreach (var result in results)
            {
                Debug.Log("Over UI Object '" + result.gameObject.name + "'.");
            }

            foreach (var result in results)
            {
                if (result.gameObject.tag != "VRMenu")
                {
                    return true;
                }
            }

            return false;
        }

        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;


        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;
        private bool m_cursorIsLocked = true;

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }


        public void LookRotation(Transform character, Transform camera)
        {
            float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
            float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

            m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

            if(clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

            if(smooth)
            {
                character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }

            UpdateCursorLock();
        }

        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if(!lockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor)
                InternalLockUpdate();
        }

        private void InternalLockUpdate()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = false;
            }
            else if(Input.GetMouseButtonUp(0) && !IsPointerOverUIObject(Input.mousePosition))
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

            angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

    }

    [Serializable]
    public class GyroLook
    {
        public void Init(Transform character, Transform camera)
        {
        }

        public static Quaternion GetRotationFromGyro()
        {
            Quaternion att = Input.gyro.attitude;
            att.z = -att.z;
            att.w = -att.w;

            Quaternion rot = Quaternion.Euler(new Vector3(90, 0, 0));

            Quaternion rotation = rot * att;

            return rotation;
        }

        public void LookRotation(Transform character, Transform camera)
        {
            Quaternion rot = GetRotationFromGyro();
            
            Quaternion m_CharacterTargetRot  = Quaternion.Euler(0, rot.eulerAngles.y, 0);
            Quaternion m_CameraTargetRot = Quaternion.Euler(rot.eulerAngles.x, 0, 0);
            
            character.localRotation = m_CharacterTargetRot;
            camera.localRotation = m_CameraTargetRot;
        }
    }
}
