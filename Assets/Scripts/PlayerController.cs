using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public SteamVRInput VRInput;
    public Transform GearHeadTracker;
    public Transform ViveHeadTracker;

    public System.Action<string> OnPointReached;
    public System.Action<string> OnObjectReached;
    public EInteractionType InteractionType;

    protected bool IsTurningOn = true;
    protected bool IsWalkingOn = true;

    protected Transform CenterEye;
    // Use this for initialization
    void Start()
    {
        transform.Find("OVRCameraRig").gameObject.SetActive(false);
        this.GetComponent<OmniMovementComponent>().cameraReference = ViveHeadTracker;
    }

    // Update is called once per frame
    void Update()
    {
        switch (InteractionType)
        {
            case EInteractionType.GamePad:
                UseStandardMotion();
                break;
            case EInteractionType.Treadmill:
                UseOmniMotion();
                break;
        }
    }

    void UseNaturalWalking()
    {
        Vector3 pos = Camera.main.transform.position;
        this.transform.position = new Vector3(pos.x, this.transform.position.y, pos.z);
    }

    void UseStandardMotion()
    {
        //bool isRightHanded = false;
        Vector3 inputDir = Vector3.forward;

        /*if (VRInput.RightController != null && VRInput.RightController.index != SteamVR_TrackedObject.EIndex.None)
            isRightHanded = SteamVR_Controller.Input((int)VRInput.RightController.index).GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad) != Vector2.zero;

        inputDir = isRightHanded ? VRInput.RightController.transform.forward : VRInput.LeftController.transform.forward;*/

       inputDir = ViveHeadTracker.forward;
        
        float moveVertical = IsWalkingOn ? VRInput.GetAnyAxis().y : 0;
        Vector3 forwardDir = new Vector3(inputDir.x, 0, inputDir.z);

        if(forwardDir != Vector3.zero)
        transform.Find("ForwardIndicator").rotation = Quaternion.LookRotation(forwardDir, this.transform.up);

        this.GetComponent<CharacterController>().Move(forwardDir * moveVertical * Speed * Time.deltaTime);    
    }

    // Gets Omni Movement Vectors from the OmniMovementComponent and uses them to Move the Player
    void UseOmniMotion()
    {
#if UNITY_EDITOR
		this.GetComponent<OmniMovementComponent>().developerMode = true;
#else
        this.GetComponent<OmniMovementComponent>().developerMode = false;
#endif
        OmniMovementComponent omniMove = this.GetComponent<OmniMovementComponent>();
        CharacterController characterMove = this.GetComponent<CharacterController>();

		if (omniMove.developerMode)
			omniMove.DeveloperModeUpdate();		
        else if (omniMove.omniFound)
            omniMove.GetOmniInputForCharacterMovement();

        if (omniMove.GetForwardMovement() != Vector3.zero)
            characterMove.Move( IsWalkingOn ? omniMove.GetForwardMovement() : Vector3.zero);
    }

    public void SetPosition(Vector3 pos)
    {
        if (this.InteractionType == EInteractionType.GamePad)
        {
            this.transform.position = pos;
            this.GetComponent<CharacterController>().Move(Vector3.one * 0.01f);
        }
        else
        {
            this.transform.position = new Vector3(pos.x - Camera.main.transform.localPosition.x, this.transform.position.y, pos.z - Camera.main.transform.localPosition.z);
            this.GetComponent<CharacterController>().Move(Vector3.one * 0.01f);
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.name == "PointA" || hit.gameObject.name == "PointB" || hit.gameObject.name == "Start")
            OnPointReached(hit.gameObject.name);

        if (hit.gameObject.tag == "TargetObject")
            OnObjectReached(hit.gameObject.name);
    }

    public void EnableMotion(bool walking, bool turning)
    {
        IsWalkingOn = walking;
        IsTurningOn = turning;
    }
}
