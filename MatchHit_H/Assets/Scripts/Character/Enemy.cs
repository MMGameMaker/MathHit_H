using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    
    public Transform enemyBattlePos;

    public float timeMoveToPos;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Init()
    {
        maxHealth = 100;
        curHealth = maxHealth;
        damage = 5;
        recoverRate = 0.5f;
        recoverAmount = 0;
        IsAlive = true;
    }

    public void EnemyPrepare()
    {
        StartCoroutine("MoveToBattlePosition");
    }

    public IEnumerator MoveToBattlePosition()
    {
        Vector2 starPos = transform.position;
        Vector2 entPos = enemyBattlePos.position;

        for (float t = 0; t < 1 * timeMoveToPos; t += Time.deltaTime)
        {
            this.transform.position = Vector2.Lerp(starPos, entPos, t / timeMoveToPos);
            yield return 0;
        }

        this.transform.position = entPos;
    }

    public override void TakeDame(int damageTaken)
    {
        base.TakeDame(damageTaken);
        Debug.Log("Enemy take " + damageTaken + " damages!");
    }

    public override void Die()
    {
        base.Die();
        Debug.Log("Character died was Enemy!");
        BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnEnemyDie);
    }
}
