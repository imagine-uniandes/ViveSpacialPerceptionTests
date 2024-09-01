using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

public class SetupTask : TaskBase
{    
    public SteamVR_TrackedObject LeftController;
    public SteamVR_TrackedObject RightController;
    public SteamVR_TrackedObject HeadTracker;
    protected float m_Time = 0;

    // Use this for initialization
    void Start()
    {

    }

    public override void Begin()
    {
        m_IsEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsEnabled)
            return;

        bool ready = (LeftController.index != SteamVR_TrackedObject.EIndex.None) && (RightController.index != SteamVR_TrackedObject.EIndex.None); /*&&
            /*(HeadTracker.index != SteamVR_TrackedObject.EIndex.None)*/

        m_Time += Time.deltaTime;
        if (m_Time > 3.0f)
        {
            if(ready)
                UI.RpcShowMessage("Press trigger to begin");
            else
                UI.RpcShowMessage("Wating for devices");
            
            m_Time = 0;
        }

        if (ready && VRInput.GetUp(VRInputButton.Trigger))
        {
            m_IsEnabled = false;
            OnTaskFinished();
        }
    }
}
