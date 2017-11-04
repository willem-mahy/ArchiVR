using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.WM.UI;

namespace Assets.Scripts.WM.ArchiVR
{
    public class ConstructionLighting : MonoBehaviour
    {
        // Reference to the 'Time' component.
        public TimeBehavior m_time = null;

        // Reference to the root gameobject containing all construction lights.
        public GameObject m_gameObjectWorldConstructionLighting = null;

        // The list of toggle buttons that all control the construction lighting mode.
        public List<ButtonConstructionLightingMode> m_buttonConstructionLightingModeList = new List<ButtonConstructionLightingMode>();

        // The initial state.
        public ConstructionLightingStates m_initialState = ConstructionLightingStates.On;

        // The current state.
        public ConstructionLightingStates m_state = ConstructionLightingStates.On;

        public enum ConstructionLightingStates
        {
            Auto = 0,
            On,
            Off,
        };

        // Use this for initialization
        void Start()
        {
            List<string> optionSpritePaths = new List<string>();
            optionSpritePaths.Add("Menu/LightMode/Construction/Auto");
            optionSpritePaths.Add("Menu/LightMode/Construction/On");
            optionSpritePaths.Add("Menu/LightMode/Construction/Off");

            for (int i = 0; i < m_buttonConstructionLightingModeList.Count; ++i) // hack: are we in the 'Manager' scene?
            {
                var buttonConstructionLightMode = m_buttonConstructionLightingModeList[i];

                if (buttonConstructionLightMode)
                {
                    buttonConstructionLightMode.LoadOptions(optionSpritePaths);
                }
            }
        }

        bool GetAutoLightStateFromTime()
        {
            if (m_time != null)
            {
                return (m_time.m_hour < 6 || m_time.m_hour > 18);
            }
            else
            {
                return true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (ConstructionLightingStates.Auto == m_state)
            {
                var gameObjectWorldConstructionLighting = GetGameObject_WorldConstructionLighting();

                if (null == gameObjectWorldConstructionLighting)
                {
                    return;
                }

                gameObjectWorldConstructionLighting.SetActive(GetAutoLightStateFromTime());
            }
        }

        public void OnButtonConstructionLightsClick(BaseEventData obj)
        {
            if (null == obj)
            {
                return; // Sanity -> NOOP
            }

            if (null == obj.selectedObject)
            {
                return; // Sanity -> NOOP
            }

            var buttonConstructionLightingMode = obj.selectedObject.GetComponent<ButtonConstructionLightingMode>();

            if (null == buttonConstructionLightingMode)
            {
                return; // Sanity -> NOOP
            }

            // The construction lighting mode toggle buttons are set NOT to auto-toggle upon click,
            // we have to select the next option from here.
            var newOption = buttonConstructionLightingMode.SetNextOption();
            var newState = (ConstructionLightingStates)newOption;

            SetState(newState);
        }

        // Get the root gameobject for construction lights.
        private GameObject GetGameObject_WorldConstructionLighting()
        {
            if (null == m_gameObjectWorldConstructionLighting)
            {
                // Search the root gameobject for construction lights in the first scene,
                // under the following path:
                // World/Construction/Lighting

                if (true)
                {
                    // Search the root gameobject for construction lights _in any loaded scene_,
                    // under the following path:
                    // World/Construction/Lighting
                    m_gameObjectWorldConstructionLighting = GameObject.Find("World/Construction/Lighting");
                }
                //else
                //{
                //    // WM: Deprecated?
                //    // Search the root game object for construction lights _in the first scene only_,
                //    // under the following path:
                //    // World/Construction/Lighting

                //    if (SceneManager.sceneCount < 1)
                //    {
                //        // There is no scene loaded -> NOOP
                //        return;
                //    }

                //    var projectScene = SceneManager.GetSceneAt(0);

                //    // Search for a root game object named 'World'.
                //    GameObject gameObjectWorld = null;

                //    GameObject[] gameObjects = projectScene.GetRootGameObjects();

                //    for (int i = 0; i < gameObjects.Length; ++i)
                //    {
                //        var gameObject = gameObjects[i];

                //        if (gameObject.name.Equals("World"))
                //        {
                //            gameObjectWorld = gameObject;
                //            break;
                //        }
                //    }

                //    if (null == gameObjectWorld)
                //    {
                //        return;
                //    }

                //    var gameObjectWorldConstruction = gameObjectWorld.transform.Find("Construction").gameObject;

                //    if (null == gameObjectWorldConstruction)
                //    {
                //        return;
                //    }

                //    m_gameObjectWorldConstructionLighting = gameObjectWorldConstruction.transform.Find("Lighting").gameObject;
                //}
            }

            return m_gameObjectWorldConstructionLighting;
        }

        public void SetState(ConstructionLightingStates state)
        {
            m_state = state;

            // Update the state of the construction lighting in the scene.
            var gameObjectWorldConstructionLighting = GetGameObject_WorldConstructionLighting();

            if (null == gameObjectWorldConstructionLighting)
            {
                return;
            }

            switch (m_state)
            {
                case ConstructionLightingStates.Auto:
                    gameObjectWorldConstructionLighting.SetActive(GetAutoLightStateFromTime());
                    break;
                case ConstructionLightingStates.On:
                    // Disable the lights in the construction.
                    gameObjectWorldConstructionLighting.SetActive(true);
                    break;
                case ConstructionLightingStates.Off:
                    // Enable the lights in the construction.
                    gameObjectWorldConstructionLighting.SetActive(false);
                    break;
            }

            // Update the state of the buttons that control the construction lighting mode.
            for (int i = 0; i < m_buttonConstructionLightingModeList.Count; ++i)
            {
                var buttonConstructionLightMode = m_buttonConstructionLightingModeList[i];

                if (buttonConstructionLightMode)
                {
                    buttonConstructionLightMode.SetOption((int)m_state);
                }
            }
        }
    }
}
