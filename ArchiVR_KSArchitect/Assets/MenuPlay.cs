using UnityEngine;
using UnityEngine.UI;

namespace Assets.WM.Script.UI.Menu
{
    public class MenuPlay : WMMenu
    {
        //! The button to open the 'Main Menu'
        public Button m_buttonMenuMain = null;

        // Update is called once per frame
        public new void OnEnable()
        {
            base.OnEnable();

            // Only show 'Main Menu' button if either Mouse or Touch input is available.
            bool showButtonMenuMain = Input.mousePresent || Input.touchSupported;

            m_buttonMenuMain.gameObject.SetActive(showButtonMenuMain);
        }
    }
}
