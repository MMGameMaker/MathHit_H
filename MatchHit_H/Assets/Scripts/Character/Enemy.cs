using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public Transform enemyBattlePos;

    public float timeMoveToPos;

    [SerializeField]
    private Player player;

    [SerializeField]
    private GameObject EnemyHealthDecreseTextPrefab;

    public Animator enemyAnim;

    int punchHash = Animator.StringToHash("Hit");

    int beHitHash = Animator.StringToHash("Beaten");

    int deadHash = Animator.StringToHash("Die");

    int victoryHash = Animator.StringToHash("Victory");

    [SerializeField]
    private AnimationClip enemyHitAnim;

    public float playHitAnimLenght;





    // Start is called before the first frame update
    void Start()
    {
        Init();

        enemyAnim = GetComponent<Animator>();

        // Register battle events listener to animation controller functions
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnPlayerHit, (param) => OnPlayerHitAnim());

        // Register TakingDamageEvent
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnEnemyTakingDamage, (param) => OnEnemysTakeDameHandler((int)param));

        playHitAnimLenght = enemyHitAnim.length;

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

    public void OnPlayerHitAnim()
    {
        enemyAnim.SetTrigger(punchHash);
    }

    //will be call in enemy hit Animation event
    public void OnEnemyBeBeatenAnim()
    {
        enemyAnim.SetTrigger(beHitHash);
    }


    //be call in BattleController script - OnBattleEndHandler()
    public void OnEnemyDieAnim()
    {
        enemyAnim.SetBool(deadHash, true);
    }


    //be call in BattleController script - OnBattleEndHandler()
    public void OnEnemyVictoryAnim()
    {
        enemyAnim.SetBool(victoryHash, true);
    }


    public void OnEnemysTakeDameHandler(int playerDamage)
    {
        TakeDame(playerDamage);
        OnEnemyBeBeatenAnim();
        InitHealthTextPop(EnemyHealthDecreseTextPrefab, this.transform.position + new Vector3(0, 5f, 0));
    }

    //
    public void PostEnemyTakeDamageEvent()
    {
        BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnPlayerTakingDamage, Damage);
    }

    private void InitHealthTextPop(GameObject textObject, Vector3 starPos)
    {
        GameObject newText = GameObject.Instantiate(textObject, starPos, Quaternion.identity);
    }





}
