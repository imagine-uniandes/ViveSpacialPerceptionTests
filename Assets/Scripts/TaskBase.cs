using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskBase : MonoBehaviour {
    public System.Action OnTaskFinished;

    protected UIController UI;
    protected PlayerController Player;
    protected VRInputBase VRInput;   
    protected bool m_IsEnabled;
    

    public bool IsEnabled
    {
        get { return m_IsEnabled; }
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual void Begin()
    {

    }

    public virtual void Begin(string message)
    {

    }


    public virtual void Begin(float radius, bool normalPath, int numTrials)
    {

    }


    public void Setup(UIController ui, PlayerController player, VRInputBase input)
    {
        Player = player;
        UI = ui;
        VRInput = input;
    }
}
