using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WM
{
    public class LayerManager : MonoBehaviour
    {
        public GameObject m_layerMenuPanel = null;

        public GameObject m_buttonPrefab = null;

        private string m_layerNamePrefix = "Layer_";

        private Dictionary<string, List<GameObject>> m_layers = new Dictionary<string, List<GameObject>>();


        private void Awake()
        {
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (m_layers.Count == 0)
            {
                DynamicallyCreateLayerButtons();
            }
        }

        void DynamicallyCreateLayerButtons()
        {
            var allGameObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (var go in allGameObjects)
            {
                if (go.name.StartsWith(m_layerNamePrefix))
                {
                    var layerName = go.name;

                    if (!m_layers.ContainsKey(layerName))
                    {
                        m_layers[layerName] = new List<GameObject>();
                    }

                    m_layers[layerName].Add(go);
                }
            }

            float x = 100;
            float y = -100;
            float yStep = -150;

            foreach (var layerEntry in m_layers)
            {
                GameObject button = DynamicallyAddButton(m_layerMenuPanel, layerEntry.Value[0].name);

                if (null == button)
                {
                    Debug.LogError("button == null");
                    continue;
                }

                button.transform.localPosition = new Vector3(x, y, 0);

                y += yStep;

                var layerButtonComponent = button.GetComponent<LayerButton>();

                if (null == layerButtonComponent)
                {
                    Debug.LogError("button == null");
                    continue;
                }

                layerButtonComponent.m_layerName = layerEntry.Key.Remove(0, m_layerNamePrefix.Length);
                layerButtonComponent.m_layerGameObjectList = layerEntry.Value;                
            }
        }

        private GameObject DynamicallyAddButton(
            GameObject buttonParent,
            string layerName)
        {
            var button = (GameObject)Instantiate(m_buttonPrefab);

            if (null == button)
            {
                Debug.LogError("button = null");
                return null;
            }

            button.name = "LayerButton_" + layerName;

            // Set button parent
            button.transform.SetParent(buttonParent.transform);

            // Set button caption.
            //  This implementation assumes the following:
            //  'button' has child with text as first gameobject.
            //  This is the default behavior for Buttons created in editor from GameObject->UI->Button            
            var buttonComponent = button.GetComponent<Button>();

            if (null == buttonComponent)
            {
                Debug.LogError("buttonComponent == null");
                return null;
            }

            var textComponent = buttonComponent.GetComponentInChildren<Text>();

            if (null == textComponent)
            {
                Debug.LogError("textComponent == null");
                return null;
            }

            textComponent.text = layerName;

            return button;
        }
    }
}