using System;

namespace Assets.Scripts.WM.Settings
{
    [Serializable()]
    public class StateSettings
    {        
        public Assets.Scripts.WM.UI.UIMode m_uiMode = Assets.Scripts.WM.UI.UIMode.ScreenSpace;

        public string m_activeProjectName = "";

        public bool m_enableVirtualGamepad = true;
    }
}
