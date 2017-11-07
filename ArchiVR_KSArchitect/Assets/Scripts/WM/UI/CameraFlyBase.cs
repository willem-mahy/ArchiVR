using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.WM.Util;

public class CameraFlyBase : MonoBehaviour
{
    static float xSpeed = 100.0f;
    static float ySpeed = 100.0f;

    static float xRotMin = -90.0f;
    static float xRotMax = 90;

    public bool m_doDebug = false;

    public Text m_textDebug = null;

    //! The camera to be controlled.
    public Camera m_camera = null;

    //! The normal translation speed, in units/sec.
    public float m_translateSpeedNormal = 20.0f;

    //! The normal translation speed, in units/sec.
    public float m_translateSpeedFast = 50.0f;

    // Update is called once per frame
	public virtual void Update()
    {
        //DebugUtil.LogKeyPress("space");
        //DebugUtil.LogKeyPress("left");
        //DebugUtil.LogKeyPress("right");
        //DebugUtil.LogKeyPress("up");
        //DebugUtil.LogKeyPress("down");
    }

    /*
     * cameraMovementDirXZ_World_Vector2    The translation direction, expressed in the Camera local frame, and encoded in a 2d vector (X=X, Y=Z).
     * offset                               The magnitude of the translation.
     * bool fixY                            Whether to constrain movement to the world XZ plane.
     */
    protected void TranslateXZ(
        Vector2 cameraMovementDirXZ_Camera_Vector2,
        float offset,
        bool fixY)
    {
        // Put the movement direction into a 3D Vector.
        Vector3 cameraMovementDirection_Camera = new Vector3(cameraMovementDirXZ_Camera_Vector2.x, 0, cameraMovementDirXZ_Camera_Vector2.y);

        // Express the movement direction in the Camera's local frame.
        Vector3 cameraMovementDirection_World = m_camera.transform.localToWorldMatrix.MultiplyVector(cameraMovementDirection_Camera);

        if (fixY)
        {
            // Project movement vector into Camera local frame's XZ plane.
            cameraMovementDirection_World.y = 0;

            if (cameraMovementDirection_World.sqrMagnitude < 0.01)
            {
                return; // no movement
            }
        }

        // Normalize the movement dir.
        cameraMovementDirection_World.Normalize();

        // Compute the 3D translation offset with magnitude.
        Vector3 cameraMovement_World = cameraMovementDirection_World * offset;

        m_camera.transform.position = m_camera.transform.position + cameraMovement_World;
    }

    protected void TranslateY(
        float offset)
    {
        var p = m_camera.transform.position;
        p.y += offset;
        m_camera.transform.position = p;
    }

    protected void Rotate(Vector3 eulerOffset)
    {
        // Mouse drag over X axis = camera rotation around Y axis.
        eulerOffset.x *= xSpeed * Time.deltaTime;
        // Mouse drag over Y axis = camera rotation around X axis.
        eulerOffset.y *= ySpeed * Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            var cameraEulerAngles = m_camera.transform.eulerAngles;

            // Mouse drag over X axis = camera rotation around Y axis.
            cameraEulerAngles.x -= Input.GetAxis("Mouse Y") * xSpeed * Time.deltaTime;
            // Mouse drag over Y axis = camera rotation around X axis.
            cameraEulerAngles.y += Input.GetAxis("Mouse X") * ySpeed * Time.deltaTime;
            
            cameraEulerAngles.x = Math.FormatAngle180(cameraEulerAngles.x);
            cameraEulerAngles.x = Mathf.Clamp(cameraEulerAngles.x, xRotMin, xRotMax);

            var rotation = Quaternion.Euler(cameraEulerAngles.x, cameraEulerAngles.y, 0);
            m_camera.transform.rotation = rotation;
        }
    }
}
