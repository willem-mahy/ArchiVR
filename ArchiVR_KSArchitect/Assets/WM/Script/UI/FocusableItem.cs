using Assets.Scripts.WM.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.WM.Script.UI.Menu
{
    public abstract class FocusableItem : MonoBehaviour
    {
        public GameObject m_uiControlGameOject;

        public enum NeighbourDirection
        {
            Left = 0,
            Right,
            Top,
            Bottom
        }

        FocusableItem[] m_neighbours = new FocusableItem[4];

        // Color of ui control when not focused.
        Color m_normalColor;
        Color m_focusColor = Color.red;

        public FocusableItem GetNeighbour(NeighbourDirection nd)
        {
            return m_neighbours[(int)nd];
        }

        public void SetNeighbourFocusable(
            FocusableItem.NeighbourDirection dir,
            FocusableItem item)
        {
            m_neighbours[(int)dir] = item;
        }

        public virtual void GainFocus()
        {
            var buttonComponent = gameObject.GetComponent<Button>();

            if (null != buttonComponent)
            {
                m_normalColor = buttonComponent.image.material.color;
                buttonComponent.image.material.color = m_focusColor;
            }
        }

        public virtual void LoseFocus()
        {
            var buttonComponent = gameObject.GetComponent<Button>();

            if (null != buttonComponent)
            {
                buttonComponent.image.material.color = m_normalColor;
            }
        }
    }
}