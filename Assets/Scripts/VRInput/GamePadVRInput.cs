using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePadVRInput : VRInputBase {
    
    public override bool Get(VRInputButton button)
    {
        switch (button)
        {
            case VRInputButton.One:
                return Input.GetButton("Jump");
            case VRInputButton.Two:
                return Input.GetButton("Submit");
            case VRInputButton.Trigger:
                return Input.GetButton("Cancel");
            default:
                return false;
        }
    }

    public override bool GetDown(VRInputButton button)
    {
        switch (button)
        {
            case VRInputButton.One:
                return Input.GetButtonDown("Jump");
            case VRInputButton.Two:
                return Input.GetButtonDown("Submit");
            case VRInputButton.Trigger:
                return Input.GetButtonDown("Cancel");
            default:
                return false;
        }
    }

    public override bool GetUp(VRInputButton button)
    {
        switch (button)
        {
            case VRInputButton.One:
                return Input.GetButtonUp("Jump");
            case VRInputButton.Two:
                return Input.GetButtonUp("Submit");
            case VRInputButton.Trigger:
                return Input.GetButtonUp("Cancel");
            default:
                return false;
        }
    }

    public override Vector2 GetAnyAxis()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
}
