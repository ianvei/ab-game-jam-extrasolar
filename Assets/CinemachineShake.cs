using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake instance;
    [SerializeField] private float globalShakeForce = 1f;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        };
    }

   public void CameraShake(CinemachineImpulseSource impulseSource, float shake)
    {
        impulseSource.GenerateImpulseWithForce(shake);
    }
}
