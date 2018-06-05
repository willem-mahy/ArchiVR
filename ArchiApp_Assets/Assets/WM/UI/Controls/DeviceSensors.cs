using System.Collections.Generic;
using UnityEngine;

public class Frame
{
    public float m_time;

    public float m_heading;

    public Vector3 m_accelerometer;
}


public class WMDeviceSensors : MonoBehaviour {

    public static float m_offsetRotY;
    public UnityEngine.UI.Text m_textCompassTrueHeading = null;
    public UnityEngine.UI.Text m_textSmoothHeading = null;
    public UnityEngine.UI.Text m_textAccelerometer = null;
    public UnityEngine.UI.Text m_textGyro = null;
    public UnityEngine.UI.Text m_textPosition = null;
    public UnityEngine.UI.Text m_textNumFrames = null;

    public UnityEngine.Camera m_camera = null;

    private List<Frame> m_frames = new List<Frame>();

    const float m_smoothHorizon = 0.5f;
    const float m_weightIncrement = 0.1f;

    // Use this for initialization
    void Start()
    {
        if (!SystemInfo.supportsGyroscope)
            return;

        Input.gyro.enabled = true;
        Input.compass.enabled = true;
        Input.location.Start();
    }

    public static Quaternion GetRotationFromGyro()
    {
        Quaternion att = Input.gyro.attitude;
        att.z = -att.z;
        att.w = -att.w;

        Quaternion rot = Quaternion.Euler(new Vector3(90, 0, 0));

        Quaternion rotation = rot* att;

        return rotation;
    }

    // Update is called once per frame   
    void Update()
    {        
        if (!SystemInfo.supportsGyroscope)
        {
            return;
        }

        Frame frame = new Frame();

        frame.m_time = Time.time;
        frame.m_accelerometer = Input.acceleration;
        frame.m_heading = Input.compass.trueHeading;

        m_frames.Add(frame);
        
        // Remove out-dated frames.
        for (var i = 0; i < m_frames.Count; ++i)
        {
            float frameAge = Time.time - m_frames[i].m_time;

            if (frameAge <= m_smoothHorizon)
            {
                m_frames.RemoveRange(0, i);
                break;
            }
        }

        float headingSmooth = 0.0f;
        
        if (m_frames.Count != 0)
        {
            // Compute smoot heading angle from most recent frames.
            float weight = 1;
                
            Vector2 unitPos = new Vector2(0, 0);
            for (var i = 0; i < m_frames.Count; ++i)
            {
                unitPos.x += weight * Mathf.Sin(frame.m_heading * Mathf.PI / 180.0f);
                unitPos.y += weight * Mathf.Cos(frame.m_heading * Mathf.PI / 180.0f);

                weight = weight + m_weightIncrement;
            }

            unitPos.Normalize();
            headingSmooth = Mathf.Atan2(unitPos.x, unitPos.y) * 180.0f / Mathf.PI;
        }
        else
        {
            headingSmooth = Input.compass.trueHeading;
        }        

        if (m_camera)
        {
            /*
            if (Application.isEditor)
            {
                if (Input.GetMouseButton(0))
                {
                    var eulerAngles = m_camera.transform.eulerAngles;

                    // Mouse drag over X axis = camera rotation around Y axis.
                    eulerAngles.x -= Input.GetAxis("Mouse Y") * xSpeed * Time.deltaTime;
                    // Mouse drag over Y axis = camera rotation around X axis.
                    eulerAngles.y += Input.GetAxis("Mouse X") * ySpeed * Time.deltaTime;

                    eulerAngles.x = WMMath.FormatAngle180(eulerAngles.x);
                    eulerAngles.x = Mathf.Clamp(eulerAngles.x, xRotMin, xRotMax);
                    
                    var rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, 0);
                    m_camera.transform.rotation = rotation;
                }
            }
            else
            {
            */
            Quaternion rotation = GetRotationFromGyro();

            if (m_offsetRotY != 0)
            {
                Quaternion r = Quaternion.Euler(0, m_offsetRotY, 0);
                rotation = r * rotation;
            }

            m_camera.transform.rotation = rotation;
            //}
        }

        if (m_textSmoothHeading != null)
        {
            m_textSmoothHeading.text = "Smooth Heading: " + headingSmooth;
        }

        if (m_textCompassTrueHeading != null)
        {
            m_textCompassTrueHeading.text = "Compass True Heading: " + Input.compass.trueHeading.ToString();
        }

        if (m_textNumFrames != null)
        {
            m_textNumFrames.text = "# Frames: " + m_frames.Count.ToString();
        }

        if (m_textAccelerometer != null)
        {
            m_textAccelerometer.text = "AcceleroMeter: " + Input.acceleration.ToString();
        }

        if (m_textGyro != null)
        {
            m_textGyro.text = "Gyro Attitude: " + Input.gyro.attitude.ToString();
        }

        if (m_textPosition != null)
        {
            m_textPosition.text = "Position: " + m_camera.transform.position;
        }

        if (m_textNumFrames != null)
        {
            m_textNumFrames.text = "m_offsetRotY: " + m_offsetRotY;
        }
    }
}
