using System;

namespace Assets.WM.Settings
{
    [Serializable()]
    public class StateSettings
    {        
        public Assets.WM.UI.UIMode m_uiMode = Assets.WM.UI.UIMode.ScreenSpace;

        public string m_activeProjectName = "";
    }
}
