using Assets.Scripts.WM.Settings;
using Assets.Scripts.WM.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.WM.Script.UI.Menu
{
    public class MenuControlsSettings : WMMenu
    {
        //! The 'Enable Onscreen Gamepad' button.
        public Button m_enableOnscreenGamepadButton = null;
        
        // Update is called once per frame
        public new void Update()
        {
            base.Update();

            var s = ApplicationSettings.GetInstance().m_data.m_controlSettings;

            m_enableOnscreenGamepadButton.GetComponent<CheckBox>().SetCheckedState(s.m_enableVirtualGamepad);
        }
    }
}
