using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderFPS : MonoBehaviour {
    // Use this for initialization
    float deltaTime;
    float measureTime;
    List<float> data;

	void Start () {
        data = new List<float>();
	}
	
	// Update is called once per frame
	void Update () {
        
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);

        this.GetComponent<TextMesh>().text = text;

       /* measureTime += Time.deltaTime;
        data.Add(fps);

        if (measureTime > 20)
        {
            string[] info = new string[data.Count];
            for (int i = 0; i < data.Count; i++)
                info[i] = data[i].ToString();

           if(!System.IO.File.Exists(@"C:\Users\Imagine\fps.csv")) 
                System.IO.File.WriteAllLines(@"C:\Users\Imagine\fps.csv", info);
        }*/
    }
}
