
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.WM.UI;
using System.Collections.Generic;

namespace Assets.Scripts.WM.ArchiVR.Menu
{
    public class MenuLayer : MonoBehaviour
    {
        public GameObject m_layerButtonPanel = null;

        public GameObject m_layerOptionPrefab = null;

        //! The button to close this menu.
        //public Button m_exitButton = null;

        // Use this for initialization
        void Start()
        {
            if (null == m_layerButtonPanel)
            {
                m_layerButtonPanel = gameObject;
            }

            DynamicallyAddLayerButtons();
        }

        // Update is called once per frame
        void Update()
        {
        }

        void ExitButton_OnClick()
        {
            Debug.Log("MenuLayer.ExitButton_OnClick()");
            UIManager.GetInstance().CloseMenu();
        }

        private void DynamicallyAddLayerButtons()
        {
            var m_layers = LayerManager.GetInstance().GetLayers();

            float x = 100;
            float y = -100;
            float yStep = -250;

            LayerButton layerButtonComponent = null;

            foreach (var layerEntry in m_layers)
            {
                GameObject option = DynamicallyAddButton(m_layerButtonPanel, layerEntry.Key, layerEntry.Value);

                if (null == option)
                {
                    Debug.LogError("option == null");
                    continue;
                }

                option.transform.localPosition = new Vector3(x, y, 0);

                y += yStep;

                //if (false)
                //{
                //    var button = option.transform.Find("LayerOptionButton");

                //    if (null == button)
                //    {
                //        Debug.LogError("button == null");
                //        continue;
                //    }

                //    layerButtonComponent = button.GetComponent<LayerButton>();
                //}
                //else
                //{
                //    var c = option.transform.GetComponentsInChildren<LayerButton>();

                //    if (0 == c.Length)
                //    {
                //        Debug.LogError("layerButtonComponent not found");
                //        continue;
                //    }

                //    layerButtonComponent = c[0];

                //    if (null == layerButtonComponent)
                //    {
                //        Debug.LogError("button == null");
                //        continue;
                //    }
                //}

                if (null != layerButtonComponent)
                {
                    layerButtonComponent.m_layerName = layerEntry.Key;
                    layerButtonComponent.m_layerGameObjectList = layerEntry.Value;                

                }
            }
        }

        private GameObject DynamicallyAddButton(
            GameObject buttonParent,
            string layerName,
            List<GameObject> gameObjectList)
        {
            var option = (GameObject)Instantiate(m_layerOptionPrefab);
                        
            if (null == option)
            {
                Debug.LogError("option = null");
                return null;
            }

            option.SetActive(true);
            option.name = "LayerOption_" + layerName;

            var button = option.transform.Find("LayerOptionButton");

            if (null == button)
            {
                Debug.LogError("button = null");
                return null;
            }

            var text = option.transform.Find("LayerOptionText");

            if (null == text)
            {
                Debug.LogError("text = null");
                return null;
            }

            // Set button parent
            option.transform.SetParent(buttonParent.transform);
            option.transform.localScale = new Vector3(1, 1, 1);

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

            var textComponent = text.GetComponentInChildren<Text>();

            if (null == textComponent)
            {
                Debug.LogError("textComponent == null");
                return null;
            }

            textComponent.text = layerName;

            var layerButtonComponent = option.GetComponent<LayerButton>();

            if (null == layerButtonComponent)
            {
                Debug.LogError("layerButtonComponent == null");
            }
            else
            {
                layerButtonComponent.m_layerName = layerName;
                layerButtonComponent.m_layerGameObjectList = gameObjectList;
            }

            return option;
        }
    }
}