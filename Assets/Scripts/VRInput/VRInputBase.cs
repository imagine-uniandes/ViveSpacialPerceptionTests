using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VRInputBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public virtual bool Get(VRInputButton Button)
    {
        return false;
    }

    public virtual bool GetDown(VRInputButton Button)
    {
        return false;
    }

    public virtual bool GetUp(VRInputButton Button)
    {
        return false;
    }

    public virtual Vector2 GetAnyAxis()
    {
        return Vector2.zero;
    }

}

public enum VRInputButton
{
    One,
    Two,
    Trigger,
    Cancel
}
    

