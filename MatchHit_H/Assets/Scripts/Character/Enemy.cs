using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100;
        curHealth = maxHealth;
        damage = 15;
        recoverRate = 0.5f;
        recoverAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
