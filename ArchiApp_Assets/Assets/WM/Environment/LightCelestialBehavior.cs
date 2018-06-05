using Assets.WM.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.WM.Environment
{
    public class LightCelestialBehavior : MonoBehaviour
    {
        public float m_timeMaxAzimuth = 0.0f;

        public TimeBehavior m_time = null;

        public GameObject m_body = null;
        public Text m_textDebugEnvironment = null;
        public Camera m_camera = null;

        public List<Color> m_timedColorsLight = new List<Color>();

        public List<Color> m_timedColorsSky1 = new List<Color>();
        public List<Color> m_timedColorsSky2 = new List<Color>();

        float m_maxIntensity = 0.5f;

        public Light m_light = null;

        // Use this for initialization
        void Start()
        {
            m_maxIntensity = m_light.intensity;
        }

        // Update is called once per frame
        void Update()
        {
            // Update position of celestial object.
            //float angleNormalized = (m_time.m_hour + m_time.m_fractionOfHour) / 24.0f;        
            //float angleDegrees = -90.0f + (angleNormalized * 360.0f);
            //gameObject.transform.localRotation = Quaternion.Euler(0, -90.0f, 0);
            //gameObject.transform.Rotate(angleDegrees * Vector3.up, Space.Self);

            float timeInHours = m_time.m_hour + m_time.m_fractionOfHour;
            float angleNormalized = ((timeInHours - m_timeMaxAzimuth) % 24.0f) / 24.0f;
            float angleDegrees = (angleNormalized * 360.0f) % 360.0f;

            gameObject.transform.localEulerAngles = new Vector3(0, angleDegrees, 0);

            float azimuth = -m_light.transform.forward.y;

            // Update celestial object light intensity
            {
                float intensity = azimuth;

                // Clamp to [0, 1]
                intensity = Mathf.Max(intensity, 0);
                intensity = Mathf.Min(intensity, 1);

                m_light.intensity = intensity * m_maxIntensity;
            }

            if (m_textDebugEnvironment)
            {
                // Show on screen:
                // Title
                // - light intensity
                //- azimuth

                m_textDebugEnvironment.text = "Sky light '" + gameObject.name + "'\n";
                m_textDebugEnvironment.text += "- intensity      : " + m_light.intensity + "\n";
                m_textDebugEnvironment.text += "- azimuth        : " + azimuth;
                m_textDebugEnvironment.text += "- max intensity :" + m_maxIntensity;
            }

            if (m_camera != null)
            {
                m_light.transform.position = m_camera.transform.position;
            }

            if (m_timedColorsLight.Count > 0)
            {
                var h = m_time.m_hour;
                var nh = m_time.m_nextHour;
                var foh = m_time.m_fractionOfHour;

                try
                {
                    Color colorLight = Color.Lerp(m_timedColorsLight[h], m_timedColorsLight[nh], foh); // Compute interpolated current color.

                    Color colorSky1 = Color.Lerp(m_timedColorsSky1[h], m_timedColorsSky1[nh], foh); // Compute interpolated current color.
                    Color colorSky2 = Color.Lerp(m_timedColorsSky2[h], m_timedColorsSky2[nh], foh); // Compute interpolated current color.

                    // Update celestial light
                    if (m_light)
                    {
                        // Most important: is it to be enabled at all?
                        m_light.enabled = (azimuth > 0);

                        m_light.color = colorLight;

                        float ambientLightIntensity = m_light.intensity;

                        if (true)
                        {
                            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;

                            //RenderSettings.ambientLight = colorSky;
                            RenderSettings.ambientLight = Color.gray;
                            //RenderSettings.ambientLight = new Color(ambientLightIntensity, ambientLightIntensity, ambientLightIntensity);

                            RenderSettings.ambientIntensity = ambientLightIntensity;
                        }
                    }

                    // Update celestial body's color.

                    //if (false)
                    if (m_body)
                    {
                        Material material = m_body.GetComponent<Renderer>().material;

                        //if (false)
                        if (material)
                        {
                            // Set it on material
                            material.color = colorLight;

                            if (true)
                            {
                                material.shader = Shader.Find("Unlit/Sun");

                                if (material.shader)
                                {
                                    // Set it on material shader float vars

                                    {
                                        material.SetColor("_Color", colorLight);
                                    }
                                }
                            }
                        }
                    }

                    // Update skydome colors
                    var sd = GameObject.Find("SkyDome");

                    if (sd)
                    {
                        Material material = sd.GetComponent<Renderer>().material;
                        material.shader = Shader.Find("Unlit/SkyDome");

                        if (material.shader)
                        {
                            // Set it on material shader float vars

                            {
                                material.SetColor("_SkyColor1", colorSky1);
                                material.SetColor("_SkyColor2", colorSky2);

                                var lightDirection = -m_light.transform.forward;
                                var lightDirectionColor = new Color(lightDirection.x, lightDirection.y, lightDirection.z, 0);
                                material.SetColor("_LightDirection", lightDirectionColor);
                            }
                        }
                    }

                    // Update background clear color
                    m_camera.backgroundColor = colorSky1;
                }
                catch (Exception e)
                {
                    Debug.Log(gameObject.name + " " + h + " " + nh + " Exception message=" + e.Message);
                }
            }
        }
    }
}