using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected int maxHealth;
    public int MaxHealth 
    {
        get => this.maxHealth;
        protected set { this.maxHealth = value; }
    }

    protected int curHealth;
    public int CurHealth 
    { 
        get => this.curHealth;
        protected set { this.curHealth = value; } 
    }

    protected float recoverRate;
    public float RecoverRate { get => recoverRate; }

    protected int recoverAmount;
    public int RecoverAmount 
    {
        get => recoverAmount;
        protected set { recoverAmount = value; } 
    }

    protected int damage;
    public int Damage
    {
        get => this.damage;
        protected set { this.damage = value; }
    }

    public virtual void TakeDame(int damageTaken)
    {
        CurHealth -= damageTaken;

        if (CurHealth <= 0)
        {
            CurHealth = 0;
            Die();
        }
    }

    public virtual void HealthRecover (int recoverAmount)
    {
        CurHealth += recoverAmount;
        if(CurHealth >= maxHealth)
        {
            CurHealth = maxHealth;
        }
    }

    public bool IsAlive()
    {
        return curHealth > 0;
    }

    public void Die()
    {
        BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnBattelEnd);
    }

}
