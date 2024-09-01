using Facet.Combinatorics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePerceptionTest : TaskBase
{
    protected int m_NumTrials = 0;
    protected ETaskState TaskState = ETaskState.Ready;
    protected int m_TrialIndex = 0;
    protected int m_TriangleIndex = 0;    
    protected List<string> m_ResData;
    protected Vector3 m_Dir;
    protected int m_Iteration;
    protected bool m_IsFeedback;
    protected float m_Delay = 0;

    protected int[] m_SortedTriangles;

    // Use this for initialization
    void Start()
    {
        Player.GetComponent<PlayerController>().OnPointReached += OnPointReached;
    }

    public override void Begin(float radius, bool normalPath, int numTrials)
    {
        m_Iteration++;
        m_TrialIndex = 0;
        m_ResData = new List<string>();
        m_IsFeedback = true;
        m_NumTrials = numTrials;

        this.GetComponent<TrianglePathController>().CreateTriangles(normalPath, radius);

        m_SortedTriangles = new int[this.GetComponent<TrianglePathController>().NumTriangles];
        for (int i = 0; i < m_SortedTriangles.Length; i++)
            m_SortedTriangles[i] = i;

        m_SortedTriangles = new Randomizer<int>().Shuffle(m_SortedTriangles);

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
            switch (TaskState)
            {
                /*case ETaskState.PathVisualization:
                    if (VRInput.GetUp(VRInputButton.Trigger))
                    {
                        UI.RpcShowMessage("Turn to the blue point");
                        TaskState = ETaskState.Ready;
                    }
                    break;*/
                case ETaskState.Ready:

                    if (VRInput.GetUp(VRInputButton.Trigger))
                    {
                        UI.RpcShowMessage("Move to the blue point");
                        Player.GetComponent<PlayerController>().EnableMotion(true, false);
                        TaskState = ETaskState.EncodingTurnA;
                    }
                    break;
                case ETaskState.EncodingWalkA:
                    m_Delay += Time.deltaTime;

                    if (m_Delay > 2 && VRInput.GetUp(VRInputButton.Trigger))
                    {
                        UI.RpcShowMessage("Estimate the distance to the red point");
                        Player.GetComponent<PlayerController>().EnableMotion(false, true);
                        TaskState = ETaskState.EncodingTurnB;

                        Camera.main.GetComponentInChildren<FadeController>().RpcEndFade();
                        m_Dir = Player.transform.Find("ForwardIndicator").forward;

                        m_Delay = 0;
                    }
                    break;

                case ETaskState.EncodingTurnB:
                    m_Delay += Time.deltaTime;

                    if (m_Delay > 2 && VRInput.GetUp(VRInputButton.Trigger))
                    {
                        UI.RpcShowMessage("Move to the red point");
                        Player.GetComponent<PlayerController>().EnableMotion(true, false);
                        TaskState = ETaskState.Execution;

                        Camera.main.GetComponentInChildren<FadeController>().RpcBeginFade();
                    }

                    break;

                case ETaskState.Execution:

                    if (VRInput.GetUp(VRInputButton.Trigger))
                    {
                        Camera.main.GetComponentInChildren<FadeController>().RpcEndFade();

                        if (m_IsFeedback)
                        {
                            UI.RpcShowMessage("Press trigger to continue");
                            m_IsFeedback = false;

                            this.SetData();
                        }
                        else
                        {
                            TaskState = ETaskState.Ready;
                            m_TrialIndex++;
                            m_TriangleIndex++;

                            if (m_TriangleIndex == this.GetComponent<TrianglePathController>().NumTriangles)
                                m_TriangleIndex = 0;

                            this.NextTriangle();
                            m_IsFeedback = true;
                        }
                    }

                    break;
            }

        }
        else
        {
            string interaction = Player.GetComponent<PlayerController>().InteractionType.ToString();
            System.IO.File.WriteAllLines(Application.dataPath + string.Format(@"\dist_{0}_{1}.txt", interaction, m_Iteration), m_ResData.ToArray());
            m_IsEnabled = false;

            m_IsEnabled = false;
            OnTaskFinished();
        }
    }

    private void SetData()
    {
        if (m_ResData.Count == 0)
            m_ResData.Add("platform;interaction;trial;target_angle;error_distance;error_angle;travel_distance;signed_angle");

        string interaction = Player.GetComponent<PlayerController>().InteractionType.ToString();
        int triangle = m_SortedTriangles[m_TriangleIndex];
        float targetAngle = this.GetComponent<TrianglePathController>().Angles[triangle];
        Vector3 targetPos = transform.Find("PointB").position;
        Vector3 pos = new Vector3(Player.transform.position.x, targetPos.y, Player.transform.position.z);
        Vector3 dirPath = transform.Find("PointB").position - transform.Find("PointA").position;

        float angle = Vector3.Angle(dirPath, m_Dir);
        float signedAngle = Vector3.SignedAngle(dirPath, m_Dir, this.transform.up);

        float offset = Vector3.Distance(pos, targetPos);
        float travelDist = Vector3.Distance(pos, transform.Find("PointA").position);
        
        string info = string.Format("{0};{1};{2};{3};{4};{5};{6}", interaction, m_TrialIndex, targetAngle, offset, angle, travelDist, signedAngle);

        Debug.Log(info);

        m_ResData.Add(info);
    }

    public void OnPointReached(string name)
    {
        if (m_IsEnabled)
        {
            if (TaskState == ETaskState.EncodingTurnA && name == "PointA")
            {
                UI.RpcShowMessage("Turn to the red point");
                TaskState = ETaskState.EncodingWalkA;
                Player.GetComponent<PlayerController>().EnableMotion(false, true);

                Camera.main.GetComponentInChildren<FadeController>().RpcBeginFade();

                m_Delay = 0;
            }
        }
    }

    private void NextTriangle()
    {
        if (m_TrialIndex < this.GetComponent<TrianglePathController>().NumTriangles * m_NumTrials)
        {
            this.transform.Rotate(Vector3.up, Random.Range(0, 180));

            int triangle = m_SortedTriangles[m_TriangleIndex];

            Debug.Log(triangle);
            Vector3 start = transform.Find("Points").GetChild(triangle * 3 + 0).position;
            Vector3 pointA = transform.Find("Points").GetChild(triangle * 3 + 1).position;
            Vector3 pointB = transform.Find("Points").GetChild(triangle * 3 + 2).position;

            transform.Find("Start").position = start;
            transform.Find("PointA").position = pointA;
            transform.Find("PointB").position = pointB;

            Player.GetComponent<PlayerController>().SetPosition(transform.Find("Start").position);

            Player.GetComponent<PlayerController>().EnableMotion(false, true);

            TaskState = ETaskState.Ready;
            UI.RpcShowMessage("Turn to the blue point");
            //UI.RpcShowMessage("Visualize the points and press trigger");
        }
    }


}
