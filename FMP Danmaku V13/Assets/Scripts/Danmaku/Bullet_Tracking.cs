using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Tracking : Bullet
{
    public Transform tracked;

    public AnimationCurve positionLerp;

    protected override void CalculateMovement()
    {
        float lifeTimeProportion = lifeTimerLol / lifeTime;
        Vector2 newPos = Vector2.Lerp(transform.position, tracked.position, positionLerp.Evaluate(lifeTimeProportion)); // + new Vector2(xOverTime.Evaluate(lifeTimeProportion), yOverTime.Evaluate(lifeTimeProportion));

        //transform.Translate(newPos * speed * Time.deltaTime);
        transform.position = newPos;

        if (shouldRotate)
        {
            float newRot = angle * rotOverTime.Evaluate(lifeTimeProportion);
            transform.rotation = Quaternion.Euler(0, 0, newRot);
        }
    }
    
    public override void SetupParams(Vector2 v, float spd, float ang, AnimationCurve curveX, AnimationCurve curveY, AnimationCurve curveRot, bool shouldRot = false, float maxLifeTime = 0)
    {
//        Debug.LogWarning(ang);
        
        velocity = v;
        speed = spd;
        angle = ang;

        shouldRotate = shouldRot;
        xOverTime = curveX;
        yOverTime = curveY;
        rotOverTime = curveRot;

        if (maxLifeTime != 0)
        {
            lifeTime = maxLifeTime;
        }
        
        transform.rotation = Quaternion.Euler(0, 0, angle);
        
    }

    public void SetTrackedObject(Transform obj)
    {
        tracked = obj;
    }

    public void SetLerpCurve(AnimationCurve curve)
    {
        positionLerp = curve;
    }
}
