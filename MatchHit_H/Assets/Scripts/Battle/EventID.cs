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
        OnPlayerHit,
        OnEnemyHit,
        OnBattleShow,
        OnBattleEnd,
        OnPlayerDie,
        OnEnemyDie,
    }
}
