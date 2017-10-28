using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.WM.Util;

public class CameraFlyBase : MonoBehaviour
{
    static private void LogKeyPress(string keyName)
    {
        if (Input.GetKeyDown(keyName))
        {
            UnityEngine.Debug.Log(keyName + " key was pressed");
        }
    }

    static float xSpeed = 100.0f;
    static float ySpeed = 100.0f;

    static float xRotMin = -90.0f;
    static float xRotMax = 90;

    public bool m_doDebug = false;

    public Text m_textDebug = null;

    public Camera m_camera = null;

    public float m_translateSpeed = 1.0f;   

    // Use this for initialization
    void Start () {
		
	}   

	// Update is called once per frame
	public virtual void Update()
    {
        LogKeyPress("space");
        LogKeyPress("left");
        LogKeyPress("right");
        LogKeyPress("up");
        LogKeyPress("space");
    }

    protected void TranslateXZ(Vector2 offsetXZ)
    {
        //offsetXZ.Normalize();

        Vector3 cameraMovement_Camera = new Vector3(offsetXZ.x, 0, offsetXZ.y);
        Vector3 cameraMovement = m_camera.transform.localToWorldMatrix.MultiplyVector(cameraMovement_Camera);

        bool m_fixY = true;

        if (m_fixY)
        {
            // Project movement vector into XZ plane.
            cameraMovement.y = 0;

            if (cameraMovement.sqrMagnitude < 0.01)
            {
                cameraMovement = Vector3.zero;
            }
            else
            {
                Vector3 cameraMovementDir = cameraMovement;
                cameraMovementDir.Normalize();
                
                cameraMovement = cameraMovementDir * offsetXZ.magnitude;

                // Extrapolate in time.
                cameraMovement*= m_translateSpeed * Time.deltaTime;
            }
        }

        m_camera.transform.position = m_camera.transform.position + cameraMovement;
    }

    protected void TranslateY(float offsetY)
    {
        var p = m_camera.transform.position;
        p.y += offsetY * m_translateSpeed * Time.deltaTime;
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
