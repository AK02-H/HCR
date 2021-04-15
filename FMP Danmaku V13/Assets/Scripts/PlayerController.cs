﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;
 using UnityEngine.InputSystem;

 public enum PlayerState{Neutral, Speen, Aiming, Rebound}
public class PlayerController : MonoBehaviour
{
    public static PlayerController instance { get; private set; }

    private PlayerControls controls;


    [SerializeField] private int lifeCount = 3;
    public int LifeCount
    {
        get => lifeCount;
        set => lifeCount = value;
    }

    public float speed;
    public bool canMove = true;


    [Space(2)]
    public GameObject aimArrow;

    [Header("Speen Params")]
    public float spinSpeedMultiplier;
    private float spinMovement;
    private float spinTimer;
    [SerializeField] private float spinTime;
    [SerializeField] private AnimationCurve spinCurve;

    [Header("Rebound Params")]
    public float reboundSpeedMultiplier;
    private float reboundMovement;
    private float reboundTimer;
    [SerializeField] private float reboundTime;
    [SerializeField] private AnimationCurve reboundCurve;
    private Vector2 bounceDir;

    [Space] private float aimAngle;
    
    [Space]
    public Transform deathPos;
    private bool respawning;
    private float respawnTimer;
    public AnimationCurve respawnCurve;

    [SerializeField] float respawnInvincibility = 3;
    private bool invincibleFlashing = false;
    
    public SpriteRenderer sRend;
    
    public Collider2D hitbox;

    private float newAxis = 0.0f;
    
    [SerializeField] private float _atk = 1;
    public float AttackStat    //ask gregor
    {
        get
        {
            return _atk;
        }
        set
        {
            _atk = value;
        }
    }
    
    public PlayerState state = PlayerState.Neutral;

    public float REFRACTIVE_INDEX;

    public Rigidbody2D rb;
    
    
    [Range(0, 1)]
    public float focusRate;    //rate of slowdown when holding
    
    private float xAxis, yAxis;
    private Vector2 movement, direction;

    public Animator anim;

    public ParticleSystem[] weapons;



    [SerializeField] private float testFloat;

    public float aimTime;
    private float aimTimer;
    
    [Header("Debug UI")]
    [SerializeField] private Text stateText;

    [SerializeField] private bool invulnerable = false;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        controls = new PlayerControls();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //canMove = false;
        deathPos = GameObject.FindGameObjectsWithTag("DeathPosition")[0].transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {

            switch (state)
            {
                case(PlayerState.Neutral):
                    
                    
                    float baseSpeed = speed;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        baseSpeed *= focusRate;
                    }

                    xAxis = Input.GetAxisRaw("Horizontal");
                    yAxis = Input.GetAxisRaw("Vertical");

                    //controls.Gameplay.Move.performed += ctx => movement = ctx.ReadValue<Vector2>().normalized;
                    //controls.Gameplay.Move.canceled += ctx => movement = Vector2.zero;
                    
                    movement = new Vector2(xAxis, yAxis).normalized;


                    //direction = (movement * baseSpeed * Time.deltaTime);

                    //transform.Translate(movement * baseSpeed * Time.deltaTime);

                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        Speen();
                        Debug.Log("Spinny spin");
                    }

                    ProcessFiring();
                    
                    
                    break;
                case(PlayerState.Speen):

                    
                    
                    spinTimer += Time.deltaTime;

                    spinMovement = spinCurve.Evaluate(spinTimer / spinTime) * spinSpeedMultiplier;
                    

                    if (spinTimer > spinTime)
                    {
                        Debug.Log("NEUTRALISED");
                        anim.SetTrigger("EndSpeen");
                        state = PlayerState.Neutral;
                    }
                    
                    break;
                case(PlayerState.Aiming):
                    
                    //Vector3 newRot = Quaternion.Euler()
                    
                    /*xAxis = Input.GetAxis("Horizontal");
                    yAxis = Input.GetAxis("Vertical");

                    float newAxis = xAxis + yAxis;

                    Quaternion rot = Quaternion.Euler(0, 0, (aimArrow.transform.rotation.z + newAxis) * 100);



                    aimArrow.transform.rotation = rot;*/

                    //xAxis = Input.GetAxis("Horizontal");


                    newAxis = newAxis + Input.GetAxis("Horizontal") * 5f;

                    aimArrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, newAxis);
                    
                    aimTimer += Time.deltaTime;
                    if (aimTimer > aimTime)
                    {
                        Vector2 vel = bounceDir * 40;
                        Debug.Log("BOUNCE FORCE: " + vel);
                        rb.velocity = vel;
                        
                        anim.SetTrigger("EndSpeen");
                        GameManager.Instance.EndSlowdown();
                        state = PlayerState.Rebound;
                    }
                    break;
                case(PlayerState.Rebound):

                    reboundTimer += Time.deltaTime;

                    reboundMovement = reboundCurve.Evaluate(reboundTimer / reboundTime) * reboundSpeedMultiplier;
                    

                    if (reboundTimer > reboundTime)
                    {
                        Debug.Log("NEUTRALISED");
                        state = PlayerState.Neutral;
                    }
                    
                    break;
                default:
                    break;
            }

            

        }
        else
        {
            if (respawning)
            {
                transform.Translate(Vector2.up * speed * 3 * Time.deltaTime * respawnCurve.Evaluate(respawnTimer / 0.3f));
                
                if (respawnTimer >= 0.45f)
                {
                    RespawnAnimEnd();
                }
                else
                {
                    respawnTimer += Time.deltaTime;
                }
            }
            else
            {
                
            }
        }

        if (invincibleFlashing)
        {
            float newAlph = 0.1f * (Mathf.Sin((Time.time - 0) / 0.01f) + 0.9f);
            Color tempCol = new Color(sRend.color.r, sRend.color.g, sRend.color.b, newAlph);
            sRend.color = tempCol;
        }
        
        
        
        //DEBUG
        if(stateText) stateText.text = "State: " + state.ToString();
        

    }

    

    private void FixedUpdate()
    {
        switch (state)
        {
            case(PlayerState.Neutral):
                rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
                break;
            case(PlayerState.Speen):
                rb.MovePosition(rb.position + movement * spinMovement * Time.fixedDeltaTime);
                break;
            case(PlayerState.Rebound):
                rb.MovePosition(rb.position + bounceDir * reboundMovement * Time.fixedDeltaTime);
                break;
            default:
                break;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (invulnerable) return;
        
        
        
        if (other.CompareTag("Bullet"))
        {
            if (state == PlayerState.Speen)
            {
                Debug.Log("<color=purple> PARRY! </color>");
                BulletBounce(other.transform);
                //other.over
            }
            else
            {
                PlayerDeath();
            }
            
        }
    }

    private void OnParticleTrigger()
    {
        
    }

    void Speen()
    {
        anim.SetTrigger("Speen");

        spinTimer = 0;
        
        state = PlayerState.Speen;
    }

    public void ResetPlayerState()
    {
        //state = PlayerState.Neutral;
    }

    void ProcessFiring()
    {
        ToggleCannon(Input.GetAxis("Fire1") > 0);
    }

    void ToggleCannon(bool isOn)
    {
        foreach (ParticleSystem w in weapons)
        {
            
            var emissionModule = w.emission;
            emissionModule.enabled = isOn;
        }
    }

    

    #region Death Handling

    void PlayerDeath()
    {
        movement = Vector2.zero;
        canMove = false;
        hitbox.enabled = false;
        //Death vfx

        AudioManager.instance.PlaySfx(2, 0.5f);
        
        transform.position = deathPos.localPosition;

        //decrease lives
        //check life count

        if (lifeCount == 0)
        {
            GameManager.Instance.HandleDeath(1f);
        }
        else
        {
            lifeCount--;
            Respawn(1);
        }
        
        
    }

    public void Respawn(float delay = 0)
    {
        Invoke("Respawn", delay);
    }
    
    void Respawn()
    {
        respawnTimer = 0;
        respawning = true;

        invincibleFlashing = true;
        //hitbox.enabled = true;
    }
    
    private void RespawnAnimEnd()
    {
        respawnTimer = 0;
        respawning = false;
        canMove = true;
        
        Invoke("TurnHitboxOn", respawnInvincibility);
    }

    private void TurnHitboxOn()
    {
        invincibleFlashing = false;
        sRend.color = Vector4.one;
        hitbox.enabled = true;
    }
    

    #endregion
    
    
    
    void BulletBounce(Transform obj)
    {
        Debug.Log("<color=pink> BOUNCE! </color>");
        //rb.velocity = new Vector2(10, 3);

        
        rb.velocity = Vector2.zero;
        
        GameManager.Instance.SlowDown();
        
        //Vector2 refract = CalcRefraction(rb.velocity, obj.up);
        
        Vector2 reboundAngle = Vector2.Reflect(movement.normalized, obj.up);

        bounceDir = reboundAngle;
        
        Debug.Log("REBOUND ANGLE: " + reboundAngle);

        aimTimer = 0;
        reboundTimer = 0;
        state = PlayerState.Aiming;
        
        //StartCoroutine(AddVelWithTime(reboundAngle));
        //rb.velocity = reboundAngle * 40;
    }


    void AddVel(Vector2 angle)
    {
        rb.velocity = angle * 40;
    }

    IEnumerator AddVelWithTime(Vector2 ang)
    {
        yield return  new WaitForSeconds(0.8f);
        rb.velocity = ang * 40;
    }

//                                incident    normal
    private Vector2 CalcRefraction(Vector2 u, Vector2 n)
    {
        Vector2 refractedAngle = Vector2.zero;

        
        refractedAngle = u.normalized - (((Mathf.Sqrt(
            ((REFRACTIVE_INDEX * REFRACTIVE_INDEX) - 1) * (u.normalized.magnitude * u.normalized.magnitude) *
            (n.normalized.magnitude * n.normalized.magnitude)
            + (Vector2.Dot(u.normalized, n.normalized) * Vector2.Dot(u.normalized, n.normalized)) -
            (Vector2.Dot(u.normalized, n.normalized))))) / (n.normalized.magnitude * n.normalized.magnitude) * n.normalized);
        
        
        /*
        refractedAngle = u.normalized - (((Mathf.Sqrt(
            ((REFRACTIVE_INDEX * REFRACTIVE_INDEX) - 1) * (u.normalized.magnitude ** 2) *
            (n.normalized.magnitude ** 2)
            + (Vector2.Dot(u.normalized, n.normalized) ** 2) -
            (Vector2.Dot(u.normalized, n.normalized))))) / n.normalized.magnitude ** 2) * n.normalized;
        */
        
        return refractedAngle;
    }
    
    //Debugs

    public void SetLifeCount(int count)
    {
        lifeCount = count;
    }
}
