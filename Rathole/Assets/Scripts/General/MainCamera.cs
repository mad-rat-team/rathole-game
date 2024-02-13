using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    void Start()
    {
        Cinemachine.CinemachineVirtualCameraBase vCam = GetComponentInChildren<Cinemachine.CinemachineVirtualCameraBase>();
        RoomManager.OnRoomChanged += () =>
        {
            vCam.PreviousStateIsValid = false;
        };
    }
}
