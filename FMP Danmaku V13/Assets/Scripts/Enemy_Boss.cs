using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;

public class Enemy_Boss : Enemy
{
    private float maxHp;
    
    public int phase;

    [SerializeField] private int tempAtkId;
    public PlayableDirector[] attackPatterns;

    private int currentAttackPattern = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        CoreStart();

        maxHp = health;
        
        for (int i = 0; i < attackPatterns.Length; i++)
        {
            attackPatterns[i].stopped += OnAttackEnd;
        }
        
        //Attack();
    }

    // Update is called once per frame
    
    //Handle most phase changes in update
    void Update()
    {
        CoreUpdate();

        if (phase != 1)
        {
            if (health / maxHp <= 0.5)
            {
                PhaseChange(1, true);
            }
        }
    }

    //this will probably be overrided in every individual boss script
    void Attack()
    {
        
        switch (phase)
        {
            case 0:
                currentAttackPattern = Random.Range(0, 2);
                Debug.Log("Launcing attack " + currentAttackPattern);
                attackPatterns[currentAttackPattern].Play();
                break;
            
            case 1:
                currentAttackPattern = 2;
                attackPatterns[2].Play();
                break;
            
            default:
                attackPatterns[Random.Range(0, attackPatterns.Length)].Play();
                break;
        }
        
        
    }

    public virtual void PhaseChange(int newPhase, bool interruptCurrentAttack = false)
    {
        phase = newPhase;
        if (interruptCurrentAttack)
        {
            attackPatterns[currentAttackPattern].Stop();
            InterruptAction();
            Attack();
        }
    }
    
    void OnAttackEnd(PlayableDirector why)
    {
        Attack();
    }

    void ProcessAttack(int id)
    {
        
    }

    protected virtual void InterruptAction()
    {
        //depend on phase switching to
        StopAllAttacks();
        LeanTween.moveX(gameObject, 0, 0.5f);
    }

    public void StopAllAttacks()
    {
        foreach (var VARIABLE in Attacks)
        {
            VARIABLE.StopFiring();
        }
    }

    public override void JumpToTimelinePoint(float timeStamp)
    {
        attackPatterns[currentAttackPattern].time = timeStamp;
    }
}
