using System.Collections.Generic;
using UnityEngine;

namespace KS.Entities
{
    /*! A building layer in the Architectural model
     */
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
}
