using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{

    public void GameWin()
    {
        GameManager.Instance.CurrentState = GameManager.eGameSates.GAME_WIN;
    }

    public void GameLose()
    {
        GameManager.Instance.CurrentState = GameManager.eGameSates.GAME_LOSE;
    }

}
