using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facet.Combinatorics;
using Valve.VR;

public class PathIntegrationBlindTest : TaskBase
{
    //public int NumTrials;
    protected ETaskState TaskState = ETaskState.Ready;   
    protected int m_TrialIndex = 0;
    protected int m_TriangleIndex = 0;
    protected int m_NumTrials = 0;
    protected List<string> m_ResData;
    protected bool m_EnableWalking;
    protected bool m_IsFeedback = true;
    protected bool m_ShowFeedback = false;
    protected string m_Mode;
    protected Vector3 m_Dir;

    protected int[] m_SortedAngles;

    // Use this for initialization
    void Start()
    {
        Player.GetComponent<PlayerController>().OnPointReached += OnPointReached;
    }

    /*public void Setup(float radius, int trials, bool showFeedback, string mode)
    {
        this.GetComponent<TrianglePathController>().CreateTriangles(radius);

        m_ShowFeedback = showFeedback;
        m_Mode = mode;
        m_NumTrials = trials;
    }*/

    public override void Begin()
    {
        m_TrialIndex = 0;
        m_ResData = new List<string>();
        m_ResData.Add("method;trial;target_angle;target_distance;offset;angle;dir");

        m_SortedAngles = new int[this.GetComponent<TrianglePathController>().NumTriangles];
        for (int i = 0; i < m_SortedAngles.Length; i++)
            m_SortedAngles[i] = i;

        m_SortedAngles = new Randomizer<int>().Shuffle(m_SortedAngles);

        this.NextTriangle();

        m_IsEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsEnabled)
            return;

        if (m_TrialIndex < this.GetComponent<TrianglePathController>().NumTriangles * m_NumTrials)
        {
            m_EnableWalking = false;

            switch (TaskState)
            {
                case ETaskState.PathVisualization:
                    if (VRInput.GetUp(VRInputButton.Trigger))
                    {
                        UI.RpcShowMessage("Turn to the blue point");
                        TaskState = ETaskState.Ready;
                    }
                    break;
                case ETaskState.Ready:
                    m_EnableWalking = true;
                    if (VRInput.GetUp(VRInputButton.Trigger))
                    {
                        UI.RpcShowMessage("Move to the blue point");
                        Player.GetComponent<PlayerController>().EnableMotion(true, false);
                        TaskState = ETaskState.EncodingTurnA;
                    }
                    break;
                case ETaskState.EncodingWalkA:
                    m_EnableWalking = true;
                    if (VRInput.GetUp(VRInputButton.Trigger))
                    {
                        UI.RpcShowMessage("Move to the red point");
                        Player.GetComponent<PlayerController>().EnableMotion(true, false);
                        TaskState = ETaskState.EncodingTurnB;
                    }
                    break;
                case ETaskState.EncodingWalkB:
                    m_EnableWalking = true;

                    if (VRInput.GetUp(VRInputButton.Trigger))
                    {
                        UI.RpcShowMessage("Move to the start position");
                        Player.GetComponent<PlayerController>().EnableMotion(true, false);

                       /* if (TestMethod == EMeasurementMethod.BlindWalking)
                            Camera.main.GetComponentInChildren<FadeController>().RpcBeginFade();
                        else
                            Camera.main.GetComponentInChildren<FadeController>().RpcEndFade();*/

                        TaskState = ETaskState.Execution;

                        m_Dir = Player.transform.Find("ForwardIndicator").forward * -1;
                    }

                    break;
                case ETaskState.Execution:
                    m_EnableWalking = true;

                    if (VRInput.GetUp(VRInputButton.Trigger))
                    {
                        Camera.main.GetComponentInChildren<FadeController>().RpcEndFade();

                        if (m_IsFeedback)
                        {
                            /*if (m_ShowFeedback && TestMethod == EMeasurementMethod.BlindWalking)
                            {
                                UI.RpcShowMessage("Return to the start position");
                            }
                            else
                            {
                                UI.RpcShowMessage("Press trigger to continue");
                                m_IsFeedback = false;
                            }*/

                            this.SetData();
                        }  
                        else
                        {
                            TaskState = ETaskState.Ready;
                            m_TrialIndex++;
                            m_TriangleIndex++;

                            if (m_TriangleIndex == this.GetComponent<TrianglePathController>().NumTriangles)
                                m_TriangleIndex = 0;

                            //TestMethod = (TestMethod == EMeasurementMethod.BlindTurning) ? EMeasurementMethod.BlindWalking : EMeasurementMethod.BlindTurning;
                                                        
                            this.NextTriangle();
                            m_IsFeedback = true;
                        }
                    }
                    break;
                default:
                    m_EnableWalking = true;
                    break;
            }

        }
        else
        {
            string interaction = Player.GetComponent<PlayerController>().InteractionType.ToString();
            System.IO.File.WriteAllLines(Application.dataPath + string.Format(@"\dist_{0}_{1}.txt", interaction, m_Mode), m_ResData.ToArray());
            m_IsEnabled = false;

            m_IsEnabled = false;
            OnTaskFinished();
        }
    }

    private void SetData()
    {
        string interaction = Player.GetComponent<PlayerController>().InteractionType.ToString();
        Vector3 targetPos = transform.Find("Start").position;
        Vector3 pos = new Vector3(Player.transform.position.x, targetPos.y, Player.transform.position.z);
        float offset = Vector3.Distance(targetPos, pos);
        Vector3 dirPath = transform.Find("PointB").position - transform.Find("Start").position;
        Vector3 dirExec = transform.Find("PointB").position - pos;
        float angle = Vector3.Angle(dirPath, dirExec);
        float dir = Vector3.Angle(dirPath, m_Dir);
        float targetAngle = 0; //this.GetComponent<TrianglePathController>().Angles[m_TriangleIndex];
        float targetDistance = Vector3.Distance(targetPos, transform.Find("PointB").position);
        string info = string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", interaction, 0, m_TrialIndex, targetAngle, targetDistance, offset, angle, dir);

        Debug.Log(info);

        m_ResData.Add(info);
    }

    public void OnPointReached(string name)
    {
        if (TaskState == ETaskState.EncodingTurnA && name == "PointA")
        {
            UI.RpcShowMessage("Turn to the red point");
            TaskState = ETaskState.EncodingWalkA;
            Player.GetComponent<PlayerController>().EnableMotion(false, true);
        }

        if (TaskState == ETaskState.EncodingTurnB && name == "PointB")
        {
            UI.RpcShowMessage("Turn to the start position");
            TaskState = ETaskState.EncodingWalkB;
            Player.GetComponent<PlayerController>().EnableMotion(false, true);

            /*if (TestMethod == EMeasurementMethod.BlindTurning)
                Camera.main.GetComponentInChildren<FadeController>().RpcBeginFade();*/

        }

        if(m_IsFeedback && TaskState == ETaskState.Execution && name == "Start")
        {
            /*if (TestMethod == EMeasurementMethod.BlindTurning)
                Player.GetComponent<PlayerController>().EnableMotion(false, false);*/

            UI.RpcShowMessage("Press trigger to continue");
            m_IsFeedback = false;
        }
    }

    private void NextTriangle()
    {
        if (m_TrialIndex < this.GetComponent<TrianglePathController>().NumTriangles * m_NumTrials)
        {
            int triangle = m_SortedAngles[m_TriangleIndex];
            
            Debug.Log(triangle);
            Vector3 start = transform.Find("Points").GetChild(triangle * 3 + 0).position;
            Vector3 pointA = transform.Find("Points").GetChild(triangle * 3 + 1).position;
            Vector3 pointB = transform.Find("Points").GetChild(triangle * 3 + 2).position;

            transform.Find("Start").position = start;
            //transform.Find("PointA").GetComponent<RemoteSync>().RpcSetPosition(pointA);
            transform.Find("PointA").position = pointA;
            //transform.Find("PointB").GetComponent<RemoteSync>().RpcSetPosition(pointB);
            transform.Find("PointB").position = pointB;

            if (Player.GetComponent<PlayerController>().InteractionType == EInteractionType.GamePad)
            {
                Player.transform.position = transform.Find("Start").position;
                Player.GetComponent<CharacterController>().Move(Vector3.one * 0.01f);                
            }
            else
            {
                Player.transform.position = new Vector3(transform.Find("Start").position.x - Camera.main.transform.localPosition.x, Player.transform.position.y, transform.Find("Start").position.z - Camera.main.transform.localPosition.z);
                Player.GetComponent<CharacterController>().Move(Vector3.one * 0.01f);
            }

            Player.GetComponent<PlayerController>().EnableMotion(false, true);

            TaskState = ETaskState.PathVisualization;
            UI.RpcShowMessage("Visualize the points and press trigger");
        }
    }
}
