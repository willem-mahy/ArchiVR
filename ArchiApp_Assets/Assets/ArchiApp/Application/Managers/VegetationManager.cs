using Assets.WM.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.ArchiApp.Application.Managers
{
    public class VegetationManager : MonoBehaviour
    {

        private static VegetationManager s_instance = null;
        public List<Terrain> m_terrains = new List<Terrain>();

        static public VegetationManager GetInstance()
        {
            return s_instance;
        }

        void Awake()
        {
            s_instance = this;
        }

        // Use this for initialization
        void Start()
        {
        }

        public void InitTerrains()
        {
            //m_terrains.AddRange(GameObject.FindObjectsOfType<Terrain>());
        }

        // Update is called once per frame
        void Update()
        {
            var s = ApplicationSettings.GetInstance().m_data.m_graphicSettings;

            if (null != s)
            {
                foreach (var terrain in m_terrains)
                {
                    if (null != terrain)
                    {
                        //terrain.enabled = s.m_enableDynamicGrass;
                        terrain.drawTreesAndFoliage = s.m_enableDynamicGrass;
                    }
                }
            }
        }
    }
}