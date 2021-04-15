using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooledObject
{
    public string tagId;
    public Collider2D collider;    //???
    [SerializeField] private float collisionDistance;
    
    public Vector2 velocity;
    public float speed;
    public float angle;

    public float lifeTime;    //stupid ass            //Add a secure to ensure that this is never zero
    public float lifeTimerLol = 0;
    
    public AnimationCurve xOverTime;
    public AnimationCurve yOverTime;
    public AnimationCurve rotOverTime;

    public bool shouldRotate;
    
    private Vector2 prevPos;
    private Vector2 newPos;

    private bool destroyAfterLifetime;

    public Vector2 PseudoDir
    {
        get
        {
            return newPos - prevPos;
        }
    }
    
    
    public float minX, maxX, minY, maxY;    //get these from camera
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Bullet()
    {
        
    }

    public Bullet(Vector2 v, float spd, float ang, AnimationCurve curveX, AnimationCurve curveY, AnimationCurve curveRot, bool shouldDestroy = false, bool shouldRot = false, float maxLifeTime = 0)
    {
        velocity = v;
        speed = spd;
        angle = ang;

        shouldRotate = shouldRot;
        xOverTime = curveX;
        yOverTime = curveY;
        rotOverTime = curveRot;

        destroyAfterLifetime = shouldDestroy;
        
        if (maxLifeTime != 0)   
        {
            lifeTime = maxLifeTime;
        }
        
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Update is called once per frame
    void Update()
    {
        //CPU consuming?
        //Call from player instead?
        HandleColliderToggle();
        
        
        
        lifeTimerLol += Time.deltaTime;
        
        
        CalculateMovement();

        BoundCheck();
        
    }

    private void HandleColliderToggle()
    {
        if (collider)
        {


            if (Mathf.Abs(Vector2.Distance(PlayerController.instance.transform.position, transform.position)) <=
                collisionDistance)
            {
                collider.enabled = true;
            }
            else
            {
                collider.enabled = false;
            }
        }
    }

    protected virtual void CalculateMovement()
    {
        float lifeTimeProportion = lifeTimerLol / lifeTime;
        Vector2 newPos = new Vector2(xOverTime.Evaluate(lifeTimeProportion), yOverTime.Evaluate(lifeTimeProportion));

        transform.Translate(newPos * speed * Time.deltaTime);

        if (shouldRotate)
        {
            float newRot = angle * rotOverTime.Evaluate(lifeTimeProportion);
            Debug.Log(newRot);
            transform.rotation = Quaternion.Euler(0, 0, newRot);
        }

        if (lifeTimeProportion >= 1 && destroyAfterLifetime)
        {
            ReturnToPool();
        }
    }

    public virtual void SetupParams(Vector2 v, float spd, float ang, AnimationCurve curveX, AnimationCurve curveY, AnimationCurve curveRot, bool shouldRot = false, float maxLifeTime = 0)
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

    public void SetShouldDestroy(bool param)
    {
        destroyAfterLifetime = param;
    }

    public void Reset()
    {
        lifeTimerLol = 0;
    }

    public void BoundCheck()
    {
        if (transform.position.x > maxX || transform.position.x < minX || transform.position.y > maxY ||
            transform.position.y < minY)
        {
            ReturnToPool();
        }
    }
    
    public void ReturnToPool()
    {
        gameObject.SetActive(false);
        BulletPool.instance.AddBulletToPool(gameObject);
    }
}


