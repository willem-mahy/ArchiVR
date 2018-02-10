using Assets.Scripts.WM.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.WM.Script.UI.Menu
{
    public class MenuPlay : WMMenu
    {
        //! The button to open the 'Main Menu'
        public Button m_buttonMenuMain = null;

        // Use this for initialization
        public new void Start()
        {
            base.Start();

            m_buttonMenuMain.onClick.AddListener(ButtonMenuMain_OnClick);

            m_enableTranslation = true;
        }

        // Update is called once per frame
        public new void OnEnable()
        {
            base.OnEnable();

            // Only show 'Main Menu' button if either Mouse or Touch input is available.
            bool showButtonMenuMain = Input.mousePresent || Input.touchSupported;

            m_buttonMenuMain.gameObject.SetActive(showButtonMenuMain);
        }

        protected override void ProcessInput()
        {
            // Do not call base class:
            // This menu should not close upon 'escape'

            // If user presses 'return' key, show 'Main' menu.
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OpenMainMenu();
            }
        }

        public void ButtonMenuMain_OnClick()
        {
            OpenMainMenu();
        }

        public void OpenMainMenu()
        {
            Debug.Log("MenuPlay.OpenMainMenu()");

            UIManager.GetInstance().OpenMenu(GameObject.Find("MenuMain").GetComponent<Assets.Scripts.WM.UI.Menu>());
        }
    }
}
