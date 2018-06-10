using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KS.Entities;

namespace WM.UI
{
    public class LayerOption : MonoBehaviour
    {
        private Toggle m_toggleComponent = null;

        private Layer m_layer = null;
                
        void Awake()
        {
            m_toggleComponent = gameObject.GetComponent<Toggle>();

            if (null == m_toggleComponent)
            {
                Debug.LogError("toggleComponent == null");
            }
            else
            {
                m_toggleComponent.onValueChanged.AddListener( (value) => { OnValueChanged(value); }  );
            }
        }

        // Update is called once per frame
        void Update()
        {
            string activeStateText = "?";
            string layerName = "?";

            if (null != m_layer)
            {
                activeStateText = (m_layer.IsVisible() ? "V" : "X");
                layerName = m_layer.GetName();
            }

            var textComponent = gameObject.GetComponentInChildren<Text>();

            if (null != textComponent)
            {
                textComponent.text = activeStateText + " " + layerName;
            }
        }

        public void OnValueChanged(bool toggleValue)
        {
            if (null == m_layer)
            {
                return;
            }

            m_layer.SetVisible(toggleValue);
        }

        public void SetLayer(Layer layer)
        {
            m_layer = layer;

            m_toggleComponent.isOn = layer.IsVisible();

            var text = transform.Find("Label");

            if (null == text)
            {
                Debug.LogError("text = null");
                return;
            }

            //var textRectTransform = text.GetComponent<RectTransform>();

            //if (null != textRectTransform)
            //{
            //    textRectTransform.offsetMax = textRectTransform.offsetMax;
            //    textRectTransform.offsetMin = textRectTransform.offsetMin;
            //}            

            var textComponent = text.GetComponentInChildren<Text>();

            if (null == textComponent)
            {
                Debug.LogError("textComponent == null");
                return;
            }

            textComponent.text = layer.GetName();
        }
    }
}
