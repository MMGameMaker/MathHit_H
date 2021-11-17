using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventID : ScriptableObject
{
    public enum EvenID
    {
        None = 0,
        OnReady,
        OnClearMatchList,
        OnMatchFinish,
        OnSpecialHit,
        OnPlayerHit,
        OnEnemyTakingDamage,
        OnEnemyTakingSpecialDamage,
        OnEnemyHit,
        OnPlayerTakingDamage,
        OnBattleShow,
        OnBattleEnd,
        OnPlayerDie,
        OnEnemyDie,
    }
}
