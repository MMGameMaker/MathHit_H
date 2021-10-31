using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinUI : MonoBehaviour
{
    public GameManager gameManager;
    public void ToNextLvlIndie()
    {
        gameManager.OnGameStateChange.Invoke(GameManager.eGameSates.INDIE);
    }
}
