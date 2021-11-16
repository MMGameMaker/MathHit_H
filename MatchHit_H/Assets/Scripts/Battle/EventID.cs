using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventID : ScriptableObject
{
    public enum EvenID
    {
        None = 0,
        OnReady,
        OnMatchFinish,
        OnSpecialHit,
        OnPlayerHit,
        OnEnemyTakingDamage,
        OnEnemyHit,
        OnPlayerTakingDamage,
        OnBattleShow,
        OnBattleEnd,
        OnPlayerDie,
        OnEnemyDie,
    }
}
