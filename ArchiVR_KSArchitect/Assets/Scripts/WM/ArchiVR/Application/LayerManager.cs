using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WM
{
    public class LayerManager : MonoBehaviour
    {
        static private string s_layerNamePrefix = "Layer_";

        private Dictionary<string, List<GameObject>> m_layers = new Dictionary<string, List<GameObject>>();

        static private LayerManager s_instance = null;

        static public LayerManager GetInstance()
        {            
            return s_instance;
        }

        private void Awake()
        {
            s_instance = this;

            DynamicallyCreateLayers();
        }

        // Use this for initialization
        void Start()
        {            
        }

        // Update is called once per frame
        void Update()
        {            
        }

        public void DynamicallyCreateLayers()
        {
            // First clear the list of layers.
            m_layers.Clear();

            // Then repopulate the list of layers.
            var allGameObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (var go in allGameObjects)
            {
                if (go.name.StartsWith(s_layerNamePrefix))
                {
                    var layerName = go.name;

                    layerName = layerName.Remove(0, s_layerNamePrefix.Length);

                    if (!m_layers.ContainsKey(layerName))
                    {
                        m_layers[layerName] = new List<GameObject>();
                    }

                    m_layers[layerName].Add(go);
                }
            }
        }

        public Dictionary<string, List<GameObject>> GetLayers()
        {
            return m_layers;
        }
    }
}