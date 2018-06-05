using System.Collections.Generic;
using UnityEngine;
using WM.Gamepad;

namespace Assets.WM.UI
{
    public class TabView : MonoBehaviour
    {
        public int m_initialActiveTab = 0;

        public List<GameObject> m_tabPanes = new List<GameObject>();

        //! Use this for initialization
        void Start()
        {
            if (GetNumTabs() > 0)
            {
                SetActiveTab(Mathf.Min(GetNumTabs(), m_initialActiveTab));
            }
        }

        //!
        int GetActiveTabIndex()
        {
            for (int debugType = 0; debugType < m_tabPanes.Count; ++debugType)
            {
                if (m_tabPanes[debugType].activeSelf)
                    return debugType;
            }

            return -1; // none selected.
        }

        //!
        int GetPrevTabIndex(bool cycle)
        {
            var nextIndex = GetActiveTabIndex() - 1;

            if (m_tabPanes.Count == 0)
            {
                return -1;
            }

            if (nextIndex < 0)
            {
                if (cycle)
                {
                   nextIndex = m_tabPanes.Count - 1;
                }
                else
                {
                    nextIndex = Mathf.Max(nextIndex, 0);
                }
            }

            return nextIndex;
        }

        //!
        int GetNextTabIndex(bool cycle)
        {
            var nextIndex = GetActiveTabIndex() + 1;

            if (cycle)
            {
                nextIndex = nextIndex % m_tabPanes.Count;
            }
            else
            {
                nextIndex = Mathf.Min(nextIndex, m_tabPanes.Count - 1);
            }

            return nextIndex;
        }

        float m_prevX = 0;

        // Update is called once per frame
        void Update()
        {
            var cycle = true;

            float x = Input.GetAxis(GamepadXBox.DPadHorizontal);

            if (x != m_prevX)
            {
                if (x > 0)
                {
                    SetActiveTab(GetNextTabIndex(cycle));
                }

                if (x < 0)
                {
                    SetActiveTab(GetPrevTabIndex(cycle));
                }

                m_prevX = x;
            }
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
