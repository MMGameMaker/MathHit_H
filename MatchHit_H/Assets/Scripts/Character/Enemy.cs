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

    //Animator component and parameterHash
    public Animator enemyAnim;

    int punchHash = Animator.StringToHash("Hit");

    int atkTypeHash = Animator.StringToHash("AtkType");

    int beHitHash = Animator.StringToHash("Beaten");

    int deadHash = Animator.StringToHash("Die");

    int victoryHash = Animator.StringToHash("Victory");

    int movingHash = Animator.StringToHash("Moving");


    [SerializeField]
    private AnimationClip enemyHitAnim;

    public float playHitAnimLenght;


    // Start is called before the first frame update
    void Start()
    {
        Init();

        enemyAnim = GetComponent<Animator>();

        // Register battle events listener to animation controller functions
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnEnemyHit, (param) => OnEnemyHitAnim());

        // Register TakingDamageEvent
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnEnemyTakingDamage, (param) => OnEnemyTakeDameHandler((int)param));
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnEnemyTakingSpecialDamage, (param) => OnEnemyTakeDameHandler((int)param));

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
        damage = 10;
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
        Vector3 starPos = transform.position;
        Vector3 entPos = enemyBattlePos.position;

        enemyAnim.SetBool(movingHash, true);

        for (float t = 0; t < 1 * timeMoveToPos; t += Time.deltaTime)
        {
            this.transform.position = Vector3.Lerp(starPos, entPos, t / timeMoveToPos);
            yield return 0;
        }

        this.transform.position = entPos;

        enemyAnim.SetBool(movingHash, false);
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

    public void OnEnemyHitAnim()
    {
        enemyAnim.SetFloat(atkTypeHash, (int)Random.Range(0, 2));

        enemyAnim.SetTrigger(punchHash);
    }

    //will be call in player hit Animation event
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

    public void OnEnemyTakeDameHandler(int playerDamage)
    {
        OnEnemyBeBeatenAnim();
        InitHealthTextPop(EnemyHealthDecreseTextPrefab, this.transform.position + new Vector3(0, 2f, 0), playerDamage);
    }

    public void PostPlayerTakeDamageEvent()
    {
        BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnPlayerTakingDamage, Damage);
    }

    private void InitHealthTextPop(GameObject textObject, Vector3 starPos, int damageTaken)
    {
        GameObject newText = GameObject.Instantiate(textObject, starPos, Quaternion.identity);

        newText.GetComponent<HealthTextPop>().SetHealthText(damageTaken);
    }

}
