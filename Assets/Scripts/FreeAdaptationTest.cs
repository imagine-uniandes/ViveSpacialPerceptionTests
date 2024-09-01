using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FreeAdaptationTest : TaskBase
{
    public Transform TargetObject;
    public int NumTrials;
    protected int m_TrialIndex;

    // Use this for initialization
    void Start()
    {
        Player.GetComponent<PlayerController>().OnObjectReached += OnObjectReached;
        TargetObject.position = new Vector3(Random.Range(-5.0f,5.0f), 0.15f, Random.Range(-5.0f,5.0f));
    }

    public override void Begin()
    {
        UI.RpcShowMessage("Find and collect the soccer balls");
        m_TrialIndex = 0;
        m_IsEnabled = true;

        Player.GetComponent<PlayerController>().EnableMotion(true, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsEnabled)
            return;

#if UNITY_EDITOR
        if (VRInput.GetUp(VRInputButton.Cancel))
        {
            m_IsEnabled = false;
            OnTaskFinished();
        }
#endif
        if (m_TrialIndex == NumTrials)
        {
            m_IsEnabled = false;
            OnTaskFinished();
        }
    }

    void OnObjectReached(string name)
    {
        if (IsEnabled)
        {
            if (m_TrialIndex < NumTrials)
            {
                UI.RpcShowMessage("Nice!");
                Random.InitState((int)Time.time);
                Vector3 pos = new Vector3(Random.Range(-5.0f, 5.0f), 0.15f, Random.Range(-5.0f, 5.0f));
                TargetObject.position = pos;
                m_TrialIndex++;
            }
            else
            {
                UI.RpcShowMessage("Congratulations!");
            }
        }
    }
}
