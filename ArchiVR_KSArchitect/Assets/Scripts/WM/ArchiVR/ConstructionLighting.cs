using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.WM.ArchiVR
{
    public class ConstructionLightingModeBase
    {
        public string m_name = null;
        public Sprite m_sprite = null;

        // Reference to the root GameObject containing all construction lighting.
        private GameObject m_gameObjectWorldConstructionLighting = null;

        public ConstructionLightingModeBase(
            string name, string spritePath)
        {
            m_name = name;
            m_sprite = Resources.Load<Sprite>(spritePath);
        }

        public virtual void Start()
        {
        }

        public virtual void Update()
        {
        }

        // Get the root gameobject for construction lights.
        public GameObject GetGameObject_WorldConstructionLighting()
        {
            if (null == m_gameObjectWorldConstructionLighting)
            {
                // Search the root gameobject for construction lights in the first scene,
                // under the following path:
                // World/Construction/Lighting
                m_gameObjectWorldConstructionLighting = GameObject.Find("World/Construction/Phases/Final/Lighting");
            }

            return m_gameObjectWorldConstructionLighting;
        }
    }

    public class ConstructionLightingModeAuto : ConstructionLightingModeBase
    {
        // Reference to the 'Time' component.
        public TimeBehavior m_time = null;

        public ConstructionLightingModeAuto() : base("Auto", "Menu/LightMode/Construction/Auto")
        {
        }

        override public void Start()
        {
            m_time = GameObject.Find("Time").GetComponent<TimeBehavior>();
        }

        override public void Update()
        {
            var gameObjectWorldConstructionLighting = GetGameObject_WorldConstructionLighting();

            if (null == gameObjectWorldConstructionLighting)
            {
                return;
            }

            gameObjectWorldConstructionLighting.SetActive(GetAutoLightStateFromTime());
        }

        bool GetAutoLightStateFromTime()
        {
            if (m_time != null)
            {
                return (m_time.m_hour < 6 || m_time.m_hour > 18);
            }
            else
            {
                return true;
            }
        }
    }
    
    public class ConstructionLightingModeOff : ConstructionLightingModeBase
    {
        public ConstructionLightingModeOff() : base("Off", "Menu/LightMode/Construction/Off")
        {
        }

        public override void Start()
        {
            var gameObjectWorldConstructionLighting = GetGameObject_WorldConstructionLighting();

            if (null == gameObjectWorldConstructionLighting)
            {
                return;
            }

            gameObjectWorldConstructionLighting.SetActive(false);
        }
    }

    public class ConstructionLightingModeOn : ConstructionLightingModeBase
    {
        public ConstructionLightingModeOn() : base("On", "Menu/LightMode/Construction/On")
        {
        }

        public override void Start()
        {
            var gameObjectWorldConstructionLighting = GetGameObject_WorldConstructionLighting();

            if (null == gameObjectWorldConstructionLighting)
            {
                return;
            }

            gameObjectWorldConstructionLighting.SetActive(true);
        }
    }

    public class ConstructionLighting : MonoBehaviour
    {
        // The initial active lighting mode
        public int m_initialActiveModeIndex = 0;
        
        // The list of construction lighting modes.
        private List<ConstructionLightingModeBase> m_lightingModeList = new List<ConstructionLightingModeBase>();

        // The current active lighting mode.
        private int m_activeModeIndex = -1;

        private void Awake()
        {
            m_lightingModeList.Add(new ConstructionLightingModeAuto());
            m_lightingModeList.Add(new ConstructionLightingModeOn());
            m_lightingModeList.Add(new ConstructionLightingModeOff());
        }

        // Use this for initialization
        void Start()
        {
            ActivateLightingMode(m_initialActiveModeIndex);
        }        

        // Update is called once per frame
        void Update()
        {
            if (m_activeModeIndex >= 0)
                m_lightingModeList[m_activeModeIndex].Update();
        }

        public void ActivateNextLightingMode()
        {
            ActivateLightingMode(m_activeModeIndex + 1);
        }

        public void ActivatePreviousLightingMode()
        {
            ActivateLightingMode(m_activeModeIndex - 1);
        }

        int MakeValidLightingMode(int newActiveLightingMode)
        {
            if (m_lightingModeList.Count == 0)
            {
                return -1;
            }

            return newActiveLightingMode % m_lightingModeList.Count;
        }

        public void ActivateLightingMode(int newActiveLightingMode)
        {
            m_activeModeIndex = MakeValidLightingMode(newActiveLightingMode);

            if (m_activeModeIndex >= 0)
            {
                m_lightingModeList[m_activeModeIndex].Start();
            }
        }

        public ConstructionLightingModeBase GetActiveMode()
        {
            return (m_activeModeIndex < 0) ? null : m_lightingModeList[m_activeModeIndex];
        }
    }
}
