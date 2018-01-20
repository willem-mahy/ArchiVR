using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.WM.Settings;

namespace Assets.WM.Scripts.UI
{
    public class TextActiveProjectName : MonoBehaviour {

        // Update is called once per frame
        void Update() {

            var activeProjectName = ApplicationSettings.GetInstance().m_data.m_stateSettings.m_activeProjectName;

            var text =  ((activeProjectName == null) || (activeProjectName == "")) ?
                        "No project loaded" :
                        activeProjectName;

            gameObject.GetComponent<Text>().text = text;
        }
    }
}
