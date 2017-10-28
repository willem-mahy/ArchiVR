using Assets.Scripts.WM.UI;
using System;
using UnityEngine;

public class ControlCameraFlyDPad : CameraFlyBase
{
    private GameObject m_padXZ = null;
    private GameObject m_padY = null;

    // Use this for initialization
    void Start()
    {
        // Get handle to XZ D-pad.
        m_padXZ = gameObject.transform.Find("DPadXZ").gameObject;

        if (!m_padXZ)
        {
            throw new Exception(gameObject.name + ".Start(): Cannot find child UI.Button 'DPadXZ' below ControlCameraFlyDPad UI.Panel gameobject.");
        }

        // Get handle to Y D-pad.
        m_padY = gameObject.transform.Find("DPadY").gameObject;

        if (!m_padY)
        {
            throw new Exception(gameObject.name + ".Start(): Cannot find child UI.Panel 'DPadY' below ControlCameraFlyDPad UI.Panel gameobject.");
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        // Adjust XZ.
        var stickXZ = m_padXZ.GetComponent<DPadBehavior>();
        var offsetXZ = stickXZ.GetStickOffset();

        TranslateXZ(offsetXZ);

        // Adjust Y.
        var stickY = m_padY.GetComponent<DPadBehavior>();
        var offsetY = stickY.GetStickOffset().y;

        TranslateY(offsetY);

        if (m_doDebug && (m_textDebug != null))
            m_textDebug.text =
                "XZ D-pad:" +
                "\n\toffset: " + stickXZ.GetStickOffset() +
                "\n\tstatus: " + stickXZ.GetStatus() +
                "\n------------------" +
                "\nY D-pad:" +
                "\n\toffset: " + stickY.GetStickOffset() +
                "\n\tstatus: " + stickY.GetStatus();
    }
}
