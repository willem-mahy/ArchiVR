﻿
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

        void ShowAllButton_OnClick()
        {
            Debug.Log("MenuLayer.ShowAllButton_OnClick()");

            LayerManager.GetInstance().SetAllLayersVisible(true);
        }

        private void DynamicallyAddLayerButtons()
        {
            var layers = LayerManager.GetInstance().GetLayers();

            float y = 0;

            // Y spacing between successive layer option UI controls.
            float ySpacing = 20;

            // Get the height of a layer option UI control.
            float yOptionHeight = m_layerOptionPrefab.GetComponent<RectTransform>().rect.height;

            // Y step between successive layer option UI controls.
            float yStep = yOptionHeight + ySpacing;

            // Generate a layer option for all layers.
            foreach (var layer in layers)
            {
                // Adds a layer option for the given layer to m_layerButtonPanel at local position Vector3.zero.
                GameObject layerOption = DynamicallyAddButton(layer, y);               
                
                y += yStep;
            }
        }

        private GameObject DynamicallyAddButton(
            Layer layer,
            float yOffset)
        {
            //var text = m_layerOptionPrefab.transform.Find("LayerOptionText");

            //if (null == text)
            //{
            //    Debug.LogError("text = null");
            //    return null; ;
            //}

            //var textRectTransform = text.GetComponent<RectTransform>();

            //if (null != textRectTransform)
            //{
            //    textRectTransform.offsetMax = Vector2.zero;
            //    textRectTransform.offsetMin = Vector2.zero;
            //}

            var option = (GameObject)Instantiate(m_layerOptionPrefab);
            
            if (null == option)
            {
                Debug.LogError("option = null");
                return null;
            }

            option.SetActive(true);
            option.name = "LayerOption_" + layer.GetName();

            // Set the rect transform top offset for the layer option UI control.
            var rectTransform = option.GetComponent<RectTransform>();

            if (null != rectTransform)
            {
                // rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMax.y - yOffset);
                var pos = rectTransform.localPosition;
                pos.y -= yOffset;
                rectTransform.localPosition = pos;
            }

            // Add layer option UI Control to its parent UI control.
            option.transform.SetParent(m_layerButtonPanel.transform, false);

            // Initialize layer option UI control local scale, rotation and offset.
            //option.transform.localScale = Vector3.one;
            //option.transform.localRotation = Quaternion.identity;            

            var layerOptionComponent = option.GetComponent<LayerButton>();

            if (null == layerOptionComponent)
            {
                Debug.LogError("layerOptionComponent == null");
                return null;
            }
            else
            {
                layerOptionComponent.SetLayer(layer);
            }

            return option;
        }
    }
}