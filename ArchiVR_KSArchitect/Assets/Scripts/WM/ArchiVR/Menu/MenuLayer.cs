
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.WM.UI;

namespace Assets.Scripts.WM.ArchiVR.Menu
{
    public class MenuLayer : MonoBehaviour
    {
        public GameObject m_layerButtonPanel = null;

        public GameObject m_layerButtonPrefab = null;

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
            float yStep = -150;

            foreach (var layerEntry in m_layers)
            {
                GameObject button = DynamicallyAddButton(m_layerButtonPanel, layerEntry.Key);

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

                layerButtonComponent.m_layerName = layerEntry.Key;
                layerButtonComponent.m_layerGameObjectList = layerEntry.Value;                
            }
        }

        private GameObject DynamicallyAddButton(
            GameObject buttonParent,
            string layerName)
        {
            var button = (GameObject)Instantiate(m_layerButtonPrefab);

            if (null == button)
            {
                Debug.LogError("button = null");
                return null;
            }

            button.name = "LayerButton_" + layerName;

            // Set button parent
            button.transform.SetParent(buttonParent.transform);
            button.transform.localScale = new Vector3(1, 1, 1);

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