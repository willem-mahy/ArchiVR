using UnityEngine;
using UnityEngine.UI;
using KS.Managers;
using KS.Entities;

namespace WM.UI
{
    public class MenuLayer : MonoBehaviour
    {
        public ScrollRect m_scrollView = null;

        public Button m_showAllLayersButton = null;

        public Button m_hideAllLayersButton = null;

        public GameObject m_layerButtonPanel = null;

        public GameObject m_layerOptionPrefab = null;

        private LayerManager m_layerManager = null;

        // Use this for initialization
        public void Start()
        {
            if (null == m_layerButtonPanel)
            {
                m_layerButtonPanel = gameObject;
            }
        }

        // Update is called once per frame
        public void Update()
        {
            if (m_layerManager != null)
            {
                m_showAllLayersButton.interactable = m_layerManager != null && !m_layerManager.AreAllLayersVisible();
                m_hideAllLayersButton.interactable = m_layerManager != null && !m_layerManager.AreAllLayersInvisible();
            }
            else
            {
                m_showAllLayersButton.interactable = false;
                m_hideAllLayersButton.interactable = false;
            }
        }

        private float GetLayerScrollStep()
        {
            int numSteps = 2;
            return 1.0f / numSteps;
        }

        public void ShowAllButton_OnClick()
        {
            Debug.Log("MenuLayer.ShowAllButton_OnClick()");

            m_layerManager.SetAllLayersVisible(true);
        }

        public void ScrollUpButton_OnClick()
        {
            Debug.Log("MenuLayer.ScrollUpButton_OnClick()");

            var np = m_scrollView.verticalNormalizedPosition;
            np = Mathf.Min(
                np + GetLayerScrollStep(),
                1.0f);

            m_scrollView.verticalNormalizedPosition = np;
        }

        public void ScrollDownButton_OnClick()
        {
            Debug.Log("MenuLayer.ScrollDownButton_OnClick()");

            var np = m_scrollView.verticalNormalizedPosition;
            np = Mathf.Max(
                np - GetLayerScrollStep(),
                0.0f);

            m_scrollView.verticalNormalizedPosition = np;
        }

        public void Init(LayerManager layerManager)
        {
            m_layerManager = layerManager;

            Clear();

            if (m_layerManager == null)
            {                
                return;
            }

            // Initialize options for Layers.
            var layers = m_layerManager.GetLayers();

            // Y spacing between successive layer option UI controls.
            float ySpacing = 20;

            // Get the height of a layer option UI control.
            float yOptionHeight = m_layerOptionPrefab.GetComponent<RectTransform>().rect.height;

            // Y step between successive layer option UI controls.
            float yStep = yOptionHeight + ySpacing;

            // Spacing on top
            float y = 0;

            // Start with a spacing above the first layer option (=top-level option in the list).
            y = ySpacing;

            // From top to bottom, 
            // generate a list option for all layers.
            foreach (var layer in layers)
            {
                // Adds a layer option for the given layer to m_layerButtonPanel at local position Vector3.zero.
                /*GameObject layerOption =*/ DynamicallyAddButton(layer, y);

                y += yStep;
            }


            var contentRectTransform = m_scrollView.content.GetComponent<RectTransform>();

            var contentSize = contentRectTransform.sizeDelta;

            contentSize.y = y + 20;

            contentRectTransform.sizeDelta = contentSize;

            //m_scrollView.content.set.rect.height = y + yStep;
        }

        private void Clear()
        {
            m_layerButtonPanel.transform.DetachChildren();
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

            var layerOptionComponent = option.GetComponent<LayerOption>();

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