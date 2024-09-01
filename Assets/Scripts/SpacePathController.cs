using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePathController : MonoBehaviour {
    const int NUM_TRIANGLES = 4;
    int NUM_SEGMENTS = 50;
    public int Radius;
    
    LineRenderer line;
    public GameObject PointPrefab;
    protected Vector3[,] Points;
    protected float[] Angles = new float[NUM_TRIANGLES] { 120.0f, -120.0f, 60.0f, -60.0f};


    void Start()
    {
        line = gameObject.GetComponent<LineRenderer>();
        
        line.positionCount= NUM_SEGMENTS + 1;
        line.useWorldSpace = false;
        CreatePoints();

        Points = new Vector3[NUM_TRIANGLES, 3];

        for (int i = 0; i < NUM_TRIANGLES; i++)
        {
            Points[i,0] = this.transform.position;
            Points[i,1] = this.transform.position + Vector3.forward * Radius;
            Vector3 segment = Points[i,1] - Points[i,0];
            Points[i, 2] = Quaternion.AngleAxis(Angles[i], this.transform.up) * segment;
        }
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (NUM_SEGMENTS + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * Radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * Radius;

            line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / NUM_SEGMENTS);
        }
    }


	// Update is called once per frame
	void Update () {
        for (int i = 0; i < NUM_TRIANGLES; i++)
        {
            Debug.DrawLine(Points[i,0], Points[i,1], Color.red);
            Debug.DrawLine(Points[i,1], Points[i,2], Color.cyan);
            Debug.DrawLine(Points[i,0], Points[i,2], Color.blue);
        }
    }
}
