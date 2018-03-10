using Assets.Scripts.WM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePadPreview : MonoBehaviour {

    public Button m_buttonX = null;
    public Button m_buttonY = null;
    public Button m_buttonA = null;
    public Button m_buttonB = null;

    public Button m_buttonL1 = null;
    public Button m_buttonL2 = null;
    public Button m_buttonR1 = null;
    public Button m_buttonR2 = null;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (m_buttonA)
        {
            if (Input.GetKey(GamepadXBox.A))
            {
                m_buttonA.OnPointerEnter(null);
            }
            else
            {
                m_buttonA.OnPointerExit(null);
            }
        }

        if (m_buttonB)
        {
            if (Input.GetKey(GamepadXBox.B))
            {
                m_buttonB.OnPointerEnter(null);
            }
            else
            {
                m_buttonB.OnPointerExit(null);
            }
        }

        if (m_buttonX)
        {
            if (Input.GetKey(GamepadXBox.X))
            {
                m_buttonX.OnPointerEnter(null);
            }
            else
            {
                m_buttonX.OnPointerExit(null);
            }
        }

        if (m_buttonY)
        {
            if (Input.GetKey(GamepadXBox.Y))
            {
                m_buttonY.OnPointerEnter(null);
            }
            else
            {
                m_buttonY.OnPointerExit(null);
            }
        }

        if (m_buttonL1)
        {
            if (Input.GetKey(GamepadXBox.L1))
            {
                m_buttonL1.OnPointerEnter(null);
            }
            else
            {
                m_buttonL1.OnPointerExit(null);
            }
        }

        //if (m_buttonL2)
        //{
        //    if (Input.GetKey(GamepadXBox.L2))
        //    {
        //        m_buttonL2.OnPointerEnter(null);
        //    }
        //    else
        //    {
        //        m_buttonL2.OnPointerExit(null);
        //    }
        //}

        if (m_buttonR1)
        {
            if (Input.GetKey(GamepadXBox.R1))
            {
                m_buttonR1.OnPointerEnter(null);
            }
            else
            {
                m_buttonR1.OnPointerExit(null);
            }
        }

        //if (m_buttonR2)
        //{
        //    if (Input.GetKey(GamepadXBox.R2))
        //    {
        //        m_buttonR2.OnPointerEnter(null);
        //    }
        //    else
        //    {
        //        m_buttonR2.OnPointerExit(null);
        //    }
        //}
    }
}
