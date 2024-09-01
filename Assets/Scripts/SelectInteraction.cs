using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SelectInteraction : TaskBase {

    protected EInteractionType m_Type;
    // Use this for initialization
    void Start () {
		
	}

    public override void Begin()
    {
        UI.RpcShowMessage("Select the interaction");
        
        m_IsEnabled = true;
    }
    // Update is called once per frame
    void Update () {

        if (!IsEnabled)
            return;

        if (VRInput.GetUp(VRInputButton.Cancel))
        {
            
            if (m_Type == EInteractionType.GamePad)
                m_Type = EInteractionType.Treadmill;
            else
                m_Type = EInteractionType.GamePad;

            Player.GetComponent<PlayerController>().InteractionType = m_Type;
            UI.RpcShowMessage(m_Type.ToString());
        }

        if (VRInput.GetUp(VRInputButton.Trigger))
        {
            m_IsEnabled = false;
            OnTaskFinished();
        }
    }
}
