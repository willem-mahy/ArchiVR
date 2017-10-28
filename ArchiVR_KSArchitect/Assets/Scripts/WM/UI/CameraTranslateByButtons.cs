using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraTranslateByButtons : MonoBehaviour {

    public float m_moveSpeed = 0;
    public Camera m_camera = null;
    public Button m_forwardButton = null;
    public Button m_backWardButton = null;

    public float m_acceleration = 0.01f;

    // Use this for initialization
    void Start () {
        {
            Button btn = m_forwardButton.GetComponent<Button>();
            btn.onClick.AddListener(ForwardButtonOnClick);
        }

        {
            Button btn = m_backWardButton.GetComponent<Button>();
            btn.onClick.AddListener(BackwardButtonOnClick);
        }
    }
	
	// Update is called once per frame
	void Update () {
        m_camera.transform.position += m_moveSpeed * 100 * Time.deltaTime * m_camera.transform.forward;
	}

    void ForwardButtonOnClick()
    {
        Debug.Log("You have clicked the Forward button!");

        if (m_moveSpeed < 0)
        {
            m_moveSpeed = 0.0f;
        }
        else
        {
            m_moveSpeed+= m_acceleration;
        }
    }

    void BackwardButtonOnClick()
    {
        Debug.Log("You have clicked the Backward button!");

        if (m_moveSpeed > 0)
        {
            m_moveSpeed = 0.0f;
        }
        else
        {
            m_moveSpeed-= m_acceleration;
        }
    }
}
