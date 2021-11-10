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
    private GameObject PlayerHealthDecreseTextPrefab;

    [SerializeField]
    private GameObject EnemyHealthDecreseTextPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.OnGameStateChange += OnBattleStartHandler;

        // Add Listener to Battle Events
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnMatchFinish, (param) => OnMatchingFinish((int) param));
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnPlayerDie, (param) => StartCoroutine("OnPlayerDieHandler"));
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnEnemyDie, (param) => StartCoroutine("OnEnemyDieHandler"));
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnBattleEnd, (param) => OnBattleEndHandler());
    }

    private void OnBattleEndHandler()
    {
        if (GameManager.Instance.GameEnd == GameManager.eGameEnd.Win)
        {
            enemy.gameObject.SetActive(false);
        }
        else if (GameManager.Instance.GameEnd == GameManager.eGameEnd.Lose)
        {
            player.gameObject.SetActive(false);
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
        BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnReady);
    }

    private IEnumerator OnEnemyDieHandler()
    {
        GameManager.Instance.GameEnd = GameManager.eGameEnd.Win;

        yield return new WaitWhile(() => isBattleShowing);

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.CurrentState = GameManager.eGameSates.GAME_OVER;
    }

    private IEnumerator OnPlayerDieHandler()
    {
        GameManager.Instance.GameEnd = GameManager.eGameEnd.Lose;

        yield return new WaitWhile(() => isBattleShowing);

        yield return new WaitForSeconds(0.5f);

        GameManager.Instance.CurrentState = GameManager.eGameSates.GAME_OVER;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PlayerAttack(int hitTimes)
    {
        if (!player.IsAlive)
            yield break;

        IsAttacking = true;
        int _hitTimes = hitTimes;

        while(_hitTimes > 0)
        {
            InitHealthTextPop(EnemyHealthDecreseTextPrefab, enemy.transform.position + new Vector3(0, 0.1f, 0));
            enemy.TakeDame(player.Damage);
            BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnPlayerHit);
            _hitTimes--;
            yield return new WaitForSeconds(0.5f);
        }

        IsAttacking = false;
    }

    private IEnumerator EnemyAttack()
    {
        if (!enemy.IsAlive)
            yield break;

        IsAttacking = true;

        int _hitTimes = (int)Random.Range(1, 5);

        while (_hitTimes > 0)
        {
            player.TakeDame(enemy.Damage);
            InitHealthTextPop(PlayerHealthDecreseTextPrefab, player.transform.position + new Vector3(0, 0.1f, 0));
            BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnEnemyHit);
            _hitTimes--;
            yield return new WaitForSeconds(0.5f);
        }

        IsAttacking = false;
    }

    private void InitHealthTextPop(GameObject textObject, Vector3 starPos)
    {
        GameObject newText = GameObject.Instantiate(textObject, starPos, Quaternion.identity);
    }


    public IEnumerator BattleShow(int matchPoint)
    {
        isBattleShowing = true;

        BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnBattleShow);

        StartCoroutine("PlayerAttack", matchPoint);

        yield return new WaitWhile(() => IsAttacking);

        StartCoroutine("EnemyAttack");

        yield return new WaitWhile(() => IsAttacking);

        BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnBattleEnd);

        isBattleShowing = false;
    }

    public void OnMatchingFinish (int matchPoint)
    {
        StartCoroutine("BattleShow", matchPoint);
    }
}
