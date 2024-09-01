using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestIntervalTask : TaskBase {

    public float RestTime;
    public Transform TargetObject;
    public string m_Message;
    protected float m_Time;
    protected bool m_ShowedMessage;

    // Use this for initialization
    void Start()
    {

    }

    public override void Begin(string message)
    {
        m_IsEnabled = true;

        m_Time = 0;
        m_ShowedMessage = false;
        m_Message = message;

        UI.RpcShowMessage("Please rest some seconds");
        Camera.main.GetComponentInChildren<FadeController>().RpcBeginFade();
    }
	
	// Update is called once per frame
	void Update () {

        if (!m_IsEnabled)
            return;

        m_Time += Time.deltaTime;

#if UNITY_EDITOR
        if (VRInput.GetUp(VRInputButton.Cancel))
            m_Time = RestTime;
#endif

        if (!m_ShowedMessage && m_Time > RestTime - 5)
        {
            UI.RpcShowMessage(m_Message);
            m_ShowedMessage = true;
        }
        if(m_Time > RestTime)
        {
            Camera.main.GetComponentInChildren<FadeController>().RpcEndFade();
            m_IsEnabled = false;
            OnTaskFinished();
        }
	}
}
