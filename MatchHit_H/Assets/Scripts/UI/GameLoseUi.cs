using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoseUi : MonoBehaviour
{
    public GameManager gameManager;
    public void Revival()
    {
        gameManager.OnGameStateChange.Invoke(GameManager.eGameSates.BATTLE_STARTED);
    }

    public void Home()
    {
        gameManager.OnGameStateChange.Invoke(GameManager.eGameSates.INDIE);
    }
}
