using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.WM.UI
{
    public class TabView : MonoBehaviour
    {
        public List<GameObject> m_tabPanes = new List<GameObject>();

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public int GetNumTabs()
        {
            return m_tabPanes.Count;
        }
        
        public void SetActiveTab(int index)
        {
            for (int debugType = 0; debugType < m_tabPanes.Count; ++debugType)
            {
                m_tabPanes[debugType].SetActive(index == debugType);
            }
        }
    }
}
