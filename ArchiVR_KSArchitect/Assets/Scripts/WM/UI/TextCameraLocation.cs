using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*! Displays the location of a Camera in a Text.
 */
public class WMTextCameraLocation : MonoBehaviour {
    public Camera m_camera = null;

    public Text m_TextPOIName = null;

    // Use this for initialization
    void Start () {
        //gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (m_camera == null)
        {
            return;
        }

        var guiText = gameObject.GetComponent<Text>();

        if (guiText == null)
        {
            return;
        }

        guiText.text = "Pos:" + m_camera.transform.position.ToString() + " Rot:" + m_camera.transform.rotation.eulerAngles.ToString();

        if (Input.GetKeyDown("p"))
        {
            var txt = guiText.text + Environment.NewLine;

            if (m_TextPOIName)
            {
                txt = m_TextPOIName.text + ": " + txt;
            }
            System.IO.File.AppendAllText("c:\\ArchiVR\\poi.txt", txt);
        }
    }
}
