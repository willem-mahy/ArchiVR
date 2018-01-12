using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.WM.Script.UI.Menu
{
    public class WMMenu : MonoBehaviour
    {
        public FocusableItem m_focusedItem;

        public void SetFocusedItem(FocusableItem item)
        {
            if (m_focusedItem)
                m_focusedItem.LoseFocus();

            m_focusedItem = item;

            if (m_focusedItem)
                m_focusedItem.GainFocus();
        }

        public void FocusNeightbour(FocusableItem.NeighbourDirection nd)
        {
            if (null == m_focusedItem)
            {
                return;
            }

            var n = m_focusedItem.GetNeighbour(nd);
            if (null == n)
            {
                return;
            }

            SetFocusedItem(n);
        }

        protected void SetSelected(Button s)
        {
            if (null == s)
            {
                return;
            }

            s.interactable = true;
            s.Select();
            EventSystem.current.SetSelectedGameObject(s.gameObject, null);
        }
    }
}
