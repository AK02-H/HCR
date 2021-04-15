using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPattern : MonoBehaviour
{
    [SerializeField] private BulletEmitter[] emitters;

    public bool shouldLoop = true;

    private bool intervalFiring = false;

    private float intervalTimer;
    private float intervalLength;


    private void Start()
    {
        
    }

    void Update()
    {
        if (intervalFiring)
        {
            if (intervalTimer >= intervalLength)
            {
                ResetPattern();

                intervalTimer = 0;
            }
            else
            {
                intervalTimer += Time.deltaTime;
            }
        }
    }

    public void FirePattern()
    {
        foreach (var emitter in emitters)
        {
            emitter.BeginEmission(shouldLoop);    
        }
    }

    public void StopFiring()
    {
        intervalFiring = false;
        intervalTimer = 0;
        
        foreach (var emitter in emitters)
        {
            emitter.EndEmission();
        }
    }

    private void ResetPattern()
    {
        foreach (var emitter in emitters)
        {
            emitter.ResetEmission();
        }
    }
    
    public void FireWithInterval(float time)
    {
        intervalTimer = 0;
        intervalFiring = true;
        
        foreach (var emitter in emitters)
        {
            emitter.BeginEmission(false);  
        }
    }
}
