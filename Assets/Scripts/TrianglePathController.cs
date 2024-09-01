
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrianglePathController : MonoBehaviour
{

    public float[] Angles;
    protected TriangleSet[] TrianglePoints;
    protected float m_Radius;
    protected int m_Sign = 1;

    public int NumTriangles
    {
        get { return TrianglePoints.Length; }
    }

    public GameObject PointPrefab;
    protected Vector3[] Points;


    // Use this for initialization
    void Start()
    {
        Points = new Vector3[Angles.Length];
        TrianglePoints = new TriangleSet[Angles.Length];
    }

    public void CreateTriangles(bool isNormalPath, float radius)
    {
        m_Radius = radius;
        for (int i = 0; i < Angles.Length; i++)
        {
            Points[i] = this.transform.position + Quaternion.AngleAxis(Angles[i] * m_Sign, this.transform.up) * this.transform.forward * radius;
            m_Sign = m_Sign * -1;
        }

        Vector3 point = this.transform.position + this.transform.forward * radius;
        Vector3 center = this.transform.position;

        for (int i = 0; i < Angles.Length; i++)
        {
            if (isNormalPath)
                TrianglePoints[i] = new TriangleSet() { p0 = Points[i], p1 = point, p2 = center };
            else
                TrianglePoints[i] = new TriangleSet() { p0 = center, p1 = point, p2 = Points[i] };
        }

        Transform points = this.transform.Find("Points");

        if (points.childCount == 0)
        {
            for (int i = 0; i < TrianglePoints.Length; i++)
            {
                GameObject point0 = GameObject.Instantiate(PointPrefab, this.transform.position, Quaternion.identity);
                GameObject point1 = GameObject.Instantiate(PointPrefab, this.transform.position, Quaternion.identity);
                GameObject point2 = GameObject.Instantiate(PointPrefab, this.transform.position, Quaternion.identity);

                point0.name = string.Format("Point{0}{1}", i, 0);
                point0.transform.parent = points;
                point0.transform.position = TrianglePoints[i].p0;
                point1.name = string.Format("Point{0}{1}", i, 1);
                point1.transform.parent = points;
                point1.transform.position = TrianglePoints[i].p1;
                point2.name = string.Format("Point{0}{1}", i, 2);
                point2.transform.parent = points;
                point2.transform.position = TrianglePoints[i].p2;
            }
        }
        else
        {
            for (int i = 0; i < TrianglePoints.Length; i++)
            {
                points.Find(string.Format("Point{0}{1}", i, 0)).position = TrianglePoints[i].p0;
                points.Find(string.Format("Point{0}{1}", i, 1)).position = TrianglePoints[i].p1;
                points.Find(string.Format("Point{0}{1}", i, 2)).position = TrianglePoints[i].p2;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Radius > 0)
        {
            Transform p = transform.Find("Points");
            for (int i = 0; i < p.childCount; i = i + 3)
            {
                Debug.DrawLine(p.GetChild(i).position, p.GetChild(i + 1).position, Color.blue);
                Debug.DrawLine(p.GetChild(i + 1).position, p.GetChild(i + 2).position, Color.red);
                Debug.DrawLine(p.GetChild(i).position, p.GetChild(i + 2).position, Color.green);
            }

            DrawCircle(m_Radius);
        }
    }

    void DrawCircle(float radius)
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        Vector3 old = this.transform.position;
        Vector3 first = this.transform.position;

        for (int i = 0; i < 50; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            if (i != 0)
                Debug.DrawLine(old, new Vector3(x, 0, z));
            else
                first = new Vector3(x, 0, z);

            old = new Vector3(x, 0, z);
            angle += (360f / 50);
        }

        Debug.DrawLine(old, first);
    }
}
