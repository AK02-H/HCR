using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter_SubEmitter : BulletEmitter
{
    public GameObject subEmittingBullet;

    public GameObject emptyBullet; //not always used
    [Header("Secondary Emitter Parameters")]
    
    public GameObject bullet_2;
    
    public bool isFiring_2 = false;
    public bool shouldLoop_2 = false;
    
    public float interval_2;
    private float timer_2;
    public float startDelay_2;
    private bool delayPassed_2 = false;
    
    public Vector2 velocity_2;
    public float speed_2;
    //public float angle;

    public AnimationCurve xCurve_2, yCurve_2, rotCurve_2;
    public bool bulletsShouldRotate_2;

    public float bulletLifeTime_2 = 0;

    public float[] anglesToFire_2;

    public int emissionSfx_2;

    public bool rotateBulletIndependently_2;

    public bool destroyAfterLifetime_2 = false; //do this

    protected override void FireBullets()
    {
        //if rotate independently, create a clone bullet, hide graphics then spawn other bullets with the set rotation as children

        if (!rotateBulletIndependently)
        {
            
            foreach (var angle in anglesToFire)
            {
                GameObject initialBullet = BulletPool.instance.SpawnFromPool(subEmittingBullet, transform.position, Quaternion.identity);
                initialBullet.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, xCurve, yCurve,
                    rotCurve,
                    bulletsShouldRotate, bulletLifeTime);

                initialBullet.GetComponent<Bullet>().SetShouldDestroy(destroyAfterLifetime);

                Debug.LogWarning(initialBullet.GetComponent<BulletEmitter>() != null);
                initialBullet.GetComponent<BulletEmitter>().SetupEmitterParams(bullet_2, isFiring_2, shouldLoop_2, interval_2,
                    timer_2, startDelay_2, delayPassed_2,
                    velocity_2, speed_2, xCurve_2, yCurve_2, rotCurve_2, bulletsShouldRotate_2, bulletLifeTime_2,
                    anglesToFire_2, emissionSfx_2, rotateBulletIndependently_2, destroyAfterLifetime_2);
                
                

                //Instantiate
            }
            
            
            
        }
        else
        {
            foreach (var angle in anglesToFire)
            {
                Transform clone = Instantiate(emptyBullet, transform.position, Quaternion.identity).transform;
                clone.parent = BulletPool.instance.transform;
                clone.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, xCurve, yCurve,
                    rotCurve,
                    bulletsShouldRotate, bulletLifeTime);

                GameObject blt = BulletPool.instance.SpawnFromPool(subEmittingBullet, transform.position, Quaternion.identity);
                
                blt.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, xCurve, yCurve,
                    rotCurve,
                    bulletsShouldRotate, bulletLifeTime);
                
                blt.transform.parent = clone;

                blt.GetComponent<Bullet>().SetShouldDestroy(destroyAfterLifetime);
                
                blt.GetComponent<BulletEmitter>().SetupEmitterParams(bullet_2, isFiring_2, shouldLoop_2, interval_2,
                    timer_2, startDelay_2, delayPassed_2,
                    velocity_2, speed_2, xCurve_2, yCurve_2, rotCurve_2, bulletsShouldRotate_2, bulletLifeTime_2,
                    anglesToFire_2, emissionSfx_2, rotateBulletIndependently_2, destroyAfterLifetime_2);
                
                
                //Instantiate
            }
            

        }
        
        

        AudioManager.instance.PlaySfx(emissionSfx);
    }
}
