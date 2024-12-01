using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{   
    public GameObject mainCameraObject;
    public float shakeDuration = 0f;
    public float shakeMagnitude = 0.1f;
    public float shakeFrequency = 2.0f;

    private CameraControl CameraControl;
    private float currentShakeDuration = 0f;
    private bool trigger;
    public bool Found = false;
    void Start()
    {
        
    }

    void Update()
    {   
        if (mainCameraObject != null && !Found)
        {
            if (CameraControl == null)
            {
                CameraControl = mainCameraObject.GetComponent<CameraControl>();
            }
            else
            {
                Found = true;
            }
        }
        else if(mainCameraObject == null)
        {
            mainCameraObject = GameObject.FindWithTag("MainCamera");
        }
        
        if (trigger)
        {
            if (currentShakeDuration > 0)
            {
                transform.localPosition =  CameraControl.targetPosition + Random.insideUnitSphere * shakeMagnitude;
                currentShakeDuration -= Time.deltaTime;
            }
            else
            {
                currentShakeDuration = 0f;
                transform.localPosition = CameraControl.targetPosition;
                trigger = false;
            }
        }
    }
    
    public void TriggerShake(float duration, float magnitude)
    {
        trigger = true;
        shakeDuration = duration;
        shakeMagnitude = magnitude;
        currentShakeDuration = shakeDuration;
    }
}
