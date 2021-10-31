using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUI : MonoBehaviour
{
    public GameManager gameManager;

    public void GameWin()
    {
        gameManager.OnGameStateChange.Invoke(GameManager.eGameSates.GAME_WIN);
    }

    public void GameLose()
    {
        gameManager.OnGameStateChange.Invoke(GameManager.eGameSates.GAME_LOSE);
    }

}
