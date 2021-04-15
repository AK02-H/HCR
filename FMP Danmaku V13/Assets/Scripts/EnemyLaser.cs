using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{

    [SerializeField] private float maxLaserDistance = 100;
    public Transform laserFirePoint;
    public LineRenderer lineRend;
    private Transform xForm;

    public bool activateLaser = false;

    public Transform laserStart;
    public Transform laserEnd;
    
    private bool laserFullyExtended = false;
    private bool endingLaser = false;
    
    public float laserLaunchSpeed;
    public AnimationCurve launchSpeedCurve;

    public EdgeCollider2D hitbox;
    
    private void Awake()
    {
        xForm = GetComponent<Transform>();
    }

    void ShootLaser()
    {
        /*
        if (Physics2D.Raycast(xForm.position, transform.up))
        {
            RaycastHit2D _hit = Physics2D.Raycast(xForm.position, transform.right);
            Draw2DRay(laserFirePoint.position, _hit.point);
        }
        else
        {
            Draw2DRay(laserFirePoint.position, laserFirePoint.transform.right * maxLaserDistance);
        }*/
        
        Draw2DRay(laserStart.transform.position, laserEnd.position);

        if (!endingLaser)
        {
            UpdateHitbox(laserStart.transform.localPosition, laserEnd.transform.localPosition);
        }
        else
        {
            Vector2 worldToLocalStart = transform.InverseTransformPoint(laserStart.transform.position);;
            Vector2 worldToLocalEnd = transform.InverseTransformPoint(laserEnd.transform.position);
            Debug.Log(worldToLocalStart + worldToLocalEnd);
            
            UpdateHitbox(worldToLocalStart, worldToLocalEnd);
        }
        
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        lineRend.SetPosition(0, startPos);
        lineRend.SetPosition(1, endPos);
    }

    void UpdateHitbox(Vector2 startPos, Vector2 endPos)
    {
        
        hitbox.edgeRadius = lineRend.endWidth / 2;

        Vector2[] colliderPoints;

        colliderPoints = hitbox.points;

        colliderPoints[0] = startPos;
        colliderPoints[1] = endPos;
        
        

        hitbox.points = colliderPoints;
        Debug.Log("END POS" + endPos);
        
        
        hitbox.points[1] = new Vector2(50, 50);
    }

    // Start is called before the first frame update
    void Start()
    {
        FireOffLaser();
        
        Invoke("EndOffLaser", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (activateLaser)
        {
            if (!laserFullyExtended)
            {
                laserEnd.transform.position += -transform.up * laserLaunchSpeed * Time.deltaTime;

                if (Vector2.Distance(laserStart.transform.position, laserEnd.transform.position) >= maxLaserDistance)
                {
                    laserFullyExtended = true;
                }
            }

            if (endingLaser)
            {
                laserStart.transform.position += -transform.up * laserLaunchSpeed * Time.deltaTime;

                if (Vector2.Distance(laserStart.transform.position, laserEnd.transform.position) <= 0)
                {
                    laserFullyExtended = false;
                    activateLaser = false;
                }
            }

            ShootLaser();
        }


    }


    public void FireOffLaser()
    {
        laserStart.transform.parent = transform;
        laserEnd.transform.parent = transform;
        
        laserStart.localPosition = Vector2.zero;
        laserEnd.localPosition = Vector2.zero;
        lineRend.enabled = true;

        endingLaser = false;
        laserFullyExtended = false;

        activateLaser = true;
    }

    public void EndOffLaser()
    {
        laserStart.transform.SetParent(null, true);
        laserEnd.transform.SetParent(null, true);

        endingLaser = true;
    }
    
    
}
