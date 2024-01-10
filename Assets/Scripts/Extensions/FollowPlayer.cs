using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[AddComponentMenu("")]
public class FollowPlayer : CinemachineExtension
{
    [SerializeField] private Transform target;
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(Vector3.Distance(transform.position, target.position) > 5f)
        {
            float targetHeight = target.position.y + 6f;
            float currentHeight = Mathf.Lerp(transform.position.y, targetHeight, .9f * Time.deltaTime);
            Quaternion euler = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            Vector3 targetPosition = target.position - (euler * Vector3.forward) * 6;
            targetPosition.y = currentHeight;
            transform.position = targetPosition;
        }
    }
}
