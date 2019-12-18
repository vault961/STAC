using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 서영균
using System;
public class CameraController : MonoBehaviour
{
    private Func<Transform> GetCameraFollowPosition;
    public void Setup(Func<Transform> GetCameraFollowPosition)
    {
        this.GetCameraFollowPosition = GetCameraFollowPosition;
    }

    public void update()
    {
        Vector3 CameraFollowPosition = GetCameraFollowPosition().position;
        // Vector3 cameraMoveDir = (CameraFollowPosition - transform.position).normalized;
        // float distance = (CameraFollowPosition - transform.position).magnitude;
        // transform.position = transform.position + cameraMoveDir * distance * cameraMoveSpeed * Time.deltaTime;

        float cameraMoveSpeed = 10f;
        transform.position = CameraFollowPosition;
        transform.rotation = Quaternion.Slerp(transform.rotation, GetCameraFollowPosition().rotation, Time.deltaTime * cameraMoveSpeed);    
    }
}
