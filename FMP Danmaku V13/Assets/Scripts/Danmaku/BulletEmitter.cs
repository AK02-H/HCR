using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;

public class BulletEmitter : MonoBehaviour
{
    public GameObject bullet;

    //public int numOfVolleys = 1;

    public bool isFiring = false;
    public bool shouldLoop = false;
    
    public float interval;
    private float timer;
    public float startDelay;
    private bool delayPassed = false;
    
    public Vector2 velocity;
    public float speed;
    //public float angle;

    public AnimationCurve xCurve, yCurve, rotCurve;
    public bool bulletsShouldRotate;

    public float bulletLifeTime = 0;

    public float[] anglesToFire;

    public int emissionSfx;

    public bool rotateBulletIndependently;

    public bool destroyAfterLifetime = false; //do this to EVERY EMITTER TYPE
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetupEmitterParams(GameObject bllt, bool a, bool b, float c, float d, float e, bool f, Vector2 g, float h, AnimationCurve i,
        AnimationCurve j, AnimationCurve k, bool l, float m, float[] n, int o, bool p, bool q)
    {
        bullet = bllt;
        
        isFiring = a;
        shouldLoop = b;

        interval = c;
        timer = d;
        startDelay = e;
        delayPassed = f;

        velocity = g;
        speed = h;

        xCurve = i;
        yCurve = j;
        rotCurve = k;
        bulletsShouldRotate = l;

        bulletLifeTime = m;

        anglesToFire = n;

        emissionSfx = o;

        rotateBulletIndependently = p;

        destroyAfterLifetime = q;
    }
    
    // Update is called once per frame
    void Update()
    {

        if (isFiring)
        {
            if (delayPassed)
            {
                if (timer >= interval)
                {
                    FireBullets();


                    if (shouldLoop)
                    {
                        timer = 0;
                    }
                    else
                    {
                        EndEmission();
                    }
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
            else
            {
                if (timer >= startDelay)
                {
                    delayPassed = true;
                    timer = interval;
                }
                else
                {
                    timer += Time.deltaTime;
                }
            }
        }

    }

    protected virtual void FireBullets()
    {
        //if rotate independently, create a clone bullet, hide graphics then spawn other bullets with the set rotation as children

        if (!rotateBulletIndependently)
        {
            
            foreach (var angle in anglesToFire)
            {
                GameObject blt = BulletPool.instance.SpawnFromPool(bullet, transform.position, Quaternion.identity);
                blt.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, xCurve, yCurve,
                    rotCurve,
                    bulletsShouldRotate, bulletLifeTime);
                blt.GetComponent<Bullet>().SetShouldDestroy(destroyAfterLifetime);



                //Instantiate
            }
            
            
            
        }
        else
        {
            foreach (var angle in anglesToFire)
            {
                Transform clone = Instantiate(gameObject, transform.position, Quaternion.identity).transform;    //THIS IS WRONG, CHANGE THIS LATER
                clone.parent = BulletPool.instance.transform;
                clone.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, xCurve, yCurve,
                    rotCurve,
                    bulletsShouldRotate, bulletLifeTime);


                GameObject blt = BulletPool.instance.SpawnFromPool(bullet, transform.position, Quaternion.identity);
                
                blt.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, xCurve, yCurve,
                    rotCurve,
                    bulletsShouldRotate, bulletLifeTime);

                blt.GetComponent<Bullet>().SetShouldDestroy(destroyAfterLifetime);
                
                blt.transform.parent = clone;

                //Instantiate
            }
            

        }
        
        

        AudioManager.instance.PlaySfx(emissionSfx);
    }

    public void BeginEmission(bool loop = true)
    {
        delayPassed = false;
        timer = interval;

        shouldLoop = loop;
        
        isFiring = true;
    }

    public void EndEmission()
    {
        isFiring = false;
    }

    public void ResetEmission()
    {
        EndEmission();
        BeginEmission();
    }


    private void OnDrawGizmosSelected()
    {
        /*
        Gizmos.color = Color.red;
        
        
        foreach (var angle in anglesToFire)
        {
            Quaternion a = Quaternion.Euler(0, 0, angle + transform.rotation.z * -360);
            Vector2 dir = a.eulerAngles;
            
            
            dir *= 2;
            Vector2 to = new Vector2(transform.position.x, transform.position.y) + dir;
            Gizmos.DrawLine(transform.position, to);
            
            //Gizmos.DrawRay(transform.position, dir);
        }*/
    }
}

[Serializable]
public class BulletGroup
{
    public GameObject bullet;
    public int bulletCount;
    
    public float minAngle;
    public float maxAngle;

    public Vector2 spawnPos;
    public float randDegree_spawnPos;

    public float interval;
}

