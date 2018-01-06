using Assets.Scripts.WM.Settings;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour {
    public enum Mode
    {
        Auto = 0,
        Fixed,
        NumModes // Do not remove and keep as last!
    }

    public GameObject m_clouds = null;

    public List<ParticleSystem> m_cloudLayers = new List<ParticleSystem>();
    
    static private CloudManager s_instance = null;

    //! Get a reference to the singleton instance.
    static public CloudManager GetInstance()
    {
        return s_instance;
    }

    private void Awake()
    {
        s_instance = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        var s = ApplicationSettings.GetInstance().m_data.m_graphicSettings;

        if (s.m_cloudsMode == Mode.Auto)
        {
            SetCloudIntensity(Mathf.Abs(Mathf.Sin(180.0f * Time.time)));
        }
	}

    public void ToggleEnableClouds()
    {
        var s = ApplicationSettings.GetInstance().m_data.m_graphicSettings;
        s.m_enableClouds = !s.m_enableClouds;

        m_clouds.SetActive(s.m_enableClouds);
    }

    public void SetMode(Mode mode)
    {
        var s = ApplicationSettings.GetInstance().m_data.m_graphicSettings;
        s.m_cloudsMode = mode;

        if (mode == Mode.Fixed)
        {
            SetCloudIntensity(s.m_fixedCloudIntensity);
        }
    }

    public void SetFixedCloudIntensity(float intensity)
    {
        var s = ApplicationSettings.GetInstance().m_data.m_graphicSettings;

        s.m_fixedCloudIntensity = intensity;

        if (s.m_cloudsMode == Mode.Fixed)
        {
            SetCloudIntensity(s.m_fixedCloudIntensity);
        }
    }

    public void SetCloudIntensity(float intensity)
    {
        float m_maxNumParticlesAtFullIntensity = 1000;

        int numParticles = (int)Mathf.Floor(intensity * m_maxNumParticlesAtFullIntensity);

        foreach (var cloudLayer in m_cloudLayers)
        {
            var main = cloudLayer.main;
            main.maxParticles = numParticles;
        }
    }
}
