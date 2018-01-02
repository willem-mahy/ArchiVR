using Assets.Scripts.WM.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.WM.Script.UI.VirtualGamepad
{
    public class VirtualGamepad_Teleport : MonoBehaviour
    {
        // Virtual Axis/Button names.
        static public string Teleport = "VirtualGamePadTeleport_Teleport";

        public Button m_teleportButton = null;

        CrossPlatformInputManager.VirtualButton m_teleportVirtualButton;     // Reference to the run button in the cross platform input

        void Awake()
        {
            Debug.Log("VirtualGamepad_Teleport.Awake()");

            // Teleport button
            m_teleportVirtualButton = new CrossPlatformInputManager.VirtualButton(VirtualGamepad_Teleport.Teleport);
        }

        void Start()
        {
            Debug.Log("VirtualGamepad_Teleport.Start()");
        }

        private void OnEnable()
        {
            Debug.Log("VirtualGamepad_Teleport.OnEnable()");

            // Teleport
            if (CrossPlatformInputManager.ButtonExists(m_teleportVirtualButton.name))
            {
                CrossPlatformInputManager.UnRegisterVirtualButton(m_teleportVirtualButton.name);
            }
            CrossPlatformInputManager.RegisterVirtualButton(m_teleportVirtualButton);

            if (m_teleportButton)
            {
                /*
                m_teleportButton.OnPointerDown += RunButton_OnPointerDown;
                m_teleportButton.OnPointerUp += RunButton_OnPointerUp;
                */
            }
        }

        private void OnDisable()
        {
            Debug.Log("VirtualGamepad_Teleport.OnDisable()");

            if (null != m_teleportVirtualButton)
            {
                CrossPlatformInputManager.UnRegisterVirtualButton(m_teleportVirtualButton.name);
            }
            
            if (m_teleportButton)
            {
                /*
                m_teleportButton.OnPointerDown -= TeleportButton_OnPointerDown;
                m_teleportButton.OnPointerUp -= TeleportButton_OnPointerUp;
                */
            }
        }

        // Update is called once per frame
        void Update()
        {
        }

        void TeleportButton_OnPointerDown(PointerEventData ped)
        {
            m_teleportVirtualButton.Pressed();
        }

        void TeleportButton_OnPointerUp(PointerEventData ped)
        {
            m_teleportVirtualButton.Released();
        }        
    }
}