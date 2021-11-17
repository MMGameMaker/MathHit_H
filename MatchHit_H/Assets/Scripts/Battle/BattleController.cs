using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance;

    [SerializeField]
    private Player player;

    [SerializeField]
    private Enemy enemy;

    private bool IsAttacking;

    private bool isBattleShowing;
    public bool IsBattleShowing 
    {
        get => isBattleShowing;
    }

    [SerializeField]
    private GameObject EnemyHealthDecreseTextPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGameStateChange += OnBattleStartHandler;

        // Add Listener to Battle Events
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnMatchFinish, (param) => OnMatchFinish((object) param));
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnPlayerDie, (param) => StartCoroutine("OnPlayerDieHandler"));
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnEnemyDie, (param) => StartCoroutine("OnEnemyDieHandler"));
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnBattleEnd, (param) => OnBattleEndHandler());
    }

    private void OnBattleEndHandler()
    {
        if (GameManager.Instance.GameEnd == GameManager.eGameEnd.Win)
        {
            enemy.OnEnemyDieAnim();
            player.OnPlayerVictoryAnim();
        }
        else if (GameManager.Instance.GameEnd == GameManager.eGameEnd.Lose)
        {
            player.OnPlayerDieAnim();
            enemy.OnEnemyVictoryAnim();
        }
    }

    public void OnBattleStartHandler(GameManager.eGameSates currentSate, GameManager.eGameSates lastState)
    {
        if (currentSate == GameManager.eGameSates.GAME_STARTED)
            StartCoroutine(BattlePrepare());    
    }

    private IEnumerator BattlePrepare()
    {
        //enemy prepare
//        enemy.EnemyPrepare();
        StartCoroutine(enemy.MoveToBattlePosition());
        Debug.Log("Enemy Ready!");
        yield return new WaitForSeconds(enemy.timeMoveToPos);

        //player Prepare

        //BoardPrepare

        //Battleready

        //Pust event battle prepare complete
        BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnReady);
    }

    private IEnumerator OnEnemyDieHandler()
    {
        GameManager.Instance.GameEnd = GameManager.eGameEnd.Win;

        yield return new WaitWhile(() => isBattleShowing);

        GameManager.Instance.CurrentState = GameManager.eGameSates.GAME_OVER;
    }

    private IEnumerator OnPlayerDieHandler()
    {
        GameManager.Instance.GameEnd = GameManager.eGameEnd.Lose;

        yield return new WaitWhile(() => isBattleShowing);

        GameManager.Instance.CurrentState = GameManager.eGameSates.GAME_OVER;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PlayerAttack(object para)
    {
        if (!player.IsAlive)
            yield break;

        var message = para as MatchingFinishMessge;
        int _normalHitTimes = message.matchPoint;
        int _specialHitValue = message.specialPoint;
        float _powerUpStepTime = player.PowerUpTime / _normalHitTimes;

        IsAttacking = true;

        player.ShowPowerUpAnim();
        for(int i = 0; i< _normalHitTimes; i++)
        {
            player.transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            yield return new WaitForSeconds(_powerUpStepTime);
        }

        StartCoroutine("ScaleNormalLize");

        while (_normalHitTimes > 0)
        {
            BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnPlayerHit);
            enemy.TakeDame(player.Damage);
            _normalHitTimes--;
            yield return new WaitForSeconds(player.playHitAnimLenght);
        }

        if(_specialHitValue > 0)
        {
            BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnSpecialHit);
            int specialDamage = _specialHitValue * player.Damage;
            player.SpecialDamage = specialDamage;
            enemy.TakeDame(specialDamage);
            yield return new WaitForSeconds(player.specialHitAnimLenght);
        }

        IsAttacking = false;
    }

    private IEnumerator ScaleNormalLize()
    {
        Vector3 curScale = transform.localScale;
        Vector3 normalScale = new Vector3(1,1,1);

        for (float t = 0; t < player.NormalLizeTime; t += Time.deltaTime)
        {
            player.transform.localScale = Vector3.Lerp(curScale, normalScale, t / player.NormalLizeTime);
            yield return 0;
        }
    }

    private IEnumerator EnemyAttack()
    {
        if (!enemy.IsAlive)
            yield break;

        IsAttacking = true;

        int _hitTimes = (int)Random.Range(1, 5);

        while (_hitTimes > 0)
        {
            BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnEnemyHit);
            player.TakeDame(enemy.Damage);
            _hitTimes--;
            yield return new WaitForSeconds(enemy.playHitAnimLenght + 0.1f);
        }

        IsAttacking = false;
    }

    public IEnumerator BattleShow(object para)
    {
        
        isBattleShowing = true;

        BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnBattleShow);

        StartCoroutine("PlayerAttack", para);

        yield return new WaitWhile(() => IsAttacking);

        StartCoroutine("EnemyAttack");

        yield return new WaitWhile(() => IsAttacking);

        BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnBattleEnd);

        isBattleShowing = false;
    }

    public void OnMatchFinish(object para)
    {
        StartCoroutine("BattleShow", para);
    }
}
