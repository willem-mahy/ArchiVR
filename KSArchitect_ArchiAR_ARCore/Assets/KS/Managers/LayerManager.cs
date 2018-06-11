using System.Collections.Generic;
using UnityEngine;
using KS.Entities;

namespace KS.Managers
{
    public class LayerManager : MonoBehaviour
    {
        static private string s_layerNamePrefix = "Layer_";

        private Dictionary<string, Layer> m_layers = new Dictionary<string, Layer>();

        static private LayerManager s_instance = null;

        //! Get a reference to the singleton instance.
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

        public void SetLayerVisible(
            string layerName,
            bool visible)
        {
            var layer = m_layers[layerName];

            if (null == layer)
            {
                Debug.LogWarning("Layer '" + layerName + "' not found!");
                return;
            }

            layer.SetVisible(visible);
        }

        private Layer GetOrAddLayer(string layerName)
        {
            if (!m_layers.ContainsKey(layerName))
            {
                m_layers[layerName] = new Layer(layerName, new List<GameObject>(), true);
            }

            return m_layers[layerName];
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
                    var layerName = go.name.Remove(0, s_layerNamePrefix.Length);

                    var layer = GetOrAddLayer(layerName);

                    layer.Add(go);
                }
            }
        }

        public Dictionary<string, Layer>.ValueCollection GetLayers()
        {
            return m_layers.Values;
        }

        public void SetAllLayersVisible(bool visible)
        {
            foreach (var layer in GetLayers())
            {
                layer.SetVisible(true);
            }
        }

        public bool AreAllLayersVisible()
        {
            foreach (var layer in GetLayers())
            {
                if (!layer.IsVisible())
                {
                    return false;
                }
            }

            return true;
        }

        public bool AreAllLayersInvisible()
        {
            foreach (var layer in GetLayers())
            {
                if (layer.IsVisible())
                {
                    return false;
                }
            }

            return true;
        }
    }
}