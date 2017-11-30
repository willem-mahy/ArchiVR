using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.WM
{
    public class LayerButton : MonoBehaviour
    {
        public string m_layerName = null;

        public List<GameObject> m_layerGameObjectList = null;

        // Use this for initialization
        void Start()
        {
            var buttonComponent = gameObject.GetComponent<Button>();

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

            if (m_layerGameObjectList != null)
            {
                if (m_layerGameObjectList.Count > 0)
                {
                    var active = m_layerGameObjectList[0].activeSelf;

                    activeStateText = (active ? "V" : "X");
                }
            }

            var textComponent = gameObject.GetComponentInChildren<Text>();

            if (null != textComponent)
            {
                textComponent.text = activeStateText + " " + m_layerName;
            }
        }

        public void OnClick()
        {
            if (m_layerGameObjectList == null)
            {
                return;
            }

            if (m_layerGameObjectList.Count == 0)
            {
                return;
            }

            var activate = !m_layerGameObjectList[0].activeSelf;

            foreach (var go in m_layerGameObjectList)
            {
                go.SetActive(activate);
            }
        }
    }
}
