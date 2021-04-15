using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    private Enemy attachedEnemy;
    [SerializeField] private Transform fill;
    private float startingXScale;
    private float enemyMaxHp;

    // Start is called before the first frame update
    void Start()
    {
        attachedEnemy = GetComponentInParent<Enemy>();
        startingXScale = fill.localScale.x;
        enemyMaxHp = attachedEnemy.health;
    }

    // Update is called once per frame
    void Update()
    {
        fill.localScale = new Vector2((attachedEnemy.health/enemyMaxHp) * startingXScale, fill.localScale.y);
    }
}
