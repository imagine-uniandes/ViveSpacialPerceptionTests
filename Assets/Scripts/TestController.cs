using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using Facet.Combinatorics;

public class TestController : MonoBehaviour
{
    public VRInputBase VRInput;
    public PlayerController Player;
    public UIController UI;
    private int m_Option = 0;
    private bool m_FirtInterationEnd = false;
    
    void Awake()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            this.GetComponentsInChildren<TaskBase>()[i].Setup(UI, Player, VRInput);
            this.GetComponentsInChildren<TaskBase>()[i].OnTaskFinished += OnTaskFinished;
        }

    }

    // Update is called once per frame
    void Update()
    {
        switch (m_Option)
        {
            case 0:
                if (!this.GetComponentInChildren<SetupTask>().IsEnabled)
                    this.GetComponentInChildren<SetupTask>().Begin();
                break;
            case 1:
                if (!this.GetComponentInChildren<SelectInteraction>().IsEnabled)
                    this.GetComponentInChildren<SelectInteraction>().Begin();
                break;
            case 2:
                if (!this.GetComponentInChildren<FreeAdaptationTest>().IsEnabled)
                    this.GetComponentInChildren<FreeAdaptationTest>().Begin();
                break;
            case 3:
                if (!this.GetComponentInChildren<RestIntervalTask>().IsEnabled)
                    this.GetComponentInChildren<RestIntervalTask>().Begin("Prepare for the practice test...");
                break;
            case 4:
                if (!this.GetComponentInChildren<PathIntegrationTest>().IsEnabled)
                    this.GetComponentInChildren<PathIntegrationTest>().Begin(3.0f, true, 1);
                break;
            case 5:
                if (!this.GetComponentInChildren<RestIntervalTask>().IsEnabled)
                    this.GetComponentInChildren<RestIntervalTask>().Begin("Prepare for the perception trial...");
                break;
            case 6:
                if (!this.GetComponentInChildren<DistancePerceptionTest>().IsEnabled)
                    this.GetComponentInChildren<DistancePerceptionTest>().Begin(3.0f, false, 1);
                break;
            case 7:
                if (!this.GetComponentInChildren<RestIntervalTask>().IsEnabled)
                    this.GetComponentInChildren<RestIntervalTask>().Begin("Prepare for the first test...");
                break;
            case 8:
                if (!this.GetComponentInChildren<PathIntegrationTest>().IsEnabled)
                    this.GetComponentInChildren<PathIntegrationTest>().Begin(7.5f, true, 2);
                break;
            case 9:
                if (!this.GetComponentInChildren<RestIntervalTask>().IsEnabled)
                    this.GetComponentInChildren<RestIntervalTask>().Begin("Prepare for the perception trial...");
                break;
            case 10:
                if (!this.GetComponentInChildren<DistancePerceptionTest>().IsEnabled)
                    this.GetComponentInChildren<DistancePerceptionTest>().Begin(7.5f, false, 2);
                break;
            case 11:
                UI.RpcShowMessage("Gracias!");
                break;

        }
    }

    void OnTaskFinished()
    {
        m_Option++;
    }
}
