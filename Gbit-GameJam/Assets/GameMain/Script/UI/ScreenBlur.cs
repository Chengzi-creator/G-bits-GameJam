
using System;
using System.Timers;
using UnityEngine;

public class ScreenBlur : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material material;
    public float BreathTime = 0.5f;
    private float BreathTimer = 0f;
    
    public float minPower = 8f;
    public float maxPower = 10f;
    private bool isUp = false;
    
    
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        spriteRenderer.enabled = false;
    }

    public void OpenBlur()
    {
        spriteRenderer.enabled = true;
    }
    public void CloseBlur()
    {
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        //呼吸效果
        BreathTimer += Time.deltaTime;
        if (BreathTimer >= BreathTime)
        {
            BreathTimer = 0f;
            isUp = !isUp;
        }
        if(isUp)
            material.SetFloat("_Power",Mathf.Lerp(minPower,maxPower,BreathTimer/BreathTime));
        else
            material.SetFloat("_Power",Mathf.Lerp(maxPower,minPower,BreathTimer/BreathTime));
        
    }
}
        
    
