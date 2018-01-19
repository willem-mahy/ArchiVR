using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

namespace Assets.WM.Script.UI.Menu
{
    public class WMMenu : MonoBehaviour
    {
        public FocusableItem m_focusedItem;

        public FirstPersonController m_firstPersonController = null;

        private void OnEnable()
        {
            EnableFPS(false);
            //EnablePOIMenu(false);
        }

        private void OnDisable()
        {
            EnableFPS(true);
            //EnablePOIMenu(true);
        }

        protected void Update()
        {
            bool e =
                Input.GetKey(KeyCode.RightControl)
                || Input.GetKey(KeyCode.LeftControl);
            EnableFPS(e);
        }

        private void EnableFPS(bool state)
        {
            if (null == m_firstPersonController)
            {
                return;
            }

            m_firstPersonController.enabled = state;
        }

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
