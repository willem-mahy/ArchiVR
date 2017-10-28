using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public static float m_offsetRotY;

    public Camera m_camera = null;

    // Use this for initialization
    public void Start()
    {
        Debug.Log("WMCameraRotate.Start()");
    }

    // Update is called once per frame   
    void Update()
    {
        //Debug.Log("WMCameraRotate.Update()");

        if (m_camera == null)
        {
            return;
        }

        UpdateCameraRotation();
   }

    protected virtual void UpdateCameraRotation()
    {
        Debug.Log("WMCameraRotate.UpdateCameraRotation()");
    }
}

public class CameraRotateByGyro : CameraRotate
{
    // Use this for initialization
    new public void Start()
    {
        base.Start();

        Debug.Log("CameraRotateByGyro.Start()");

        if (!SystemInfo.supportsGyroscope)
            return;

        Input.gyro.enabled = true;
    }

    protected override void UpdateCameraRotation()
    {
        //Debug.Log("WMCameraRotateByGyro.UpdateCameraRotation()");

        if (!SystemInfo.supportsGyroscope)
        {
            return;
        }

        Quaternion rotation = GetRotationFromGyro();

        if (m_offsetRotY != 0)
        {
            Quaternion r = Quaternion.Euler(0, m_offsetRotY, 0);
            rotation = r * rotation;
        }

        m_camera.transform.rotation = rotation;
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
}
