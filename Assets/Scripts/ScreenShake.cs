using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource _impulseSource;

    public static ScreenShake Instance;
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }
    public void Shake(float intensity = 1f)
    {
        _impulseSource.GenerateImpulse(intensity);
    }
}
