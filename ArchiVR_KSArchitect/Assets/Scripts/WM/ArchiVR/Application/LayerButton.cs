using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WM
{
    // TODO: Rename class LayerButton into LayerOption
    public class LayerButton : MonoBehaviour
    {
        private Layer m_layer = null;

        // Use this for initialization
        void Start()
        {
            var button = gameObject.transform.Find("LayerOptionButton");

            if (null == button)
            {
                Debug.LogError("button = null");
                return;
            }

            var buttonComponent = button.GetComponent<Button>();
            
            if (null == buttonComponent)
            {
                Debug.LogError("buttonComponent == null");
            }
            else
            {
                buttonComponent.onClick.AddListener(OnClick);
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

        public void OnClick()
        {
            if (null == m_layer)
            {
                return;
            }

            m_layer.ToggleVisible();
        }

        public void SetLayer(Layer layer)
        {
            m_layer = layer;

            var button = transform.Find("LayerOptionButton");

            if (null == button)
            {
                Debug.LogError("button = null");
                return;
            }

            var text = transform.Find("LayerOptionText");

            if (null == text)
            {
                Debug.LogError("text = null");
                return;
            }

            var textRectTransform = text.GetComponent<RectTransform>();

            if (null != textRectTransform)
            {
                textRectTransform.offsetMax = textRectTransform.offsetMax;
                textRectTransform.offsetMin = textRectTransform.offsetMin;
            }

            // Set button caption.
            //  This implementation assumes the following:
            //  'button' has child with text as first gameobject.
            //  This is the default behavior for Buttons created in editor from GameObject->UI->Button            
            var buttonComponent = button.GetComponent<Button>();

            if (null == buttonComponent)
            {
                Debug.LogError("buttonComponent == null");
                return;
            }

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
