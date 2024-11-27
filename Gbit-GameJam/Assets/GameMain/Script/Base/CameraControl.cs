using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    private Vector3 targetPos;
    private float smooothSpeed = 5f;
    
    private float minX = -5.5f;
    private float maxX = 5.1f;
    private float minY = 1.395f;
    private float maxY = 1.4f;

    private void Update()
    {
        
    }
    
    private void LateUpdate()
    {   
        if (target == null)
        {
            if (GameObject.FindWithTag("Player") != null)
            {
                target = GameObject.FindWithTag("Player").transform;
                Debug.Log("Find");
            }
        }

        if (target != null)
        {
          
            targetPos = target.position + new Vector3(0f, 0, 0f);
            
            float clampedX = Mathf.Clamp(targetPos.x, minX, maxX);
            float clampedY = Mathf.Clamp(targetPos.y, minY, maxY);
            
            
            Vector3 targetPosition = new Vector3(clampedX, clampedY, transform.position.z);
            
            if (transform.position != targetPosition)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, smooothSpeed * Time.deltaTime);
            }
        }
    }
}