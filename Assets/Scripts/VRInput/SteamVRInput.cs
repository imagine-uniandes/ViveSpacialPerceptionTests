using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SteamVRInput : VRInputBase {

    public SteamVR_TrackedObject LeftController;
    public SteamVR_TrackedObject RightController;

    protected SteamVR_Controller.Device m_LeftInput;
    protected SteamVR_Controller.Device m_RightInput;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        m_LeftInput = LeftController.index != SteamVR_TrackedObject.EIndex.None ? SteamVR_Controller.Input((int)LeftController.index) : null;
        m_RightInput = RightController.index != SteamVR_TrackedObject.EIndex.None ? SteamVR_Controller.Input((int)RightController.index) : null;
    }

    public override Vector2 GetAnyAxis()
    {
        return GetLeftAxis() + GetRightAxis();
    }

    public override bool Get(VRInputButton Button)
    {
        switch(Button)
        {
            case VRInputButton.One:
                return (m_LeftInput != null && m_LeftInput.GetPress(EVRButtonId.k_EButton_A)) || (m_RightInput != null && m_RightInput.GetPress(EVRButtonId.k_EButton_A));
            case VRInputButton.Two:
                return (m_LeftInput != null && m_LeftInput.GetPress(EVRButtonId.k_EButton_ApplicationMenu)) || (m_RightInput != null && m_RightInput.GetPress(EVRButtonId.k_EButton_ApplicationMenu));
            case VRInputButton.Trigger:
                return (m_LeftInput != null && m_LeftInput.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger)) || (m_RightInput != null && m_RightInput.GetPress(EVRButtonId.k_EButton_SteamVR_Trigger));
            case VRInputButton.Cancel:
                return (m_LeftInput != null && m_LeftInput.GetPress(EVRButtonId.k_EButton_Grip)) || (m_RightInput != null && m_RightInput.GetPress(EVRButtonId.k_EButton_Grip));
        }
        return false;
    }

    public override bool GetDown(VRInputButton Button)
    {
        switch (Button)
        {
            case VRInputButton.One:
                return (m_LeftInput != null && m_LeftInput.GetPressDown(EVRButtonId.k_EButton_A)) || (m_RightInput != null && m_RightInput.GetPressDown(EVRButtonId.k_EButton_A));
            case VRInputButton.Two:
                return (m_LeftInput != null && m_LeftInput.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu)) || (m_RightInput != null && m_RightInput.GetPressDown(EVRButtonId.k_EButton_ApplicationMenu));
            case VRInputButton.Trigger:
                return (m_LeftInput != null && m_LeftInput.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger)) || (m_RightInput != null && m_RightInput.GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger));
            case VRInputButton.Cancel:
                return (m_LeftInput != null && m_LeftInput.GetPressDown(EVRButtonId.k_EButton_Grip)) || (m_RightInput != null && m_RightInput.GetPressDown(EVRButtonId.k_EButton_Grip));
        }
        return false;
    }

    public override bool GetUp(VRInputButton Button)
    {
        switch (Button)
        {
            case VRInputButton.One:
                return (m_LeftInput != null && m_LeftInput.GetPressUp(EVRButtonId.k_EButton_A)) || (m_RightInput != null && m_RightInput.GetPressUp(EVRButtonId.k_EButton_A));
            case VRInputButton.Two:
                return (m_LeftInput != null && m_LeftInput.GetPressUp(EVRButtonId.k_EButton_ApplicationMenu)) || (m_RightInput != null && m_RightInput.GetPressUp(EVRButtonId.k_EButton_ApplicationMenu));
            case VRInputButton.Trigger:
                return (m_LeftInput != null && m_LeftInput.GetPressUp(EVRButtonId.k_EButton_SteamVR_Trigger)) || (m_RightInput != null && m_RightInput.GetPressUp(EVRButtonId.k_EButton_SteamVR_Trigger));
            case VRInputButton.Cancel:
                return (m_LeftInput != null && m_LeftInput.GetPressUp(EVRButtonId.k_EButton_Grip)) || (m_RightInput != null && m_RightInput.GetPressUp(EVRButtonId.k_EButton_Grip));
        }
        return false;
    }

    private Vector2 GetLeftAxis()
    {
        float x = (m_LeftInput != null && m_LeftInput.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad)) ? m_LeftInput.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad).x : 0;
        float y = (m_LeftInput != null && m_LeftInput.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad)) ? m_LeftInput.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad).y : 0;

        return new Vector2(x, y);
    }

    private Vector2 GetRightAxis()
    {
        float x = (m_RightInput != null && m_RightInput.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad)) ? m_RightInput.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad).x : 0;
        float y = (m_RightInput != null && m_RightInput.GetPress(EVRButtonId.k_EButton_SteamVR_Touchpad)) ? m_RightInput.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad).y : 0;

        return new Vector2(x, y);
    }

}
