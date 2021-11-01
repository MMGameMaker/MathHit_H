using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    [Serializable]
    public enum eGameSates
    {
        INDIE,
        LOADING,
        BATTLE_STARTED,
        GAME_WIN,
        GAME_LOSE,
    }

    public eGameSates _gameState;

    public UnityEvent<eGameSates> OnGameStateChange;

    public static GameManager instance;


    private void Awake()
    {
        if (instance != null && instance != this)
             Destroy(gameObject);

        instance = this;

        OnGameStateChange.AddListener(GameStateChange);
    }


   public void GameStateChange(eGameSates gameSates)
    {
        this._gameState = gameSates;
    }

    private void Start()
    {
        OnGameStateChange.Invoke(eGameSates.INDIE);
        Debug.Log("Game Start");
    }

}
