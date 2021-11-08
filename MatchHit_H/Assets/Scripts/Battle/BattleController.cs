using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField]
    private Character player;

    [SerializeField]
    private Character enemy;

    private bool IsAttacking;

    // Start is called before the first frame update
    void Start()
    {
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnMatchFinish, (param) => OnMatchingFinish((int) param));
        BattleEventDispatcher.Instance.RegisterListener(EventID.EvenID.OnBattelEnd, (param) => OnBattleFinish());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator PlayerAttack(int hitTimes)
    {
        IsAttacking = true;
        int _hitTimes = hitTimes;

        while(_hitTimes > 0)
        {
            enemy.TakeDame(player.Damage);
            yield return new WaitForSeconds(0.5f);
            BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnPlayerHit);
            _hitTimes--;
        }

        IsAttacking = false;
    }

    private IEnumerator EnemyAttack()
    {
        int _hitTimes = (int)Random.Range(1, 4);

        while (_hitTimes > 0)
        {
            player.TakeDame(enemy.Damage);
            yield return new WaitForSeconds(0.5f);
            BattleEventDispatcher.Instance.PostEvent(EventID.EvenID.OnEnemyHit);
            _hitTimes--;
        }
    }

    public IEnumerator BattleShow(int matchPoint)
    {
        StartCoroutine("PlayerAttack", matchPoint);

        yield return new WaitWhile(() => IsAttacking);

        StartCoroutine("EnemyAttack");
    }


    public void OnMatchingFinish (int matchPoint)
    {
        StartCoroutine("BattleShow", matchPoint);
    }

    public void OnBattleFinish()
    {
        StopCoroutine("BattleShow");
    }

}
