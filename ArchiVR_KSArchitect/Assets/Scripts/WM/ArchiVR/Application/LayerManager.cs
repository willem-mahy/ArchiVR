using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WM
{
    public class Layer
    {
        //! The layer name.
        private string m_name;

        //! Flags whether the layer is visible or not.
        private bool m_isVisible = true;

        //! The list of GameObjects contained in the layer.
        private List<GameObject> m_gameObjectList;

        public Layer(
            string name,
            List<GameObject> gameObjectList,
            bool visible)
        {
            m_name = name;
            m_gameObjectList = gameObjectList;

            SetVisible(visible);
        }

        public string GetName()
        {
            return m_name;
        }

        public bool IsVisible()
        {            
            return m_isVisible;
        }

        public void SetVisible(bool visible)
        {
            m_isVisible = visible;

            UpdateGameObjectActiveState();
        }

        private void UpdateGameObjectActiveState()
        {
            if (null == m_gameObjectList)
            {
                return;
            }

            if (0 == m_gameObjectList.Count)
            {
                return;
            }

            foreach (var go in m_gameObjectList)
            {
                go.SetActive(m_isVisible);
            }
        }

        public void ToggleVisible()
        {
            SetVisible(!m_isVisible);
        }

        public void Add(GameObject go)
        {
            m_gameObjectList.Add(go);
            go.SetActive(m_isVisible);
        }
    }

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

                    m_layers[layerName].Add(go);
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
    }
}