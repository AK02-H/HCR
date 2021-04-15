using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Enemy : MonoBehaviour
{
    public float health = 25f;

    [SerializeField] private int scoreValue = 100;
    
    public bool impervious;

    private Material mat;  
    public BulletPattern[] Attacks;

    private float tempAtkInterval;

    public bool playOnAwake = false;
    public int defaultAtk = 0;


    private GameObject hpBar;

    private bool isDamageFlashing;
    private float delayBetweenDamageFlash = 0.15f;    //hard coded
    
    // Start is called before the first frame update
    void Start()
    {
        CoreStart();
    }

    protected void CoreStart()
    {
        //LaunchAttack(0);

        mat = GetComponent<SpriteRenderer>().material;
        hpBar = GetComponentInChildren<EnemyHp>().gameObject;

        hpBar.SetActive(false);
        if (playOnAwake)
        {
            LaunchAttack(defaultAtk);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CoreUpdate();
    }

    protected void CoreUpdate()
    {
        if (isDamageFlashing)
        {
            delayBetweenDamageFlash -= Time.deltaTime;

            if (delayBetweenDamageFlash <= 0)
            {
                mat.SetFloat("_FlashAmount", 0f);
                delayBetweenDamageFlash = 0.15f;
                isDamageFlashing = false;
            }
        }
    }


    public void LaunchAttack(int atkId = 0)
    {
        if (Attacks.Length == 0)
        {
            Debug.LogWarning("WARNING: No attack patterns set");
            return;
        }
        
        atkId = Mathf.Clamp(atkId, 0, Attacks.Length);
        
        Attacks[atkId].FirePattern();
    }
    
    public void StopAttack(int atkId = 0)
    {
        if (Attacks.Length == 0)
        {
            Debug.LogWarning("WARNING: No attack patterns set");
            return;
        }
        
        atkId = Mathf.Clamp(atkId, 0, Attacks.Length);
        
        Attacks[atkId].StopFiring();
    }

    public void SetInterval(float value)
    {
        tempAtkInterval = value;
    }

    public void LaunchAttackRepeating(int atkId = 0)
    {
        if (Attacks.Length == 0)
        {
            Debug.LogWarning("WARNING: No attack patterns set");
            return;
        }
        
        atkId = Mathf.Clamp(atkId, 0, Attacks.Length);
        
        Attacks[atkId].FireWithInterval(tempAtkInterval);
    }
    
    
    void OnParticleCollision(GameObject other)
    {
        if (!impervious)
        {
            PlayerController
                player = other
                    .GetComponentInParent<PlayerController>(); //Is this more efficient than calling a singleton?

            health -= player.AttackStat;

            mat.SetFloat("_FlashAmount", 0.4f);
            isDamageFlashing = true;

            
            if (health <= 0)
            {
                if(GameManager.Instance) GameManager.Instance.Score += scoreValue;
                Destroy(gameObject);
            }
        }


        if (!hpBar.activeSelf)
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy e in enemies)
            {
                e.HideHpBar();
            }
            ShowHpBar();
        }
        
        
    }

    public void EnemyExit()
    {
        Destroy(gameObject);
    }

    public void Test1()
    {
        Debug.LogWarning("Enemy entered");
    }

    public void Test2()
    {
        Debug.LogWarning("Enemy Left");
    }

    public virtual void JumpToTimelinePoint(float timeStamp)
    {
        WaveManager.Instance.JumpToTimestamp(timeStamp);
    }

    public void JumpToThisTimelinePoint(float time)
    {
        transform.parent.GetComponent<PlayableDirector>().time = time;
    }

    public void EndWave()
    {
        Debug.Log("<color=orange>WAVE AUTOMATICALLY ENDED</color>");
        WaveManager.Instance.CallNextWave();
    }

    public void ShowHpBar()
    {
        hpBar.SetActive(true);
    }
    public void HideHpBar()
    {
        hpBar.SetActive(false);
    }

    public void DisableAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }
}
