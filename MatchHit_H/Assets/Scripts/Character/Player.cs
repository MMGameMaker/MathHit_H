using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private Enemy enemy;

    [SerializeField]
    private GameObject PlayerHealthDecreseTextPrefab;

    public Animator playerAnim;

    int powerUpHash = Animator.StringToHash("PowerUp");

    int hitHash = Animator.StringToHash("Hit");

    int atkTypeHash = Animator.StringToHash("AtkType");

    int beHitHash = Animator.StringToHash("Beaten");

    int deadHash = Animator.StringToHash("Die");

    int victoryHash = Animator.StringToHash("Victory");

    int specialHitHash = Animator.StringToHash("SpecialHit");

    [SerializeField]
    private float powerUpTime;

    public float PowerUpTime 
    {
        get => this.powerUpTime;
        set { this.powerUpTime = value; }
    }


    [SerializeField]
    private float normalLizeTime;

    public float NormalLizeTime
    {
        get => this.normalLizeTime;
    }

    [SerializeField]
    private AnimationClip playerHitAnim;
    
    public float playHitAnimLenght;

    [SerializeField]
    private AnimationClip specialHitAnim;

    private int specialDamage;

    public int SpecialDamage 
    {
        get => this.specialDamage;
        set { this.specialDamage = value; }
    }

    public float specialHitAnimLenght;

    // Start is called before the first frame update
    void Start()
    {
        Init();

        playerAnim = GetComponent<Animator>();

        // Register battle events listener to animation controller functions
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnPlayerHit, (param) => OnPlayerHitAnim());
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnSpecialHit, (param) => OnSpecialHitAnim());


        // Register TakingDamageEvent
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnPlayerTakingDamage, (param) => OnPlayerTakeDameHandler((int) param));

        playHitAnimLenght = playerHitAnim.length;
        specialHitAnimLenght = specialHitAnim.length;
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


    public void ShowPowerUpAnim()
    {
        playerAnim.SetTrigger(powerUpHash);
    }

    public void OnPlayerHitAnim()
    {
        playerAnim.SetFloat(atkTypeHash, (int)Random.Range(0, 4));
        
        playerAnim.SetTrigger(hitHash);
    }

    public void OnSpecialHitAnim()
    {
        playerAnim.SetTrigger(specialHitHash);
    }


    //will be call in enemy hit Animation event
    public void OnPlayerBeBeatenAnim()
    {
        playerAnim.SetTrigger(beHitHash);
    }


    //be call in BattleController script - OnBattleEndHandler()
    public void OnPlayerDieAnim()
    {
        playerAnim.SetBool(deadHash, true);
    }


    //be call in BattleController script - OnBattleEndHandler()
    public void OnPlayerVictoryAnim()
    {
        playerAnim.SetBool(victoryHash, true);
    }

    public void OnPlayerTakeDameHandler(int enemyDamage)
    {
        OnPlayerBeBeatenAnim();

        InitHealthTextPop(PlayerHealthDecreseTextPrefab, this.transform.position + new Vector3(0, 2f, 0), enemyDamage);
    }

    // Post event while normal hit animations play
    public void PostEnemyTakeDamageEvent()
    {
        BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnEnemyTakingDamage, Damage);
    }

    // Post event while special hit animations play
    public void PostEnemyTakeSpecialDamageEvent()
    {
        BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnEnemyTakingSpecialDamage, SpecialDamage);
    }

    private void InitHealthTextPop(GameObject textObject, Vector3 starPos, int damageTaken)
    {
        GameObject newText = GameObject.Instantiate(textObject, starPos, Quaternion.identity);

        newText.GetComponent<HealthTextPop>().SetHealthText(damageTaken);
    }


}
