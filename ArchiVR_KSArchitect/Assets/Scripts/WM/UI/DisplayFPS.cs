using UnityEngine;
using System.Collections;

namespace Assets.Scripts.WM.UI
{  
    //! Attach to a Text.
    public class DisplayFPS : MonoBehaviour
    {
        float deltaTime = 0.0f;

        void Update()
        {
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            gameObject.GetComponent<UnityEngine.UI.Text>().text = text;
            //GUI.Label(rect, text, style);
        }

        //void OnGUI()
        //{
        //    int h = 40;
        //    int w = h * 15;

        //    GUIStyle style = new GUIStyle();

        //    var pos = gameObject.transform.position;
        //    Rect rect = new Rect(0, 0, w, h);
        //    style.alignment = TextAnchor.UpperLeft;
        //    style.fontSize = 40;
        //    style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);            
        //}
    }
}
