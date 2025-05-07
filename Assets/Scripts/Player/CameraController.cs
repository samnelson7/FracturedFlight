using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour

{
    enum cameraType { FollowPlayer, FollowRoom}
    [SerializeField] private Transform player;
    [SerializeField] private cameraType CameraType;
    void FixedUpdate()
    {
        if (CameraType == cameraType.FollowRoom)
        {

        }
        else if (CameraType == cameraType.FollowPlayer)
        {
            transform.position = new Vector3(player.position.x, player.position.y+3, transform.position.z);
        }
    }
}
