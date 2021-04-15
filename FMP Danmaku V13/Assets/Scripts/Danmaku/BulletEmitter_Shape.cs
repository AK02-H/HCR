using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEmitter_Shape : BulletEmitter
{
    public GameObject trackBullet;
    public BulletMatrix matrix;
    // Start is called before the first frame update

    public AnimationCurve lingerValue;

    public bool spawnAllAtOnce = true;
    private int matrixIndex = 0;
    private BulletMatrix currentMatrix;
    
    protected override void FireBullets()
    {
        Debug.Log("Fire");

        if (!rotateBulletIndependently)
        {
            foreach (var angle in anglesToFire)
            {

                if (spawnAllAtOnce)
                {
                    BulletMatrix bm =
                        Instantiate(matrix.gameObject, transform.position,
                            Quaternion.Euler(0, 0, angle + transform.rotation.z * -360)).GetComponent<BulletMatrix>();
                    bm.SetupParams
                        (velocity, speed, angle + transform.rotation.z * -360, xCurve, yCurve, rotCurve, bulletsShouldRotate, bulletLifeTime);

                    foreach (Transform xForm in bm.positions)
                    {
                        GameObject b = BulletPool.instance.SpawnFromPool(trackBullet, transform.position, Quaternion.identity);
                
                        b.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, xCurve, yCurve,
                            rotCurve,
                            bulletsShouldRotate, bulletLifeTime);
                        b.GetComponent<Bullet_Tracking>().SetTrackedObject(xForm);
                        b.GetComponent<Bullet_Tracking>().SetLerpCurve(lingerValue);
                    }
                }
                else
                {
                    if (matrixIndex <= 0)
                    {
                        BulletMatrix bm =
                            Instantiate(matrix.gameObject, transform.position,
                                Quaternion.Euler(0, 0, angle + transform.rotation.z * -360)).GetComponent<BulletMatrix>();
                        bm.SetupParams
                            (velocity, speed, angle + transform.rotation.z * -360, xCurve, yCurve, rotCurve, bulletsShouldRotate, bulletLifeTime);

                        currentMatrix = bm;
                        
                        GameObject b = BulletPool.instance.SpawnFromPool(trackBullet, transform.position, Quaternion.identity);
                
                        b.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, xCurve, yCurve,
                            rotCurve,
                            bulletsShouldRotate, bulletLifeTime);
                        b.GetComponent<Bullet_Tracking>().SetTrackedObject(currentMatrix.positions[matrixIndex]);
                        b.GetComponent<Bullet_Tracking>().SetLerpCurve(lingerValue);

                        matrixIndex++;

                        if (matrixIndex > currentMatrix.positions.Length)
                        {
                            //done, restart cycle
                            matrixIndex = 0;
                        }
                        
                    }
                    else
                    {
                        GameObject b = BulletPool.instance.SpawnFromPool(trackBullet, transform.position, Quaternion.identity);
                
                        b.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, xCurve, yCurve,
                            rotCurve,
                            bulletsShouldRotate, bulletLifeTime);
                        b.GetComponent<Bullet_Tracking>().SetTrackedObject(currentMatrix.positions[matrixIndex]);
                        b.GetComponent<Bullet_Tracking>().SetLerpCurve(lingerValue);

                        matrixIndex++;

                        if (matrixIndex >= currentMatrix.positions.Length)
                        {
                            //done, restart cycle
                            matrixIndex = 0;
                        }
                    }
                }
                
                

            
                //Instantiate
            }
            
        }
        else
        {
            foreach (var angle in anglesToFire)
            {

                if (spawnAllAtOnce)
                {
                    Transform clone = Instantiate(bullet, transform.position, Quaternion.identity).transform;
                    clone.name = "Empty Holder";
                    clone.parent = BulletPool.instance.transform;
                    clone.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, xCurve, yCurve,
                        rotCurve,
                        false, bulletLifeTime);
                
                    //have separate curve for parent rotation?
                
                    BulletMatrix bm =
                        Instantiate(matrix.gameObject, transform.position,
                            Quaternion.Euler(0, 0, angle + transform.rotation.z * -360)).GetComponent<BulletMatrix>();
                    bm.SetupParams
                        (velocity, speed, angle + transform.rotation.z * -360, new AnimationCurve(), new AnimationCurve(), rotCurve, bulletsShouldRotate, bulletLifeTime);


                    bm.transform.parent = clone;
                
                    foreach (Transform xForm in bm.positions)
                    {
                        Debug.Log("Send tracking bullet");
                        GameObject b = BulletPool.instance.SpawnFromPool(trackBullet, transform.position, Quaternion.identity);
                
                        AnimationCurve blankCurve = new AnimationCurve();
                    
                        b.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, blankCurve, blankCurve,
                            rotCurve,
                            bulletsShouldRotate, bulletLifeTime);
                        b.GetComponent<Bullet_Tracking>().SetTrackedObject(xForm);
                    }
                }
                else
                {
                    if (matrixIndex <= 0)
                    {
                        Transform clone = Instantiate(bullet, transform.position, Quaternion.identity).transform;
                        clone.name = "Empty Holder";
                        clone.parent = BulletPool.instance.transform;
                        clone.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, xCurve, yCurve,
                            rotCurve,
                            false, bulletLifeTime);
                
                        //have separate curve for parent rotation?
                
                        BulletMatrix bm =
                            Instantiate(matrix.gameObject, transform.position,
                                Quaternion.Euler(0, 0, angle + transform.rotation.z * -360)).GetComponent<BulletMatrix>();
                        bm.SetupParams
                            (velocity, speed, angle + transform.rotation.z * -360, new AnimationCurve(), new AnimationCurve(), rotCurve, bulletsShouldRotate, bulletLifeTime);


                        bm.transform.parent = clone;

                        currentMatrix = bm;
                
                        
                        Debug.Log("Send tracking bullet");
                        GameObject b = BulletPool.instance.SpawnFromPool(trackBullet, transform.position, Quaternion.identity);
                
                        AnimationCurve blankCurve = new AnimationCurve();
                    
                        b.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, blankCurve, blankCurve,
                            rotCurve,
                            bulletsShouldRotate, bulletLifeTime);
                        b.GetComponent<Bullet_Tracking>().SetTrackedObject(currentMatrix.positions[matrixIndex]);
                        
                        matrixIndex++;
                        
                        if (matrixIndex >= currentMatrix.positions.Length)
                        {
                            //done, restart cycle
                            matrixIndex = 0;
                        }
                    }
                    else
                    {
                        Debug.Log("Send tracking bullet");
                        GameObject b = BulletPool.instance.SpawnFromPool(trackBullet, transform.position, Quaternion.identity);
                
                        AnimationCurve blankCurve = new AnimationCurve();
                    
                        b.GetComponent<Bullet>().SetupParams(velocity, speed, angle + transform.rotation.z * -360, blankCurve, blankCurve,
                            rotCurve,
                            bulletsShouldRotate, bulletLifeTime);
                        b.GetComponent<Bullet_Tracking>().SetTrackedObject(currentMatrix.positions[matrixIndex]);
                        
                        matrixIndex++;
                        
                        if (matrixIndex >= currentMatrix.positions.Length)
                        {
                            //done, restart cycle
                            matrixIndex = 0;
                        }
                    }
                }
                
                
                
                

            
                //Instantiate
            }
        }
        
        

        AudioManager.instance.PlaySfx(emissionSfx);
    }
    
    
}
