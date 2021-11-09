using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
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
        damage = 10;
        recoverRate = 0.5f;
        recoverAmount = 5;
        isAlive = true;
    }

    public override void TakeDame(int damageTaken)
    {
        base.TakeDame(damageTaken);
        Debug.Log("Player take " + damageTaken + " damages!");
    }

    public override void Die()
    {
        base.Die();
        Debug.Log("Character died was Player!");
        BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnPlayerDie);
    }

}
